using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace mike_and_conquer_simulation.rest.init
{
    public class SimulationRestInitializer
    {

        public static void RunRestServer()
        {
            // CreateHostBuilder(null).Build().Run();
            var task = CreateHostBuilder(null).Build().RunAsync();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false);
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<SimulationRestStartup>()
                        .UseUrls("http://*:5000");
                        //.UseUrls("http://*:80");
                });
    }
}
