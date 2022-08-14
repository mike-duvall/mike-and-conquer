
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

        private bool isDragSelectHappening;
        private Point selectionBoxDragStartPoint;

        private Rectangle selectionBoxRectangle = new Rectangle(0,0,10,10);

        public Rectangle SelectionBoxRectangle
        {
            get { return selectionBoxRectangle; }
        }

        public bool IsDragSelectHappening
        {
            get { return isDragSelectHappening; }
        }



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
            isDragSelectHappening = true;
        }

        public void HandleDragFromLeftToRight(Point mouseWorldLocationPoint)
        {
            if (mouseWorldLocationPoint.Y > selectionBoxDragStartPoint.Y)
            {
                HandleDragFromTopLeftToBottomRight(mouseWorldLocationPoint);
            }
            else
            {
                HandleDragFromBottomLeftToTopRight(mouseWorldLocationPoint);
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

        private void HandleDragFromBottomLeftToTopRight(Point mouseWorldLocationPoint)
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


        public void HandleStartDragSelect(Point aPoint)
        {
            this.selectionBoxDragStartPoint = aPoint;
        }
    }





}
