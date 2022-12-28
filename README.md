# RedisDemo
A demo of using *REDIS* from C# in .NET Core 3.1 LTS

## Prerequisites

1. Install Docker
2. Start Docker
3. Start REDIS in your docker container

```bash
./scripts/docker_redis_start.sh
```

To stop the container:
```bash
./scripts/docker_redis_stop.sh
```

## Docker REDIS Connection String

Your REDIS instance will have the following connection string:

```text
127.0.0.1:6379
```

## Demo: Command Line

> Set the `BlitzkriegSoftware.Redis.Demo` as the start up project

### Usage

```bash
$ ./BlitzkriegSoftware.Redis.Demo.exe -help
```
```text
BlitzkriegSoftware.Redis.Demo Copyright c 2020 Blitzkrieg Software 1.1.1
Demo using REDIS in DotNet Core
 Copyright c 2020 Blitzkrieg Software
BlitzkriegSoftware.Redis.Demo 1.1.1
Copyright c 2020 Blitzkrieg Software

  -v, --verbose             Set output to verbose messages.

  -m, --MessageCount        (Default: 50) How many messages to generate

  -c, --ConnectionString    (Default: 127.0.0.1:6379) Redis Commandline

  --help                    Display this help screen.

  --version                 Display version information.
```

### Sample

```bash
$ ./BlitzkriegSoftware.Redis.Demo.exe -m 9 -c "127.0.0.1:6379" -v
```
```text
BlitzkriegSoftware.Redis.Demo Copyright c 2020 Blitzkrieg Software 1.1.1
Demo using REDIS in DotNet Core
 Copyright c 2020 Blitzkrieg Software
BlitzkriegSoftware.Redis.Demo --ConnectionString 127.0.0.1:6379 --MessageCount 9 --verbose
info: BlitzkriegSoftware.Redis.Demo.Workers.RedisWorker[0]
      0=Doloribus praesentium quaeratvoluptatem consequatur etiam hac saepeeveniet quidolorem sapien consequat sunt dolore.
info: BlitzkriegSoftware.Redis.Demo.Workers.RedisWorker[0]
      1=Incidunt dictumst assumenda eos sollicitudin accusantiumdoloremque ipsum id diam consequaturquis facilisi harumquidem ornare.
info: BlitzkriegSoftware.Redis.Demo.Workers.RedisWorker[0]
      2=Quisquam etaccusamus egestas conubia illo est est.
info: BlitzkriegSoftware.Redis.Demo.Workers.RedisWorker[0]
      3=Mattis auctor sociosqu.
info: BlitzkriegSoftware.Redis.Demo.Workers.RedisWorker[0]
      4=Cubilia etiam rem mi nonrecusandae eligendi delectus venenatis felis hendrerit massa perferendis.
info: BlitzkriegSoftware.Redis.Demo.Workers.RedisWorker[0]
      5=Eos magnam nemo potenti aliquam mi voluptas consequuntur.
info: BlitzkriegSoftware.Redis.Demo.Workers.RedisWorker[0]
      6=Ac utaut pariatur similique.
info: BlitzkriegSoftware.Redis.Demo.Workers.RedisWorker[0]
      7=Aspernaturaut fuga tempora.
info: BlitzkriegSoftware.Redis.Demo.Workers.RedisWorker[0]
      8=Potenti odio cum ex tenetur.
```

### What is it doing?

1. Does the usual .NET core startup including wiring up logging
2. Creates an entry for our worker
3. Parses command line for arguments
4. Starts the worker with the arguments
5. Creates `-m` key/value pairs and writes and reads them to REDIS, and for each verifies what we got out is what we put in.

## Demo: Unit Tests

i. Using the visual unit test blade run or debug through the unit tests

ii. See the test code in `Tests` in `TestRedisLibrary.cs`

## Demo: Web

> Set the start up project to `BlitzkriegSoftware.Redis.WebDemo`

1. Press **F5** 
2. Go to the `/swagger` page 
3. Try the `Stach` and `Fetch` functions

### What is it doing?

a. In `startup.cs` lines 70-74 we 

```cs
_ = services.AddStackExchangeRedisCache(options => {
   options.Configuration = "localhost";
   options.InstanceName = "SampleInstance";
});
```

which will use REDIS as an `IDistributedCache` using the default localhost connection string, and an `InstanceName` that in this case is required but meaningless.

b. In `Controllers/ValuesController.cs` in the **CTOR** we inject `IDistributedCache` so we can use it to set and get values

```cs
// Set a value
this._cache.SetString(name, value);

// Get a value
var value = this._cache.GetString(name);
```

## BlitzkriegSoftware.Redis.Library

This is a quick facade over the exellent StackExchange REDIS client: https://github.com/StackExchange/StackExchange.Redis/



# About 

Stuart Williams

* Cloud/DevOps Practice Lead

* Magenic Technologies Inc.
* Office of the CTO

* [e-mail](stuartw@magenic.com)

* [Blog](https://blitzkriegsoftware.azurewebsites.net/Blog) 
* [LinkedIn](http://lnkd.in/P35kVT)

* [YouTube](https://www.youtube.com/user/spookdejur1962/videos)
