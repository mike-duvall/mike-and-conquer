using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mike_and_conquer_simulation.events;
using mike_and_conquer_simulation.main;
using mike_and_conquer_simulation.rest.domain;


namespace mike_and_conquer_simulation.rest.controller
{
    [ApiController]
    // [Route("[controller]")]
    [Route("simulation/query/eventscount")]

    public class SimulationStateUpdateEventsCountController : ControllerBase
    {


        private readonly ILogger<SimulationStateUpdateEventsController> _logger;

        public SimulationStateUpdateEventsCountController(ILogger<SimulationStateUpdateEventsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public int Get()
        {
            List<RestSimulationStateUpdateEvent> restReturnList = new List<RestSimulationStateUpdateEvent>();

            List<SimulationStateUpdateEvent> simulationStateUpdateList = 
                SimulationMain.instance.GetCopyOfEventHistoryViaEvent();

            return simulationStateUpdateList.Count;
        }


    }
}
