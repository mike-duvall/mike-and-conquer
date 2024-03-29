using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace mike_and_conquer_simulation.rest.init
{
    public class SimulationRestInitializer
    {

        public static void RunRestServer(Serilog.ILogger logger)
        {
            var task = CreateHostBuilder(logger,null).Build().RunAsync();
        }


        public static IHostBuilder CreateHostBuilder(Serilog.ILogger logger,string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false);
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddSerilog();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<SimulationRestStartup>()
                        .UseUrls("http://*:5000");
                        //.UseUrls("http://*:80");
                })
                .UseSerilog(logger);
    }
}
