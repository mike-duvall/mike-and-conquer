using System;
using commands;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using mike_and_conquer_monogame.rest.init;
using mike_and_conquer_simulation;
using mike_and_conquer_simulation.main;
using mike_and_conquer_simulation.rest.init;


using Serilog;
using Serilog.Context;

namespace mike_and_conquer_monogame.main
{
    public class MainProgram
    {


        // public static ILoggerFactory loggerFactory;

        public static Serilog.Core.Logger logger;

        [STAThread]
        static void Main()
        {

            // var configuration = new ConfigurationBuilder()
            //     .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: false)
            //     .Build();
            //
            // var loggingConfig = configuration.GetSection("Logging");
            //
            // loggerFactory = LoggerFactory.Create(builder =>
            // {
            //     builder
            //         .AddDebug()
            //         .AddConsole()
            //         .AddConfiguration(loggingConfig)
            //         ;
            // });
            //
            //
            // ILogger logger = loggerFactory.CreateLogger<MainProgram>();
            // logger.LogInformation("************************Mike is cool");
            // logger.LogWarning("************************Mike is cool");

            // using var log = new LoggerConfiguration()
            //     .WriteTo.Console()
            //     .WriteTo.Debug()
            //     .CreateLogger();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();


            logger
            // var logger 
                = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();



            // var contextLog = logger.ForContext("SourceContext", "LogTest");
            var contextLog = logger.ForContext<MainProgram>();
            contextLog.Information("This shows up because of the override!");
            contextLog.Information("... And this too!");


            logger.Information("MainProgram::Main:   Hello, Serilog!");
            DummyClass.DoSomeStuff();




            MainProgram.RunRestServer(logger);
            SimulationRestInitializer.RunRestServer();

            MikeAndConquerGame game = new MikeAndConquerGame();

            SimulationMain.StartSimulation(game.simulationStateListenerList);

            // using (var game = new MikeAndConquerGame())
            //     game.Run();
            using (game)
                game.Run();

        }


        public static void RunRestServer(Serilog.Core.Logger logger)
        {
            var task = CreateHostBuilder(logger,null).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(Serilog.Core.Logger logger, string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false);
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    // logging.AddConsole();
                    // logging.AddDebug();
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
