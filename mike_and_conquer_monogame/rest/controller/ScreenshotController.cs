
using Microsoft.AspNetCore.Mvc;
using mike_and_conquer_monogame.main;
using MemoryStream = System.IO.MemoryStream;


namespace mike_and_conquer_monogame.rest.controller
{
    [ApiController]
    [Route("ui/screenshot")]


    public class ScreenshotController : ControllerBase
    {


        [HttpGet]
        public IActionResult Get()
        {
            MemoryStream stream = MikeAndConquerGame.instance.GetScreenshotViaEvent();
            stream.Position = 0;
            return File(stream, "image/png");
        }



    }
}
