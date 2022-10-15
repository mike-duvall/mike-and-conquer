using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mike_and_conquer_monogame.main;
using mike_and_conquer_monogame.rest.domain;

namespace mike_and_conquer_monogame.rest.controller
{
    [ApiController]

    [Route("ui/query/uioptions")]


    public class UIOptionsQueryController : ControllerBase
    {

        private readonly ILogger<UIOptionsQueryController> _logger;

        public UIOptionsQueryController(ILogger<UIOptionsQueryController> logger)
        {
            _logger = logger;
        }



        [HttpGet]
        public ActionResult Get()
        {

            // TODO:  Is this thread safe and does it need to be?

            RestUIOptions restUIOptions = new RestUIOptions(
                GameOptions.instance.DrawShroud,
                GameOptions.instance.MapZoomLevel);


            return new OkObjectResult(restUIOptions);
        }


    }
}
