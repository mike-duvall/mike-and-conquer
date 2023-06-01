
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using mike_and_conquer_simulation.events;
using mike_and_conquer_simulation.main;
using mike_and_conquer_simulation.rest.domain;
using Serilog;



namespace mike_and_conquer_simulation.rest.controller
{
    [ApiController]
    [Route("simulation/query/events")]

    public class SimulationQueryEventsController : ControllerBase
    {


        private static readonly ILogger Logger = Log.ForContext<SimulationQueryEventsController>();


        [HttpGet]
        public IEnumerable<RestSimulationStateUpdateEvent> Get([FromQuery] int startIndex)
        {

            Logger.Information("Get called with startIndex = {startIndex}", startIndex);

            List<RestSimulationStateUpdateEvent> restReturnList = new List<RestSimulationStateUpdateEvent>();

            List<SimulationStateUpdateEvent> simulationStateUpdateList = 
                SimulationMain.instance.GetCopyOfEventHistoryViaCommand();


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
