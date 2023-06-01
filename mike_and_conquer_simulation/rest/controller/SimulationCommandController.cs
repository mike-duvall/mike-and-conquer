using System;
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


    public class SimulationCommandController : ControllerBase
    {

        private static readonly ILogger Logger = Log.ForContext<SimulationCommandController>();


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult PostAdminCommand([FromBody] RestJsonAsyncSimulationCommand incomingJsonAsyncSimulationCommand)
        {
            try
            {
                JsonAsyncSimulationCommand jsonAsyncSimulationCommand = new JsonAsyncSimulationCommand();
                jsonAsyncSimulationCommand.CommandType = incomingJsonAsyncSimulationCommand.CommandType;
                jsonAsyncSimulationCommand.JsonCommandData = incomingJsonAsyncSimulationCommand.JsonCommandData;

                SimulationMain.instance.PostCommand(jsonAsyncSimulationCommand);
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
