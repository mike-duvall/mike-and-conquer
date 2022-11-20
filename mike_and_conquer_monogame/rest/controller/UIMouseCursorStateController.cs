using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mike_and_conquer_monogame.gameview;

namespace mike_and_conquer_monogame.rest.controller
{
    [ApiController]
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

            // TODO Should this be retrieved via event on game loop?
            string cursorState = GameWorldView.instance.gameCursor.StateAsString;
            return new OkObjectResult(cursorState);

        }





    }
}
