using System;
using Microsoft.Xna.Framework;

using mike_and_conquer.gameview;
using mike_and_conquer_monogame.main;
using mike_and_conquer_simulation.gameworld;
using MouseState = Microsoft.Xna.Framework.Input.MouseState;
using Mouse = Microsoft.Xna.Framework.Input.Mouse;



namespace mike_and_conquer.gameworld.humancontroller
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

        public static Boolean CheckForAndHandleLeftClickOnFriendlyUnit(Point mouseLocation)
        {
            int mouseX = mouseLocation.X;
            int mouseY = mouseLocation.Y;
            Boolean handled = false;
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
                    // GameWorld.instance.SelectSingleGDIUnit(nextMinigunner);
                    unitView.Selected = true;

                    // MikeAndConquerGame.instance.SoundManager.PlayUnitAwaitingOrders();
                }
            }




            // if (!handled)
            // {
            //     handled = CheckForAndHandleLeftClickOnMCV(mouseX, mouseY);
            // }

            return handled;
        }


        // private static bool CheckForAndHandleLeftClickOnMCV(int mouseX, int mouseY)
        // {
        //     Boolean handled = false;
        //     MCV mcv = GameWorld.instance.MCV;
        //     if (mcv != null)
        //     {
        //         if (mcv.ContainsPoint(mouseX, mouseY))
        //         {
        //             handled = true;
        //             if (mcv.selected == false)
        //             {
        //                 GameWorld.instance.SelectMCV(GameWorld.instance.MCV);
        //             }
        //             else
        //             {
        //                 MapTileLocation mapTileLocation =
        //                     MapTileLocation.CreateFromWorldCoordinatesInVector2(mcv.GameWorldLocation.WorldCoordinatesAsVector2);
        //                 MikeAndConquerGame.instance.RemoveMCV();
        //                 MikeAndConquerGame.instance.AddGDIConstructionYard(mapTileLocation);
        //             }
        //         }
        //     }
        //
        //     return handled;
        // }




    }
}

