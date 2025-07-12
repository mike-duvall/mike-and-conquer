using Microsoft.AspNetCore.Mvc;
using mike_and_conquer_monogame.gameview;
using mike_and_conquer_monogame.rest.domain;

namespace mike_and_conquer_monogame.rest.controller
{
    [ApiController]
    [Route("ui/query/barracksplacement")]
    public class BarracksPlacementController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            bool isValidPlacement = false;

            // Check if BarracksPlacementIndicatorView exists
            BarracksPlacementIndicatorView barracksPlacementView = GameWorldView.instance?.BarracksPlacementIndicatorView;
            
            if (barracksPlacementView != null)
            {
                // Check if ValidBuildingLocation returns true
                isValidPlacement = barracksPlacementView.ValidBuildingLocation();
            }

            RestBarracksPlacementStatus status = new RestBarracksPlacementStatus(isValidPlacement);
            return new OkObjectResult(status);
        }
    }
}