# Microservices.demo1 
Ocelot consul identityserver4 rabitmq redis docker

getway:
http://localhost:5000/up/weatherforecast

service1:
http://localhost:8010/api/weatherForecast

service2:
http://localhost:8011/api/weatherForecast

# 运行步骤
### 创建一个webapi做为网关并集成ocelot
> 注意：ocelot是网关，对服务的配置通过json文件进行，需要手动配置服务的ip和端口，而且也没有健康检查等，所以需要配合consul等服务治理一起享用。
- 安装consul,docker:https://hub.docker.com/_/consul?tab=description
```
docker pull consul:1.8.0
docker run -d -e CONSUL_BIND_INTERFACE=eth0 -p 8500:8500 --name=consul consul:1.8.0 agent -server -ui -node=server-1 -client=0.0.0.0 -datacenter=dc1 -dev 
docker run -d -e CONSUL_BIND_INTERFACE=eth0 consul:1.8.0 agent -server -ui -node=server-2 -client=0.0.0.0 -datacenter=dc1 -join 172.17.0.9 -dev
docker run -d -e CONSUL_BIND_INTERFACE=eth0 consul:1.8.0 agent -server -ui -node=server-3 -client=0.0.0.0 -datacenter=dc1 -join 172.17.0.2 -dev
-dev 参数带上后，不会再往硬盘写入信息
-bootstrap_expect=3 service必须有3个以上才能选出leader
docker exec consul consul members
docker exec consul consul operator raft list-peers
```
### 集成到ocelot里
```
"GlobalConfiguration": {
    //"BaseUrl": "http://localhost:5000",
    "ServiceDiscoveryProvider": {
      "Host": "localhost",
      "Port": 8500,
      "Type": "Consul"
      //"Type": "PollConsul",  //修改策略为轮询，这个性能比较好
      //"PollingInterval": 100  //轮询间隔
    }
  }
//同时需要配置ServiceName,对应consul里的服务名
```
### service的api注册到consul里
> Consul的http api完成注册或发现，也可以写配置文件手动定义服务的ip和端口
- 方式一：通过http api来注册 `PUT 到 http://localhost:8500/v1/agent/service/register`
```config
{
    "id": "service1",
    "name": "service1",
    "address": "localhost",
    "port": 8010,
    "tags": ["dev"],
    "checks": 
    [
        {
            "http": "http://localhost:8010/api/health","interval": "5s"
        }
    ]
}
```
删除：通过http api方式：
`PUT 到 http://localhost:8500/v1/agent/service/deregister/{服务实例ID}`

- 方式二：通过配置文件（需要配置中心和git支持）
- 方式三（推荐、侵入式）：通过代码实现注册和自动删除

# id4s实现

# ocelot配置
### 服务A调用服务B

### ocelot的超时、熔断、限流、降级、聚合请求
- 限流
```
"RateLimitOptions": {
        "ClientWhitelist": [ "one", "two" ], // 白名单:在请求头中包含ClientId=xxx的请求不受限流控制
        "EnableRateLimiting": true, //开启限流or not
        "Period": "1m", //1s, 5m, 1h, 1d
        "PeriodTimespan": 30, // 几秒后可重试
        "Limit": 10 // 最大请求数
      }
```
如果配置在GlobalConfiguration节点里，将是全局配置
```
"ClientIdHeader": "appKey",
"HttpStatusCode": 429,

```
- 熔断
```
"QoSOptions": {
      "ExceptionsAllowedBeforeBreaking": 3, //允许几个异常
      "DurationOfBreak": 10000, // 熔断时间，ms
      "TimeoutValue": 4000 //请求超时时间
     }
```
### ocelot的缓存
Ocelot目前支持对下游服务的URL进行缓存，并可以设置一个以秒为单位的TTL使缓存过期。 您也可以通过调用Ocelot的管理API来清除某个Region的缓存。

- 作用：

- 配置：
在ConfigureServices里添加：`.AddCacheManager(x => { x.WithDictionaryHandle(); })`
修改`ocelot.json`文件，在Routes的数组内增加以下内容：
```
"FileCacheOptions": {
       "TtlSeconds": 15,
       "Region": "Region1" 
}
```

# Apollo配置中心

# swagger配置

# 参考资料
- [consul文档](https://www.consul.io/docs/agent/options.html)
- 
