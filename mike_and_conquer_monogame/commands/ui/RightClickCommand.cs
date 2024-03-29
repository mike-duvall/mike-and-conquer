﻿using mike_and_conquer_monogame.main;

namespace mike_and_conquer_monogame.commands.ui
{
    public class RightClickCommand : AsyncViewCommand
    {


        private int xInWorldCoordinates;
        private int yInWorldCoordinates;


        public const string CommandName = "RightClick";

        public RightClickCommand(int xInWorldCoordinates, int yInWorldCoordinates)
        {
            this.xInWorldCoordinates = xInWorldCoordinates;
            this.yInWorldCoordinates = yInWorldCoordinates;
        }

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.RightClick(xInWorldCoordinates, yInWorldCoordinates);

        }
    }
}
