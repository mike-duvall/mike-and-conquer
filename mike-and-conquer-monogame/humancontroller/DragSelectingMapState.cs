
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using mike_and_conquer.gameview;
using mike_and_conquer_monogame.main;


namespace mike_and_conquer.gameworld.humancontroller
{
    public class DragSelectingMapState : HumanControllerState
    {
        public DragSelectingMapState(Point leftMouseDownStartPoint)
        {
            UnitSelectionBox unitSelectionBox = GameWorldView.instance.unitSelectionBox;
            unitSelectionBox.HandleStartDragSelect(leftMouseDownStartPoint);
        }

        public override HumanControllerState Update( MouseState newMouseState, MouseState oldMouseState)
        {

            if (MouseInputUtil.LeftMouseButtonIsBeingHeldDown(newMouseState, oldMouseState))
            {
                MikeAndConquerGame.instance.logger.LogWarning("LeftMouseButtonIsBeingHeldDown");
                Point mouseWorldLocationPoint = MouseInputUtil.GetWorldLocationPointFromMouseState(newMouseState);
                UnitSelectionBox unitSelectionBox = GameWorldView.instance.unitSelectionBox;
                unitSelectionBox.HandleMouseMoveDuringDragSelect(mouseWorldLocationPoint);
                return this;
            }
            else 
            {
                UnitSelectionBox unitSelectionBox = GameWorldView.instance.unitSelectionBox;
                MikeAndConquerGame.instance.logger.LogWarning("HandleEndDragSelect");
                unitSelectionBox.HandleEndDragSelect();
                // if (!GameWorldView.instance.IsAMinigunnerSelected())
                if (!GameWorldView.instance.IsAUnitViewSelected())
                {
                    Point mouseWorldLocationPoint = MouseInputUtil.GetWorldLocationPointFromMouseState(newMouseState);
                    HumanPlayerController.CheckForAndHandleLeftClickOnFriendlyUnit(mouseWorldLocationPoint);
                }

                return new PointerOverMapState();
            }
        }

    }


}
