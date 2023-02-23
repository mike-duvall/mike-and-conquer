
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

using Math = System.Math;

using Point = Microsoft.Xna.Framework.Point;
using MouseState = Microsoft.Xna.Framework.Input.MouseState;

using GameWorldView = mike_and_conquer_monogame.gameview.GameWorldView;
using MikeAndConquerGame = mike_and_conquer_monogame.main.MikeAndConquerGame;
using UnitView = mike_and_conquer_monogame.gameview.UnitView;
using MapTileInstanceView = mike_and_conquer_monogame.gameview.MapTileInstanceView;

using OrderUnitToMoveCommand = mike_and_conquer_simulation.commands.OrderUnitToMoveCommand;

using SimulationMain = mike_and_conquer_simulation.main.SimulationMain;

using mike_and_conquer_simulation.commands;


namespace mike_and_conquer_monogame.humancontroller
{
    public class PointerOverMapState : HumanControllerState
    {

        private Point leftMouseDownStartPoint = new Point(-1, -1);

        public override HumanControllerState Update(MouseState newMouseState, MouseState oldMouseState)
        {
            if (MouseInputUtil.IsOverSidebar(newMouseState))
            {
                return new PointerOverSidebarState();
            }
            
            GameWorldView.instance.gameCursor.SetToMainCursor();

            Point mouseWorldLocationPoint = MouseInputUtil.GetWorldLocationPointFromMouseState(newMouseState);

            if (GameWorldView.instance.IsAGDIMinigunnerSelected())
            {
                UpdateMousePointerWhenMinigunnerSelected(mouseWorldLocationPoint);
            }
            if (GameWorldView.instance.IsMCVSelected())
            {
                UpdateMousePointerWhenMCVSelected(mouseWorldLocationPoint);
            }


            if (MouseInputUtil.LeftMouseButtonClicked(newMouseState, oldMouseState))
            {
                MikeAndConquerGame.instance.logger.LogInformation("LeftMouseButtonClicked");
                leftMouseDownStartPoint = mouseWorldLocationPoint;

            }

            if (MouseInputUtil.LeftMouseButtonIsBeingHeldDown(newMouseState, oldMouseState))
            {
                MikeAndConquerGame.instance.logger.LogInformation("LeftMouseButtonIsBeingHeldDown");
                if (IsMouseDragHappening(mouseWorldLocationPoint))
                {
                    MikeAndConquerGame.instance.logger.LogInformation("IsMouseDragHappening");
                    return new DragSelectingMapState(leftMouseDownStartPoint);
                }
            }

            if (MouseInputUtil.LeftMouseButtonUnclicked(newMouseState, oldMouseState))
            {
                MikeAndConquerGame.instance.logger.LogInformation("LeftMouseButtonUnclicked");
                leftMouseDownStartPoint.X = -1;
                leftMouseDownStartPoint.Y = -1;
                bool handledEvent = HumanPlayerController.CheckForAndHandleLeftClickOnFriendlyUnit(mouseWorldLocationPoint);
                if (!handledEvent)
                {
                    handledEvent = CheckForAndHandleLeftClickOnEnemyUnit(mouseWorldLocationPoint);
                }
                
                if (!handledEvent)
                {
                    handledEvent = CheckForAndHandleLeftClickOnMap(mouseWorldLocationPoint);
                }
            }

            if (MouseInputUtil.RightMouseButtonClicked(newMouseState, oldMouseState))
            {
                HandleRightClick(mouseWorldLocationPoint);
            }

            return this;
        }

        private bool IsMouseDragHappening(Point mouseWorldLocationPoint)
        {
        
            if (leftMouseDownStartPoint.X != -1 && leftMouseDownStartPoint.Y != -1)
            {
                double distance = GetDistance(leftMouseDownStartPoint.X, leftMouseDownStartPoint.Y,
                    mouseWorldLocationPoint.X, mouseWorldLocationPoint.Y);
        
                if (distance > 2)
                {
                    return true;
                }
            }
        
            return false;
        
        }


