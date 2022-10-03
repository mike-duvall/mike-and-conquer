using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mike_and_conquer_monogame.gameview;
using mike_and_conquer_monogame.main;
using mike_and_conquer_monogame.rest.domain;
using mike_and_conquer_simulation.rest.controller;

namespace mike_and_conquer_monogame.rest.controller
{
    [ApiController]
    //    [Route("[controller]")]

    [Route("ui/query/uioptions")]


    public class UIOptionsQueryController : ControllerBase
    {

        private readonly ILogger<UIOptionsQueryController> _logger;

        public UIOptionsQueryController(ILogger<UIOptionsQueryController> logger)
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

            RestUIOptions restUIOptions = new RestUIOptions(
                GameOptions.instance.DrawShroud,
                GameOptions.instance.MapZoomLevel);


            return new OkObjectResult(restUIOptions);
        }


    }
}
