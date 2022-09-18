
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mike_and_conquer_monogame.main;


using HttpResponseMessage= System.Net.Http.HttpResponseMessage;
using MemoryStream = System.IO.MemoryStream;
using FileStream = System.IO.FileStream;
using HttpStatusCode = System.Net.HttpStatusCode;
using ByteArrayContent = System.Net.Http.ByteArrayContent;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;


namespace mike_and_conquer_monogame.rest.controller
{
    [ApiController]
    //    [Route("[controller]")]
    [Route("ui/screenshot")]


    public class ScreenshotController : ControllerBase
    {

        private readonly ILogger<ScreenshotController> _logger;

        public ScreenshotController(ILogger<ScreenshotController> logger)
        {
            _logger = logger;
        }

        // [HttpGet]
        // public HttpResponseMessage Generate()
        // {
        //     MemoryStream stream = GameWorld.instance.GetScreenshotViaEvent();
        //
        //     var result = new HttpResponseMessage(HttpStatusCode.OK)
        //     {
        //         Content = new ByteArrayContent(stream.ToArray())
        //     };
        //     result.Content.Headers.ContentDisposition =
        //         new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
        //         {
        //             FileName = "screenshot.png"
        //         };
        //     result.Content.Headers.ContentType =
        //         new MediaTypeHeaderValue("application/octet-stream");
        //
        //     return result;
        // }

        // [HttpGet]
        // public HttpResponseMessage Generate()
        // {
        //     MemoryStream stream = MikeAndConquerGame.instance.GetScreenshotViaEvent();
        //
        //     var result = new HttpResponseMessage(HttpStatusCode.OK)
        //     {
        //         Content = new ByteArrayContent(stream.ToArray())
        //     };
        //     result.Content.Headers.ContentDisposition =
        //         new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
        //         {
        //             FileName = "screenshot.png"
        //         };
        //     result.Content.Headers.ContentType =
        //         new MediaTypeHeaderValue("application/octet-stream");
        //
        //     return result;
        // }


        [HttpGet]
        public IActionResult Get()
        {
            // var image = System.IO.File.OpenRead("C:\\buildoutput\\real-game-shroud-1-start-x408-y129-232x159.png");
            MemoryStream stream = MikeAndConquerGame.instance.GetScreenshotViaEvent();
            stream.Flush();

            // FileStream fileStream = new FileStream("C:\\buildoutput\\temp3.png", FileMode.Create,FileAccess.ReadWrite);
            FileStream fileStream = new FileStream("C:\\buildoutput\\temp3.png", FileMode.Open, FileAccess.Read);
            // stream.CopyTo(fileStream);


            
            return File(fileStream, "image/png");


        }


        // [HttpGet]
        // public IActionResult Get()
        // {
        //     var image = System.IO.File.OpenRead("C:\\buildoutput\\real-game-shroud-1-start-x408-y129-232x159.png");
        //     return File(image, "image/jpeg");
        // }


        // [HttpGet]
        // public ActionResult Get([FromQuery] int unitId)
        // {
        //
        //     // UnitView unitView = MikeAndConquerGame.instance.GetUnitViewByIdByEvent(unitId);
        //
        //
        //     RestUnit restUnit = new RestUnit();
        //
        //     // restUnit.UnitId = unitView.UnitId;
        //     // restUnit.Selected = unitView.Selected;
        //
        //     return new OkObjectResult(restUnit);
        // }



    }
}
