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

    [Route("ui/query/sidebar")]


    public class SidebarController : ControllerBase
    {

        private readonly ILogger<SidebarController> _logger;

        public SidebarController(ILogger<SidebarController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public ActionResult Get()
        {

            RestSidebar sidebar = new RestSidebar();
            sidebar.buildBarracksEnabled = false;
            sidebar.buildMinigunnerEnabled = false;
            sidebar.barracksIsBuilding = false;
            sidebar.minigunnerIsBuilding = false;

            GDIConstructionYardView gdiConstructionYardView =  GameWorldView.instance.GDIConstructionYardView;
            if (gdiConstructionYardView != null)
            {
                sidebar.barracksIsBuilding = gdiConstructionYardView.IsBuildingBarracks;
                sidebar.barracksReadyToPlace = gdiConstructionYardView.IsBarracksReadyToPlace;

            }

            if (GameWorldView.instance.BarracksSidebarIconView != null)
            {
                sidebar.buildBarracksEnabled = true;
            
            }

            if (GameWorldView.instance.MinigunnerSidebarIconView != null)
            {
                sidebar.buildMinigunnerEnabled = true;
            }
            
            return new OkObjectResult(sidebar);
        }



    }
}
