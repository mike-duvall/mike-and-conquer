using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using mike_and_conquer_monogame.rest.init;

using mike_and_conquer_simulation.main;
using mike_and_conquer_simulation.rest.init;


using Serilog;


namespace mike_and_conquer_monogame.main
{
    public class MainProgram
    {


        private static Serilog.ILogger _logger;

        [STAThread]
        static void Main()
        {

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();


            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();


            _logger = Log.ForContext<MainProgram>();

            _logger.Information("MainProgram::Main:   Hello, Serilog!");

            MainProgram.RunRestServer(_logger);
            SimulationRestInitializer.RunRestServer(_logger);

            MikeAndConquerGame game = new MikeAndConquerGame();

            SimulationMain.StartSimulation(game.simulationStateListenerList);

            // using (var game = new MikeAndConquerGame())
            //     game.Run();
            using (game)
                game.Run();

        }


        public static void RunRestServer(Serilog.ILogger logger)
        {
            var task = CreateHostBuilder(logger,null).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(Serilog.ILogger logger, string[] args) =>
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
                    webBuilder.UseStartup<MonogameRestStartup>()
                        .UseUrls("http://*:5010");
                })
                .UseSerilog(logger);

    }
}
