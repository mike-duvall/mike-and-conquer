

using Point = Microsoft.Xna.Framework.Point;
using MouseState = Microsoft.Xna.Framework.Input.MouseState;

using GameWorldView = mike_and_conquer_monogame.gameview.GameWorldView;

using GDIConstructionYardView = mike_and_conquer_monogame.gameview.GDIConstructionYardView;
using BeginBuildingBarracksCommand = mike_and_conquer_simulation.commands.BeginBuildingBarracksCommand;
using BeginBuildingMinigunnerCommand = mike_and_conquer_simulation.commands.BeginBuildingMinigunnerCommand;



using SimulationMain = mike_and_conquer_simulation.main.SimulationMain;


namespace mike_and_conquer_monogame.humancontroller
{
    class PointerOverSidebarState : HumanControllerState
    {
        public override HumanControllerState Update(MouseState newMouseState, MouseState oldMouseState)
        {

            if (MouseInputUtil.IsOverSidebar(newMouseState))
            {

                if (MouseInputUtil.LeftMouseButtonClicked(newMouseState, oldMouseState))
                {
                    Point sidebarWorldLocation = MouseInputUtil.GetSidebarWorldLocationPointFromMouseState(newMouseState);

                    // TODO:  Add Sidebar class, have build buttons sit inside of it, iterate through
                    // them and ask if they contain point where sidebar was clicked
                    if (sidebarWorldLocation.X > 0 && sidebarWorldLocation.X < 64 && sidebarWorldLocation.Y > 0 && sidebarWorldLocation.Y < 48)
                    {
                        return HandleClickOnBuildBarracks();
                    }
                    else if (sidebarWorldLocation.X > 80 && sidebarWorldLocation.X < 144 && sidebarWorldLocation.Y > 0 && sidebarWorldLocation.Y < 48)
                    {
                        HandleClickOnBuildMinigunner();
                    }

                }

                return this;
            }
            else
            {
                // TODO:  Check if units are selected or not first
                // to find correct state
                return new PointerOverMapState();
            }
        }


        private HumanControllerState HandleClickOnBuildBarracks()
        {
            // GDIConstructionYard gdiConstructionYard = GameWorld.instance.GDIConstructionYard;
            //
            // if (gdiConstructionYard.IsBarracksReadyToPlace)
            // {
            //     return new PlacingBuildingState();
            // }
            // else if (!gdiConstructionYard.IsBuildingBarracks)
            // {
            //     GameWorld.instance.GDIConstructionYard.StartBuildingBarracks();
            //     MikeAndConquerGame.instance.SoundManager.PlayEVABuilding();
            // }
            //
            // return this;


            GDIConstructionYardView view = GameWorldView.instance.GDIConstructionYardView;
            if (view != null)
            {

                if (view.IsBarracksReadyToPlace)
                {
                    return new PlacingBuildingState();
                }

                if (!view.IsBuildingBarracks)
                {
                    OrderBeginBuldingBarracks();
                }
            }

            return this;
        }

        private void OrderBeginBuldingBarracks()
        {
            BeginBuildingBarracksCommand command = new BeginBuildingBarracksCommand();
            SimulationMain.instance.PostCommand(command);
        }


        private void HandleClickOnBuildMinigunner()
        {
            // GameWorld.instance.GDIBarracks.StartBuildingMinigunner();
            BeginBuildingMinigunnerCommand command = new BeginBuildingMinigunnerCommand();
            SimulationMain.instance.PostCommand(command);
        }


    }
}
