
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using mike_and_conquer_simulation.events;
using mike_and_conquer_simulation.main;



namespace mike_and_conquer_simulation.rest.controller
{
    [ApiController]
    [Route("simulation/query/eventscount")]

    public class SimulationQueryEventsCountController : ControllerBase
    {



        [HttpGet]
        public int Get()
        {

            List<SimulationStateUpdateEvent> simulationStateUpdateList = 
                SimulationMain.instance.GetCopyOfEventHistoryViaCommand();

            return simulationStateUpdateList.Count;
        }


    }
}
