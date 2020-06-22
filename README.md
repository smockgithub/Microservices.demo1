# Microservices.demo1 
Ocelot consul identityserver4 rabitmq redis docker

getway:
http://localhost:5000/up/weatherforecast

service1:
http://localhost:8010/api/weatherForecast

service2:
http://localhost:8011/api/weatherForecast

# ���в���
### ����һ��webapi��Ϊ���ز�����ocelot
> ע�⣺ocelot�����أ��Է��������ͨ��json�ļ����У���Ҫ�ֶ����÷����ip�Ͷ˿ڣ�����Ҳû�н������ȣ�������Ҫ���consul�ȷ�������һ�����á�
- ��װconsul,docker:https://hub.docker.com/_/consul?tab=description
```
docker pull consul:1.8.0
docker run -d -e CONSUL_BIND_INTERFACE=eth0 -p 8500:8500 --name=consul consul:1.8.0 agent -server -ui -node=server-1 -client=0.0.0.0 -datacenter=dc1 -dev 
docker run -d -e CONSUL_BIND_INTERFACE=eth0 consul:1.8.0 agent -server -ui -node=server-2 -client=0.0.0.0 -datacenter=dc1 -join 172.17.0.9 -dev
docker run -d -e CONSUL_BIND_INTERFACE=eth0 consul:1.8.0 agent -server -ui -node=server-3 -client=0.0.0.0 -datacenter=dc1 -join 172.17.0.2 -dev
-dev �������Ϻ󣬲�������Ӳ��д����Ϣ
-bootstrap_expect=3 service������3�����ϲ���ѡ��leader
docker exec consul consul members
docker exec consul consul operator raft list-peers
```
### ���ɵ�ocelot��
```
"GlobalConfiguration": {
    //"BaseUrl": "http://localhost:5000",
    "ServiceDiscoveryProvider": {
      "Host": "localhost",
      "Port": 8500,
      "Type": "Consul"
      //"Type": "PollConsul",  //�޸Ĳ���Ϊ��ѯ��������ܱȽϺ�
      //"PollingInterval": 100  //��ѯ���
    }
  }
//ͬʱ��Ҫ����ServiceName,��Ӧconsul��ķ�����
```
### service��apiע�ᵽconsul��
> Consul��http api���ע����֣�Ҳ����д�����ļ��ֶ���������ip�Ͷ˿�
- ��ʽһ��ͨ��http api��ע�� `PUT �� http://localhost:8500/v1/agent/service/register`
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
ɾ����ͨ��http api��ʽ��
`PUT �� http://localhost:8500/v1/agent/service/deregister/{����ʵ��ID}`

- ��ʽ����ͨ�������ļ�����Ҫ�������ĺ�git֧�֣�
- ��ʽ�����Ƽ�������ʽ����ͨ������ʵ��ע����Զ�ɾ��

# id4sʵ��

# ocelot����
### ����A���÷���B

### ocelot�ĳ�ʱ���۶ϡ��������������ۺ�����
- ����
```
"RateLimitOptions": {
        "ClientWhitelist": [ "one", "two" ], // ������:������ͷ�а���ClientId=xxx����������������
        "EnableRateLimiting": true, //��������or not
        "Period": "1m", //1s, 5m, 1h, 1d
        "PeriodTimespan": 30, // ����������
        "Limit": 10 // ���������
      }
```
���������GlobalConfiguration�ڵ������ȫ������
```
"ClientIdHeader": "appKey",
"HttpStatusCode": 429,

```
- �۶�
```
"QoSOptions": {
      "ExceptionsAllowedBeforeBreaking": 3, //�������쳣
      "DurationOfBreak": 10000, // �۶�ʱ�䣬ms
      "TimeoutValue": 4000 //����ʱʱ��
     }
```
### ocelot�Ļ���
OcelotĿǰ֧�ֶ����η����URL���л��棬����������һ������Ϊ��λ��TTLʹ������ڡ� ��Ҳ����ͨ������Ocelot�Ĺ���API�����ĳ��Region�Ļ��档

- ���ã�

- ���ã�
��ConfigureServices����ӣ�`.AddCacheManager(x => { x.WithDictionaryHandle(); })`
�޸�`ocelot.json`�ļ�����Routes�������������������ݣ�
```
"FileCacheOptions": {
       "TtlSeconds": 15,
       "Region": "Region1" 
}
```

# Apollo��������

# swagger����

# �ο�����
- [consul�ĵ�](https://www.consul.io/docs/agent/options.html)
- 
