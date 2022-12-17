

using BarracksPlacementIndicatorView = mike_and_conquer_monogame.gameview.BarracksPlacementIndicatorView;
using Point = Microsoft.Xna.Framework.Point;

namespace mike_and_conquer_monogame.commands
{
    public class UpdateBarracksPlacementIndicatorCommand : AsyncViewCommand
    {
        private BarracksPlacementIndicatorView barracksPlacementIndicatorView;
        private Point mouseLocation;


        public UpdateBarracksPlacementIndicatorCommand(BarracksPlacementIndicatorView barracksPlacementIndicatorView, Point mouseLocation)
        {
            this.barracksPlacementIndicatorView = barracksPlacementIndicatorView;
            this.mouseLocation = mouseLocation;
        }

        protected override void ProcessImpl()
        {
            // MikeAndConquerGame.instance.UpdateUnitViewPosition(unitId, xInWorldCoordinates, yInWorldCoordinates);
            barracksPlacementIndicatorView.UpdateLocationInWorldCoordinates(mouseLocation);
        }
    }
}
