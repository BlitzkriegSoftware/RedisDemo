using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blitz.Redis.WebDemo.Libs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Blitz.Redis.WebDemo
{
    /// <summary>
    /// Main entry class
    /// </summary>
    public class Program
    {
        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Create Host Builder
        /// </summary>
        /// <param name="args"></param>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


        private static Models.BlitzAssemblyVersionMetadata _blitzassemblyversionmetadata = null;

        /// <summary>
        /// Semantic Version, etc from Assembly Metadata
        /// </summary>
        public static Models.BlitzAssemblyVersionMetadata ProgramMetadata
        {
            get
            {
                if (_blitzassemblyversionmetadata == null)
                {
                    _blitzassemblyversionmetadata = new Models.BlitzAssemblyVersionMetadata();
                    var assembly = typeof(Program).Assembly;
                    foreach (var attribute in assembly.GetCustomAttributesData())
                    {
                        if (!attribute.TryParse(out string value))
                        {
                            value = string.Empty;
                        }
                        var name = attribute.AttributeType.Name;
                        System.Diagnostics.Trace.WriteLine($"{name}, {value}");
                        _blitzassemblyversionmetadata.PropertySet(name, value);
                    }
                }
                return _blitzassemblyversionmetadata;
            }
        }

    }
}
