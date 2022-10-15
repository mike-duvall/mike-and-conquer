
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mike_and_conquer_monogame.main;

using MemoryStream = System.IO.MemoryStream;


namespace mike_and_conquer_monogame.rest.controller
{
    [ApiController]
    [Route("ui/screenshot")]


    public class ScreenshotController : ControllerBase
    {

        private readonly ILogger<ScreenshotController> _logger;

        public ScreenshotController(ILogger<ScreenshotController> logger)
        {
            _logger = logger;
        }



        [HttpGet]
        public IActionResult Get()
        {
            MemoryStream stream = MikeAndConquerGame.instance.GetScreenshotViaEvent();
            stream.Position = 0;
            return File(stream, "image/png");
        }



    }
}
