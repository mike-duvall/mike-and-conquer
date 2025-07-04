using Microsoft.AspNetCore.Mvc;

using mike_and_conquer_monogame.gameview;
using mike_and_conquer_monogame.main;
using Serilog;

namespace mike_and_conquer_monogame.rest.controller
{
    [ApiController]
    [Route("ui/query/mouseCursor")]


    public class UIMouseCursorStateController : ControllerBase
    {


        [HttpGet]
        public ActionResult Get()
        {

            // TODO Should this be retrieved via event on game loop?
            string cursorState = "\"" + GameWorldView.instance.gameCursor.StateAsString + "\"";
            return new OkObjectResult(cursorState);

        }





    }
}
