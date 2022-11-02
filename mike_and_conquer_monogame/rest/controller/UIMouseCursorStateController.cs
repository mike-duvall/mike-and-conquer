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

    [Route("ui/query/mouseCursor")]


    public class UIMouseCursorStateController : ControllerBase
    {

        private readonly ILogger<UIMouseCursorStateController> _logger;

        public UIMouseCursorStateController(ILogger<UIMouseCursorStateController> logger)
        {
            _logger = logger;
        }



        [HttpGet]
        public ActionResult Get()
        {

            string cursorState = GameWorldView.instance.gameCursor.StateAsString;
            return new OkObjectResult(cursorState);

            // UnitView unitView = MikeAndConquerGame.instance.GetUnitViewByIdByEvent(unitId);
            //
            //
            // RestUnit restUnit = new RestUnit();
            //
            // restUnit.UnitId = unitView.UnitId;
            // restUnit.Selected = unitView.Selected;
            //
            // return new OkObjectResult(restUnit);
        }





    }
}
