
using mike_and_conquer_monogame.main;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Boolean = System.Boolean;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Point = Microsoft.Xna.Framework.Point;



namespace mike_and_conquer.gameview
{

    public class UnitSelectionBox
    {

        public Vector2 Position
        {
            get { return new Vector2(selectionBoxRectangle.X, selectionBoxRectangle.Y);  }
        }

        public Boolean isDragSelectHappening;
        public Point selectionBoxDragStartPoint;

        public Rectangle selectionBoxRectangle = new Rectangle(0,0,10,10);



        internal void HandleMouseMoveDuringDragSelect(Point mouseWorldLocationPoint)
        {
            if (mouseWorldLocationPoint.X > selectionBoxDragStartPoint.X)
            {
                HandleDragFromLeftToRight(mouseWorldLocationPoint);
            }
            else
            {
                HandleDragFromRightToLeft(mouseWorldLocationPoint);
            }

        }

        public void HandleDragFromLeftToRight(Point mouseWorldLocationPoint)
        {
            if (mouseWorldLocationPoint.Y > selectionBoxDragStartPoint.Y)
            {
                HandleDragFromTopLeftToBottomRight(mouseWorldLocationPoint);
            }
            else
            {
                HandleDrapFromBottomLeftToTopRight(mouseWorldLocationPoint);
            }
        }

        public void HandleDragFromRightToLeft(Point mouseWorldLocationPoint)
        {
            if (mouseWorldLocationPoint.Y > selectionBoxDragStartPoint.Y)
            {
                HandleDragFromTopRightToBottomLeft(mouseWorldLocationPoint);
            }
            else
            {
                HandleDragFromBottomRightToTopLeft(mouseWorldLocationPoint);
            }
        }


        private void HandleDragFromBottomRightToTopLeft(Point mouseWorldLocationPoint)
        {
            selectionBoxRectangle = new Rectangle(
                mouseWorldLocationPoint.X,
                mouseWorldLocationPoint.Y,
                selectionBoxDragStartPoint.X - mouseWorldLocationPoint.X,
                selectionBoxDragStartPoint.Y - mouseWorldLocationPoint.Y);
        }

        private void HandleDragFromTopRightToBottomLeft(Point mouseWorldLocationPoint)
        {
            selectionBoxRectangle = new Rectangle(
                mouseWorldLocationPoint.X,
                selectionBoxDragStartPoint.Y,
                selectionBoxDragStartPoint.X - mouseWorldLocationPoint.X,
                mouseWorldLocationPoint.Y - selectionBoxDragStartPoint.Y);
        }

        private void HandleDrapFromBottomLeftToTopRight(Point mouseWorldLocationPoint)
        {
            selectionBoxRectangle = new Rectangle(
                selectionBoxDragStartPoint.X,
                mouseWorldLocationPoint.Y,
                mouseWorldLocationPoint.X - selectionBoxDragStartPoint.X,
                selectionBoxDragStartPoint.Y - mouseWorldLocationPoint.Y);
        }

        private void HandleDragFromTopLeftToBottomRight(Point mouseWorldLocationPoint)
        {
            selectionBoxRectangle = new Rectangle(
                selectionBoxDragStartPoint.X,
                selectionBoxDragStartPoint.Y,
                mouseWorldLocationPoint.X - selectionBoxDragStartPoint.X,
                mouseWorldLocationPoint.Y - selectionBoxDragStartPoint.Y);
        }

        public int HandleEndDragSelect()
        {

            // TODO:  For now, limiting the number of allowed selected units to 5
            // Until can add ability to handle the case where more than 5 unites are directed
            // to move to the same square (i.e. add code to shunt some of the selected units off to nearby squares, since
            // a single square can only hold 5 minigunners
            int numMinigunnersSelected = 0;
            int maxAllowedSelected = 5;
            // foreach (Minigunner minigunner in MikeAndConquerGame.instance.gameWorld.GDIMinigunnerList)
            // {
            //     if (numMinigunnersSelected < maxAllowedSelected && selectionBoxRectangle.Contains(minigunner.GameWorldLocation.WorldCoordinatesAsVector2))
            //     {
            //         minigunner.selected = true;
            //         numMinigunnersSelected++;
            //     }
            //     else
            //     {
            //         minigunner.selected = false;
            //     }
            // }
            foreach (UnitView unitView in GameWorldView.instance.UnitViewList)
            {
                Vector2 unitWorldCoordinatesVector2 =
                    new Vector2(unitView.XInWorldCoordinates, unitView.YInWorldCoordinates);
                if (numMinigunnersSelected < maxAllowedSelected && selectionBoxRectangle.Contains(unitWorldCoordinatesVector2))
                {
                    unitView.Selected = true;
                    numMinigunnersSelected++;
                }
                else
                {
                    unitView.Selected = false;
                }
            }



            isDragSelectHappening = false;
            return numMinigunnersSelected;
        }



    }





}
