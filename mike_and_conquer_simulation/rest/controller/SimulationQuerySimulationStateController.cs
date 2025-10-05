using Microsoft.AspNetCore.Mvc;
using mike_and_conquer_simulation.events;
using mike_and_conquer_simulation.main;
using mike_and_conquer_simulation.rest.domain;
using System;
using System.Collections.Generic;
using mike_and_conquer_simulation.simulationstate;

namespace mike_and_conquer_simulation.rest.controller
{
    [ApiController]
    [Route("simulation/query/simulationstate")]


    public class SimulationQuerySimulationStateController : ControllerBase
    {



        [HttpGet]
        public ActionResult Get()
        {

            SimulationState currentSimulationState = SimulationMain.instance.GetCurrentSimulationStateViaCommand();
            string nameOfClass = currentSimulationState.GetType().Name;
            RestSimulationState restSimulationState = new RestSimulationState(nameOfClass);
            return new OkObjectResult(restSimulationState);
        }


    }
}
