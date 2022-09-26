using mike_and_conquer_monogame.main;

namespace mike_and_conquer_monogame.commands.ui
{
    public class SetUIOptionsCommand : AsyncViewCommand
    {

        // private int xInWorldCoordinates;
        // private int yInWorldCoordinates;
        private bool drawShroud;
        private float mapZoomLevel;



        public const string CommandName = "SetUIOptions";

        public SetUIOptionsCommand(bool drawShroud, float mapZoomLevel)
        {
            this.drawShroud = drawShroud;
            this.mapZoomLevel = mapZoomLevel;

        }

        protected override void ProcessImpl()
        {
            // MikeAndConquerGame.instance.ReleaseLeftMouseButton(xInWorldCoordinates, yInWorldCoordinates);
            MikeAndConquerGame.instance.SetUIOptions(drawShroud, mapZoomLevel);

        }
    }
}
