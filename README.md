# Microservices.demo1 
Ocelot consul identityserver4 rabitmq redis docker

getway:
http://localhost:5000/up/weatherforecast

service1:
http://localhost:8010/api/weatherForecast

service2:
http://localhost:8012/api/weatherForecast

# 运行步骤
- 创建一个webapi做为网关并集成ocelot
> 注意：ocelot是网关，对服务的配置通过json文件进行，需要手动配置服务的ip和端口，而且也没有健康检查等，所以需要配合consul等服务治理一起享用。
- 安装consul,docker:https://hub.docker.com/_/consul?tab=description
> docker pull consul:1.8.0
> docker run -d -p 8500:8500 --name=consul consul:1.8.0 agent -server -ui -node=server-1 -client=0.0.0.0 -datacenter=dc1 -dev
> docker run -d consul:1.8.0 agent -server -ui -node=server-2 -client=0.0.0.0 -datacenter=dc1 -join 172.17.0.2 -dev
> docker run -d consul:1.8.0 agent -server -ui -node=server-3 -client=0.0.0.0 -datacenter=dc1 -join 172.17.0.2 -dev
> -dev 参数带上后，不会再往硬盘写入信息
> docker exec consul consul members
> docker exec consul consul operator raft list-peers
- 集成到ocelot里

- service的api注册到consul里
> Consul的http api完成注册或发现，也可以写配置文件手动定义服务的ip和端口
方式一：通过http api来注册
> PUT 到 http://localhost:8500/v1/agent/service/register
```{
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
}```
删除：通过http api方式：
> PUT 到 http://localhost:8500/v1/agent/service/deregister/{服务实例ID}
