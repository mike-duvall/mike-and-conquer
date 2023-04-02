using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mike_and_conquer_simulation.commands;
using mike_and_conquer_simulation.main;
using mike_and_conquer_simulation.rest.domain;

using Serilog;



namespace mike_and_conquer_simulation.rest.controller
{
    [ApiController]
    [Route("simulation/command")]


    public class AdminCommandController : ControllerBase
    {

        private static readonly ILogger Logger = Log.ForContext<AdminCommandController>();


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult PostAdminCommand([FromBody] RestRawCommand incomingRawCommand)
        {
            try
            {
                RawCommand rawCommand = new RawCommand();
                rawCommand.CommandType = incomingRawCommand.CommandType;
                rawCommand.CommandData = incomingRawCommand.CommandData;

                SimulationMain.instance.PostCommand(rawCommand);
                return new OkObjectResult(new {Message = "Command Accepted"});

            }
            catch (Exception e)
            {
                Logger.Error(e, "Error processing Command");

                return ValidationProblem(e.Message);
            }
        }
        
    }
}
