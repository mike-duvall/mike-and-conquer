using System;
using Microsoft.Xna.Framework;
using mike_and_conquer_monogame.gameview;
using mike_and_conquer_simulation.commands;
using mike_and_conquer_simulation.gameworld;
using MouseState = Microsoft.Xna.Framework.Input.MouseState;
using Mouse = Microsoft.Xna.Framework.Input.Mouse;

using SimulationMain = mike_and_conquer_simulation.main.SimulationMain;


namespace mike_and_conquer_monogame.humancontroller
{
    class HumanPlayerController : PlayerController
    {

        private HumanControllerState previousHumanControllerState;
        private HumanControllerState humanControllerState;

        public static HumanPlayerController instance;

        private MouseState oldMouseState;


        public MouseState MouseState
        {
            get { return oldMouseState; }
        }

        public HumanPlayerController()
        {
            instance = this;
            previousHumanControllerState = null;
            humanControllerState = new PointerOverMapState();
        }

        public override void Update()
        {
            MouseState newMouseState = Mouse.GetState();

            if (previousHumanControllerState != humanControllerState)
            {
                // TODO Fix logger here to use local logger
                // so that unit test can pass
                // MikeAndConquerGame.instance.log.Information("HumanControllerState instance type=" +
                //                                             humanControllerState.GetType().FullName);
            }
            previousHumanControllerState = humanControllerState;

            humanControllerState = humanControllerState.Update(newMouseState, oldMouseState);
            oldMouseState = newMouseState;
        }


        // public override void Add(Minigunner minigunner, bool aiIsOn)
        // {
        //     // Do nothing
        //     // TODO: This was added to AI controller could know about new minigunners
        //     // Reconsider how this is handled
        // }

        public static bool CheckForAndHandleLeftClickOnFriendlyUnit(Point mouseLocation)
        {
            int mouseX = mouseLocation.X;
            int mouseY = mouseLocation.Y;
            bool handled = false;
            // foreach (Minigunner nextMinigunner in GameWorld.instance.GDIMinigunnerList)
            // {
            //     if (nextMinigunner.ContainsPoint(mouseX, mouseY))
            //     {
            //         handled = true;
            //         GameWorld.instance.SelectSingleGDIUnit(nextMinigunner);
            //         MikeAndConquerGame.instance.SoundManager.PlayUnitAwaitingOrders();
            //     }
            // }


            foreach (UnitView unitView in GameWorldView.instance.UnitViewList)
            {
                if (unitView.ContainsPoint(mouseX, mouseY))
                {
                    handled = true;

                    if (unitView is MCVView)
                    {

                        if (unitView.Selected)
                        {
                            // If left clicking on MCV that is already selected, build construction yard
                            CreateGDIConstructionYardCommand command = new CreateGDIConstructionYardCommand();
                            SimulationMain.instance.PostCommand(command);
                        }

                    }

                    unitView.Selected = true;
                    // MikeAndConquerGame.instance.SoundManager.PlayUnitAwaitingOrders();
                }
            }


            return handled;
        }


    }
}

