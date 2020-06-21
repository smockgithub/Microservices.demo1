# Microservices.demo1 
Ocelot consul identityserver4 rabitmq redis docker

getway:
http://localhost:5000/up/weatherforecast

service1:
http://localhost:8010/api/weatherForecast

service2:
http://localhost:8012/api/weatherForecast

# ���в���
- ����һ��webapi��Ϊ���ز�����ocelot
> ע�⣺ocelot�����أ��Է��������ͨ��json�ļ����У���Ҫ�ֶ����÷����ip�Ͷ˿ڣ�����Ҳû�н������ȣ�������Ҫ���consul�ȷ�������һ�����á�
- ��װconsul,docker:https://hub.docker.com/_/consul?tab=description
> docker pull consul:1.8.0
> docker run -d -p 8500:8500 --name=consul consul:1.8.0 agent -server -ui -node=server-1 -client=0.0.0.0 -datacenter=dc1 -dev
> docker run -d consul:1.8.0 agent -server -ui -node=server-2 -client=0.0.0.0 -datacenter=dc1 -join 172.17.0.2 -dev
> docker run -d consul:1.8.0 agent -server -ui -node=server-3 -client=0.0.0.0 -datacenter=dc1 -join 172.17.0.2 -dev
> -dev �������Ϻ󣬲�������Ӳ��д����Ϣ
> docker exec consul consul members
> docker exec consul consul operator raft list-peers
- ���ɵ�ocelot��

- service��apiע�ᵽconsul��
> Consul��http api���ע����֣�Ҳ����д�����ļ��ֶ���������ip�Ͷ˿�
��ʽһ��ͨ��http api��ע��
> PUT �� http://localhost:8500/v1/agent/service/register
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
ɾ����ͨ��http api��ʽ��
> PUT �� http://localhost:8500/v1/agent/service/deregister/{����ʵ��ID}
