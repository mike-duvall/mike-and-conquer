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
        public ActionResult PostAdminCommand([FromBody] RestJsonUICommand incomingJsonUiCommand)
        {
            Logger.Information("PostAdminCommand called with incomingJsonUiCommand = {incomingJsonUiCommand}", incomingJsonUiCommand);
            try
            {
                JsonUICommand jsonUiCommand = new JsonUICommand();
                jsonUiCommand.CommandType = incomingJsonUiCommand.CommandType;
                jsonUiCommand.JsonCommandData = incomingJsonUiCommand.JsonCommandData;
                
                MikeAndConquerGame.instance.ProcessUiCommandSynchronously(jsonUiCommand);

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
