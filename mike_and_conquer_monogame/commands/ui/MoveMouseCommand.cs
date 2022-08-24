using mike_and_conquer_monogame.main;

namespace mike_and_conquer_monogame.commands.ui
{
    public class MoveMouseCommand : AsyncViewCommand
    {


        private int xInWorldCoordinates;
        private int yInWorldCoordinates;


        public const string CommandName = "MoveMouse";

        public MoveMouseCommand(int xInWorldCoordinates, int yInWorldCoordinates)
        {
            this.xInWorldCoordinates = xInWorldCoordinates;
            this.yInWorldCoordinates = yInWorldCoordinates;
        }

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.MoveMouse(xInWorldCoordinates, yInWorldCoordinates);

        }
    }
}
