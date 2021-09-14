using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace JacksonVeroneze.NET.Commons.AspNet
{
    public static class BaseProgram
    {
        public static IHostBuilder CreateHostBuilder<TStartup>(string[] args) where TStartup : class =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<TStartup>();
                    webBuilder.UseSerilog();
                });

        public static ILogger FactoryLogger(string environment, string applicationName)
        {
            return Logger.Logger.FactoryLogger(x =>
            {
                x.Environment = environment;
                x.ApplicationName = applicationName;
                x.CurrentDirectory = Directory.GetCurrentDirectory();
            });
        }
    }
}