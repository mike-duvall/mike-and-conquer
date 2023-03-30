using Microsoft.AspNetCore.Mvc;
using mike_and_conquer_monogame.main;
using mike_and_conquer_monogame.rest.domain;

namespace mike_and_conquer_monogame.rest.controller
{
    [ApiController]

    [Route("ui/query/uioptions")]


    public class UIOptionsQueryController : ControllerBase
    {


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
