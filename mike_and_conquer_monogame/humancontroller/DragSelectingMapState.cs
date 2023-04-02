
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using mike_and_conquer_monogame.gameview;
using Serilog;


namespace mike_and_conquer_monogame.humancontroller
{


    class DragSelectingMapState : HumanControllerState
    {


        private static readonly ILogger Logger = Log.ForContext<DragSelectingMapState>();

        public DragSelectingMapState(Point leftMouseDownStartPoint)
        {
            UnitSelectionBox unitSelectionBox = GameWorldView.instance.unitSelectionBox;
            unitSelectionBox.HandleStartDragSelect(leftMouseDownStartPoint);
        }

        public override HumanControllerState Update( MouseState newMouseState, MouseState oldMouseState)
        {

            if (MouseInputUtil.LeftMouseButtonIsBeingHeldDown(newMouseState, oldMouseState))
            {
                Logger.Information("LeftMouseButtonIsBeingHeldDown");
                Point mouseWorldLocationPoint = MouseInputUtil.GetWorldLocationPointFromMouseState(newMouseState);
                UnitSelectionBox unitSelectionBox = GameWorldView.instance.unitSelectionBox;
                unitSelectionBox.HandleMouseMoveDuringDragSelect(mouseWorldLocationPoint);
                return this;
            }
            else 
            {
                UnitSelectionBox unitSelectionBox = GameWorldView.instance.unitSelectionBox;
                Logger.Information("HandleEndDragSelect");
                unitSelectionBox.HandleEndDragSelect();
                if (!GameWorldView.instance.IsAGDIUnitViewSelected())
                {
                    Point mouseWorldLocationPoint = MouseInputUtil.GetWorldLocationPointFromMouseState(newMouseState);
                    HumanPlayerController.CheckForAndHandleLeftClickOnFriendlyUnit(mouseWorldLocationPoint);
                }

                return new PointerOverMapState();
            }
        }

    }


}