        internal void HandleRightClick(Point mouseLocation)
        {

            int mouseX = mouseLocation.X;
            int mouseY = mouseLocation.Y;

            // foreach (Minigunner nextMinigunner in GameWorld.instance.GDIMinigunnerList)
            // {
            //     nextMinigunner.selected = false;
            // }
            //
            // if (GameWorld.instance.MCV != null)
            // {
            //     GameWorld.instance.MCV.selected = false;
            // }

            foreach (UnitView unitView in GameWorldView.instance.GDIUnitViewList)
            {
                unitView.Selected = false;
            }

        }

        private double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }




        // private bool CheckForAndHandleLeftClickOnMap(Point mouseLocation)
        // {
        //
        //     int mouseX = mouseLocation.X;
        //     int mouseY = mouseLocation.Y;
        //
        //     bool unitOrderedToMove = false;
        //
        //     foreach (Minigunner nextMinigunner in GameWorld.instance.GDIMinigunnerList)
        //     {
        //         if (nextMinigunner.selected == true)
        //         {
        //             if (GameWorld.instance.IsValidMoveDestination(new Point(mouseX, mouseY)))
        //             {
        //                 MapTileInstance clickedMapTileInstance =
        //                     GameWorld.instance.FindMapTileInstance(
        //                         MapTileLocation.CreateFromWorldCoordinates(mouseX, mouseY));
        //
        //
        //                 Point centerOfSquare = clickedMapTileInstance.GetCenter();
        //                 nextMinigunner.OrderToMoveToDestination(centerOfSquare);
        //                 unitOrderedToMove = true;
        //             }
        //         }
        //     }
        //
        //     MCV mcv = GameWorld.instance.MCV;
        //     if (mcv != null)
        //     {
        //         if (mcv.selected == true)
        //         {
        //             if (GameWorld.instance.IsValidMoveDestination(new Point(mouseX, mouseY)))
        //             {
        //                 MapTileInstance clickedMapTileInstance =
        //                     GameWorld.instance.FindMapTileInstance(
        //                         MapTileLocation.CreateFromWorldCoordinates(mouseX, mouseY));
        //
        //                 Point centerOfSquare = clickedMapTileInstance.GetCenter();
        //                 mcv.OrderToMoveToDestination(centerOfSquare);
        //                 unitOrderedToMove = true;
        //             }
        //
        //         }
        //     }
        //
        //     if (unitOrderedToMove)
        //     {
        //         MikeAndConquerGame.instance.SoundManager.PlayUnitAffirmative1();
        //     }
        //     return true;
        //
        // }
        //


        public void OrderToMoveToDestination(int unitId, Point centerOfSquare)
        {
            OrderUnitToMoveCommand command = new OrderUnitToMoveCommand();
            command.UnitId = unitId;
            command.DestinationXInWorldCoordinates = centerOfSquare.X;
            command.DestinationYInWorldCoordinates = centerOfSquare.Y;

            SimulationMain.instance.PostCommand(command);

        }


        public void OrderToAttackTarget(UnitView attackerUnitView, UnitView targetUnitView)
        {
            OrderUnitToAttackCommand command = new OrderUnitToAttackCommand();
            command.AttackerUnitId = attackerUnitView.UnitId;
            command.TargetUnitId = targetUnitView.UnitId;

            SimulationMain.instance.PostCommand(command);
        }



