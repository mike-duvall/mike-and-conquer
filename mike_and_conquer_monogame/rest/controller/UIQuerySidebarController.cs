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

            // UnitView unitView = MikeAndConquerGame.instance.GetUnitViewByIdByEvent(unitId);
            //
            //
            // RestUnit restUnit = new RestUnit();
            //
            // restUnit.UnitId = unitView.UnitId;
            // restUnit.Selected = unitView.Selected;
            //
            // return new OkObjectResult(restUnit);


            RestSidebar sidebar = new RestSidebar();
            sidebar.buildBarracksEnabled = false;
            sidebar.buildMinigunnerEnabled = false;
            sidebar.barracksIsBuilding = false;


            GDIConstructionYardView gdiConstructionYardView =  GameWorldView.instance.GDIConstructionYardView;
            if (gdiConstructionYardView != null)
            {
                sidebar.barracksIsBuilding = gdiConstructionYardView.IsBuildingBarracks;
                sidebar.barracksReadyToPlace = gdiConstructionYardView.IsBarracksReadyToPlace;

            }


            // GDIConstructionYard constructionYard = GameWorld.instance.GDIConstructionYard;
            // if (constructionYard != null)
            // {
            //     sidebar.barracksIsBuilding = constructionYard.IsBuildingBarracks;
            //     sidebar.barracksReadyToPlace = constructionYard.IsBarracksReadyToPlace;
            // }
            //
            //
            if (GameWorldView.instance.BarracksSidebarIconView != null)
            {
                sidebar.buildBarracksEnabled = true;
            
            }
            //
            // if (GameWorldView.instance.MinigunnerSidebarIconView != null)
            // {
            //     sidebar.buildMinigunnerEnabled = true;
            // }
            //
            // return Ok(sidebar);

            return new OkObjectResult(sidebar);
        }



    }
}
