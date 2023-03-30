using Microsoft.AspNetCore.Mvc;

using mike_and_conquer_monogame.gameview;

using mike_and_conquer_monogame.rest.domain;


namespace mike_and_conquer_monogame.rest.controller
{
    [ApiController]
    //    [Route("[controller]")]

    [Route("ui/query/sidebar")]


    public class SidebarController : ControllerBase
    {


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

            GDIBarracksView gdiBarracksView = GameWorldView.instance.GDIBarracksView;

            if (gdiBarracksView != null)
            {
                sidebar.minigunnerIsBuilding = gdiBarracksView.IsBuildingMinigunner;
            }
            
            return new OkObjectResult(sidebar);
        }



    }
}
