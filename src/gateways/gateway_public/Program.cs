using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.Middleware;
using Ocelot.DependencyInjection;

namespace gateway_public
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            //https://ocelot.readthedocs.io/en/latest/introduction/gettingstarted.html#net-core-3-1

            //new WebHostBuilder()
            //   .UseKestrel()
            //   .UseContentRoot(Directory.GetCurrentDirectory())
            //   .ConfigureAppConfiguration((hostingContext, config) =>
            //   {
            //       config
            //           .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
            //           .AddJsonFile("appsettings.json", true, true)
            //           .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
            //           .AddJsonFile("ocelot.json")
            //           .AddEnvironmentVariables();
            //   })
            //   .ConfigureServices(s => {
            //       s.AddOcelot();
            //   })
            //   .ConfigureLogging((hostingContext, logging) =>
            //   {
            //       //add your logging
            //   })
            //   .UseIISIntegration()
            //   .Configure(app =>
            //   {
            //       app.UseOcelot().Wait();
            //   })
            //   .Build()
            //   .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //add ocelot config
                .ConfigureAppConfiguration(conf =>
                {
                    conf.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
