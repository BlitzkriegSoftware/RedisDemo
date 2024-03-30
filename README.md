# RedisDemo
A demo of using *REDIS*/*VALKEY* from C# in .NET 

## Prerequisites

1. Install Docker or your favorite container engine
2. Start Docker
3. Start REDIS or Valkey in your docker container

### Start REDIS or VALKEY

```powershell
.\scripts\docker_redis_start.ps1
```

-or-

```powershell
.\scripts\valkey_start.ps1
```

### Stop REDIS

```powershell
.\scripts\docker_redis_stop.ps1
```

-or-

```powershell
.\scripts\valkey_stop.ps1
```


## Docker Connection String

Your REDIS/VALKEY container instance will have the following connection string:

```text
127.0.0.1:6379
```

## Demo: Unit Tests (really integration tests)

i. Using the visual unit test blade run or debug through the unit tests

ii. See the test code in `Tests` in `TestRedisLibrary.cs`


# About 

Stuart Williams / Blitzkrieg Software

* [E-Mail](mailto:Stuart.T.Williams@outlook.com)
* [LinkedIn](http://lnkd.in/P35kVT)
* [YouTube](https://www.youtube.com/user/spookdejur1962/videos)
* [GitHub](https://github.com/BlitzkriegSoftware)