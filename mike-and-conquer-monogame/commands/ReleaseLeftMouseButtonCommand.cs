using mike_and_conquer_monogame.main;


namespace mike_and_conquer_monogame.commands
{
    public class ReleaseLeftMouseButtonCommand : AsyncViewCommand
    {

        private int xInWorldCoordinates;
        private int yInWorldCoordinates;




        public const string CommandName = "ReleaseLeftMouseButton";

        public ReleaseLeftMouseButtonCommand(int xInWorldCoordinates, int yInWorldCoordinates)
        {
            this.xInWorldCoordinates = xInWorldCoordinates;
            this.yInWorldCoordinates = yInWorldCoordinates;

        }

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.ReleaseLeftMouseButton(xInWorldCoordinates, yInWorldCoordinates);

        }
    }
}
