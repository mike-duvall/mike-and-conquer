using System;
using Microsoft.AspNetCore.Mvc;
using mike_and_conquer_simulation.main;
using mike_and_conquer_simulation.rest.domain;

namespace mike_and_conquer_simulation.rest.controller
{
    [ApiController]
    [Route("simulation/query/options")]


    public class SimulationControllerQueryController : ControllerBase
    {



        [HttpGet]
        public ActionResult Get()
        {
            // TODO:  Is this thread safe and does it need to be?

            SimulationOptions options = SimulationMain.instance.GetSimulationOptions();
            SimulationOptions.GameSpeed gameSpeed = options.CurrentGameSpeed;

            String gameSpeedAsString = gameSpeed.ToString();


            RestSimulationOptions restSimulationOptions = new RestSimulationOptions(gameSpeedAsString);


            return new OkObjectResult(restSimulationOptions);
        }


    }
}
