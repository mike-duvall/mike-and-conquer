using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


using mike_and_conquer_monogame.commands.ui;
using mike_and_conquer_monogame.main;
using mike_and_conquer_monogame.rest.domain;




namespace mike_and_conquer_monogame.rest.controller
{
    [ApiController]
    [Route("ui/command")]


    public class UICommandController : ControllerBase
    {

        private readonly ILogger<UICommandController> _logger;

        public UICommandController(ILogger<UICommandController> logger)
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
