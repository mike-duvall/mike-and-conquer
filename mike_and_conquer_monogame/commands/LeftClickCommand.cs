using mike_and_conquer_monogame.main;


namespace mike_and_conquer_monogame.commands
{
    public class LeftClickCommand : AsyncViewCommand
    {


        private int xInWorldCoordinates;
        private int yInWorldCoordinates;


        public const string CommandName = "LeftClick";

        public LeftClickCommand(int xInWorldCoordinates, int yInWorldCoordinates)
        {
            this.xInWorldCoordinates = xInWorldCoordinates;
            this.yInWorldCoordinates = yInWorldCoordinates;
        }

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.LeftClick(xInWorldCoordinates, yInWorldCoordinates);

        }
    }
}
