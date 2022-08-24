using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mike_and_conquer_monogame.commands;
using mike_and_conquer_monogame.commands.ui;
using mike_and_conquer_monogame.main;
using mike_and_conquer_monogame.rest.domain;
using mike_and_conquer_simulation.rest.controller;

// using WeatherForecast = mike_and_conquer_simulation.rest.domain.WeatherForecast;

namespace mike_and_conquer_monogame.rest.controller
{
    [ApiController]
    //    [Route("[controller]")]
    [Route("ui/command")]


    public class UICommandController : ControllerBase
    {

        private readonly ILogger<AdminCommandController> _logger;

        public UICommandController(ILogger<AdminCommandController> logger)
        {
            _logger = logger;
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult PostAdminCommand([FromBody] RestRawCommandUI incomingRawCommand)
        {
            try
            {
                RawCommandUI rawCommand = new RawCommandUI();
                rawCommand.CommandType = incomingRawCommand.CommandType;
                rawCommand.CommandData = incomingRawCommand.CommandData;
                
                MikeAndConquerGame.instance.PostCommand(rawCommand);

                return new OkObjectResult(new { Message = "Command Accepted" });

            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Error processing Command");

                return ValidationProblem(e.Message);
            }
        }

    }
}
