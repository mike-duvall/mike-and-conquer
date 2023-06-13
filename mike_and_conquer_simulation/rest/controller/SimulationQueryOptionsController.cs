using System;
using Microsoft.AspNetCore.Mvc;
using mike_and_conquer_simulation.main;
using mike_and_conquer_simulation.rest.domain;

namespace mike_and_conquer_simulation.rest.controller
{
    [ApiController]
    [Route("simulation/query/options")]


    public class SimulationQueryOptionsController : ControllerBase
    {



        [HttpGet]
        public ActionResult Get()
        {
            // TODO:  Is this thread safe and does it need to be?
            // I think it technically might be since GameOptions is an enum, which should be atomic
            // and it's getting converted to a String, which should also be atomic
            // but probably still should make it follow the standard pattern of using a thread safe
            // Async command to retrieve the data

            SimulationOptions options = SimulationMain.instance.GetSimulationOptions();
            SimulationOptions.GameSpeed gameSpeed = options.CurrentGameSpeed;

            String gameSpeedAsString = gameSpeed.ToString();


            RestSimulationOptions restSimulationOptions = new RestSimulationOptions(gameSpeedAsString);


            return new OkObjectResult(restSimulationOptions);
        }


    }
}
