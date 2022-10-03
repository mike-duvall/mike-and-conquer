using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mike_and_conquer_simulation.main;
using mike_and_conquer_simulation.rest.domain;

namespace mike_and_conquer_monogame.rest.controller
{
    [ApiController]
    //    [Route("[controller]")]

    [Route("simulation/query/options")]


    public class SimulationControllerQueryController : ControllerBase
    {

        private readonly ILogger<SimulationControllerQueryController> _logger;

        public SimulationControllerQueryController(ILogger<SimulationControllerQueryController> logger)
        {
            _logger = logger;
        }




        // [HttpGet]
        // public ActionResult Get([FromQuery] int unitId)
        // {
        //
        //     UnitView unitView = MikeAndConquerGame.instance.GetUnitViewByIdByEvent(unitId);
        //
        //
        //     RestUnit restUnit = new RestUnit();
        //
        //     restUnit.UnitId = unitView.UnitId;
        //     restUnit.Selected = unitView.Selected;
        //
        //     return new OkObjectResult(restUnit);
        // }


        // public static String convertToString(this Enum eff)
        // {
        //     return Enum.GetName(eff.GetType(), eff);
        // }

        [HttpGet]
        public ActionResult Get()
        {
            // GameOptions gameoptions = MikeAndConquerGame.instance.GetGameOptionsByEvent();
            //
            // RestUIOptions restUIOptions = new RestUIOptions(
            //     gameoptions.DrawShroud,
            //     gameoptions.MapZoomLevel
            //     );
            // TODO:  Is this thread safe and does it need to be?

            // String gameSpeedAsString = SimulationMain.instance.GetSimulationOptions().ToString();
            SimulationOptions options = SimulationMain.instance.GetSimulationOptions();
            SimulationOptions.GameSpeed gameSpeed = options.CurrentGameSpeed;

            String gameSpeedAsString = gameSpeed.ToString();


            RestSimulationOptions restSimulationOptions = new RestSimulationOptions(gameSpeedAsString);


            return new OkObjectResult(restSimulationOptions);
        }


    }
}
