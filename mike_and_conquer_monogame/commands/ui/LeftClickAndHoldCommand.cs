using mike_and_conquer_monogame.main;

namespace mike_and_conquer_monogame.commands.ui
{
    public class LeftClickAndHoldCommand : AsyncViewCommand
    {
        private int xInWorldCoordinates;
        private int yInWorldCoordinates;



        public const string CommandName = "LeftClickAndHold";

        public LeftClickAndHoldCommand(int xInWorldCoordinates, int yInWorldCoordinates)
        {
            this.xInWorldCoordinates = xInWorldCoordinates;
            this.yInWorldCoordinates = yInWorldCoordinates;
        }

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.LeftClickAndHold(xInWorldCoordinates, yInWorldCoordinates);

        }
    }
}
