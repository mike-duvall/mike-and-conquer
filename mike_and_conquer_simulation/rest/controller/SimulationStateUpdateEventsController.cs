
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mike_and_conquer_simulation.events;
using mike_and_conquer_simulation.main;
using mike_and_conquer_simulation.rest.domain;


namespace mike_and_conquer_simulation.rest.controller
{
    [ApiController]
    [Route("simulation/query/events")]

    public class SimulationStateUpdateEventsController : ControllerBase
    {


        private readonly ILogger<SimulationStateUpdateEventsController> _logger;

        public SimulationStateUpdateEventsController(ILogger<SimulationStateUpdateEventsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<RestSimulationStateUpdateEvent> Get([FromQuery] int startIndex)
        {


            List<RestSimulationStateUpdateEvent> restReturnList = new List<RestSimulationStateUpdateEvent>();

            List<SimulationStateUpdateEvent> simulationStateUpdateList = 
                SimulationMain.instance.GetCopyOfEventHistoryViaEvent();


            int currentIndex = 0;
            foreach (SimulationStateUpdateEvent simulationStateUpdateEvent in simulationStateUpdateList)
            {
                if (currentIndex >= startIndex)
                {
                    RestSimulationStateUpdateEvent anEvent = new RestSimulationStateUpdateEvent();
                    anEvent.EventType = simulationStateUpdateEvent.EventType;
                    anEvent.EventData = simulationStateUpdateEvent.EventData;
                    restReturnList.Add(anEvent);
                }

                currentIndex++;
            }


            return restReturnList;
        }


    }
}
