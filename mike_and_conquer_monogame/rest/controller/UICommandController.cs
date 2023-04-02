using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


using mike_and_conquer_monogame.commands.ui;
using mike_and_conquer_monogame.main;
using mike_and_conquer_monogame.rest.domain;
using Serilog;


namespace mike_and_conquer_monogame.rest.controller
{
    [ApiController]
    [Route("ui/command")]


    public class UICommandController : ControllerBase
    {

        private static readonly ILogger Logger = Log.ForContext<UICommandController>();


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult PostAdminCommand([FromBody] RestRawCommandUI incomingRawCommand)
        {
            Logger.Information("PostAdminCommand called with incomingRawCommand = {incomingRawCommand}", incomingRawCommand);
            try
            {
                RawCommandUI rawCommand = new RawCommandUI();
                rawCommand.CommandType = incomingRawCommand.CommandType;
                rawCommand.CommandData = incomingRawCommand.CommandData;
                
                MikeAndConquerGame.instance.ProcessUiCommandSynchronously(rawCommand);

                return new OkObjectResult(new { Message = "Command Accepted" });

            }
            catch (Exception e)
            {
                Logger.Warning(e, "Error processing Command");
                

                return ValidationProblem(e.Message);
            }
        }

    }
}