        private bool CheckForAndHandleLeftClickOnMap(Point mouseLocation)
        {

            int mouseX = mouseLocation.X;
            int mouseY = mouseLocation.Y;

            bool unitOrderedToMove = false;

            foreach (UnitView unitView in GameWorldView.instance.GDIUnitViewList)
            {
                if (unitView.Selected)
                {
                    // if (GameWorld.instance.IsValidMoveDestination(new Point(mouseX, mouseY)))
                    // {
                    MapTileInstanceView clickedMapTileInstance =
                        GameWorldView.instance.FindMapTileInstanceView(mouseX, mouseY);



                    Point centerOfSquare = clickedMapTileInstance.GetCenter();
                    OrderToMoveToDestination(unitView.UnitId, centerOfSquare);
                    // unitView.OrderToMoveToDestination(centerOfSquare);
                    unitOrderedToMove = true;
                    // }
                }
            }

            // MCV mcv = GameWorld.instance.MCV;
            // if (mcv != null)
            // {
            //     if (mcv.selected == true)
            //     {
            //         if (GameWorld.instance.IsValidMoveDestination(new Point(mouseX, mouseY)))
            //         {
            //             MapTileInstance clickedMapTileInstance =
            //                 GameWorld.instance.FindMapTileInstance(
            //                     MapTileLocation.CreateFromWorldCoordinates(mouseX, mouseY));
            //
            //             Point centerOfSquare = clickedMapTileInstance.GetCenter();
            //             mcv.OrderToMoveToDestination(centerOfSquare);
            //             unitOrderedToMove = true;
            //         }
            //
            //     }
            // }
            //
            // if (unitOrderedToMove)
            // {
            //     MikeAndConquerGame.instance.SoundManager.PlayUnitAffirmative1();
            // }
            return true;

        }


        internal bool CheckForAndHandleLeftClickOnEnemyUnit(Point mouseLocation)
        {
            int mouseX = mouseLocation.X;
            int mouseY = mouseLocation.Y;
        
            bool handled = false;
            foreach (UnitView unitView in GameWorldView.instance.NodUnitViewList)
            {
                if (unitView.ContainsPoint(mouseX, mouseY))
                {
                    handled = true;

                    List<UnitView> currentlySelectedUnitViews = GetCurrentlySelectedGDIUnitViews();
                    foreach (UnitView selectedUnitView in currentlySelectedUnitViews)
                    {
                        OrderToAttackTarget(selectedUnitView, unitView);
                    }
                    // MikeAndConquerGame.instance.SoundManager.PlayUnitAwaitingOrders();
                }
            }


            return handled;
        }


        List<UnitView> GetCurrentlySelectedGDIUnitViews()
        {
            List<UnitView> currentlySelectedUnitViews = new List<UnitView>();

            foreach (UnitView unitView in GameWorldView.instance.GDIUnitViewList)
            {
                if (unitView.Selected)
                {
                    currentlySelectedUnitViews.Add(unitView);
                }
            }

            return currentlySelectedUnitViews;
        }

        private static void UpdateMousePointerWhenMinigunnerSelected(Point mousePositionAsPointInWorldCoordinates)
        {

            if (GameWorldView.instance.IsPointOverEnemy(mousePositionAsPointInWorldCoordinates.X, mousePositionAsPointInWorldCoordinates.Y))
            {
                GameWorldView.instance.gameCursor.SetToAttackEnemyLocationCursor();
            }
            else
            if (GameWorldView.instance.IsValidMoveDestination(mousePositionAsPointInWorldCoordinates.X, mousePositionAsPointInWorldCoordinates.Y))
            {
                GameWorldView.instance.gameCursor.SetToMoveToLocationCursor();
            }
            else
            {
                GameWorldView.instance.gameCursor.SetToMovementNotAllowedCursor();
            }
        }

        private static void UpdateMousePointerWhenMCVSelected(Point mousePositionAsPointInWorldCoordinates)
        {
            if (GameWorldView.instance.IsPointOverMCV(mousePositionAsPointInWorldCoordinates.X, mousePositionAsPointInWorldCoordinates.Y))
            {
                GameWorldView.instance.gameCursor.SetToBuildConstructionYardCursor();
            }
            else if (GameWorldView.instance.IsValidMoveDestination(mousePositionAsPointInWorldCoordinates.X, mousePositionAsPointInWorldCoordinates.Y))
            {
                GameWorldView.instance.gameCursor.SetToMoveToLocationCursor();
            }
            else
            {
                GameWorldView.instance.gameCursor.SetToMovementNotAllowedCursor();
            }
        }

    }
}

