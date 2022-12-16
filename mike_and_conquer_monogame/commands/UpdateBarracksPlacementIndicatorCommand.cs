

using BarracksPlacementIndicator = mike_and_conquer_monogame.gameview.BarracksPlacementIndicator;
using Point = Microsoft.Xna.Framework.Point;

namespace mike_and_conquer_monogame.commands
{
    public class UpdateBarracksPlacementIndicatorCommand : AsyncViewCommand
    {
        private BarracksPlacementIndicator barracksPlacementIndicator;
        private Point mouseLocation;


        public UpdateBarracksPlacementIndicatorCommand(BarracksPlacementIndicator barracksPlacementIndicator, Point mouseLocation)
        {
            this.barracksPlacementIndicator = barracksPlacementIndicator;
            this.mouseLocation = mouseLocation;
        }

        protected override void ProcessImpl()
        {
            // MikeAndConquerGame.instance.UpdateUnitViewPosition(unitId, xInWorldCoordinates, yInWorldCoordinates);
            barracksPlacementIndicator.UpdateLocationInWorldCoordinates(mouseLocation);
        }
    }
}
