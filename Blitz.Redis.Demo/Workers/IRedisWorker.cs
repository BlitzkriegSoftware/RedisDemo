using System;
using System.Collections.Generic;
using System.Text;

namespace Blitz.Redis.Demo.Workers
{
    public interface IRedisWorker
    {
        void Run(Models.CommandOptions o);
    }
}
