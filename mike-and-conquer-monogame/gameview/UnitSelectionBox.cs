
using mike_and_conquer_monogame.main;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Point = Microsoft.Xna.Framework.Point;
using Color = Microsoft.Xna.Framework.Color;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using SpriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects;

namespace mike_and_conquer.gameview
{

    internal class UnitSelectionBox
    {


        private bool isDragSelectHappening;
        private Point selectionBoxDragStartPoint;

        private Rectangle selectionBoxRectangle = new Rectangle(0,0,10,10);

        private Texture2D unitSelectionBoxTexture = null;
        private float defaultScale = 1;



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

        private void HandleDragFromLeftToRight(Point mouseWorldLocationPoint)
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

        private void HandleDragFromRightToLeft(Point mouseWorldLocationPoint)
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




        internal int HandleEndDragSelect()
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


        internal void HandleStartDragSelect(Point aPoint)
        {
            this.selectionBoxDragStartPoint = aPoint;
        }

        private void FillHorizontalLine(Color[] data, int width, int height, int lineIndex, Color color)
        {
            int beginIndex = width * lineIndex;
            for (int i = beginIndex; i < (beginIndex + width); ++i)
            {
                data[i] = color;
            }
        }

        private void FillVerticalLine(Color[] data, int width, int height, int lineIndex, Color color)
        {
            int beginIndex = lineIndex;
            for (int i = beginIndex; i < (width * height); i += width)
            {
                data[i] = color;
            }
        }


        private void UpdateBoundingRectangle()
        {

            int width = selectionBoxRectangle.Right - selectionBoxRectangle.Left;
            int height = selectionBoxRectangle.Bottom - selectionBoxRectangle.Top;

            if (width < 1) width = 1;
            if (height < 1) height = 1;
            if (unitSelectionBoxTexture != null)
            {
                unitSelectionBoxTexture.Dispose();
            }

            unitSelectionBoxTexture = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, width, height);
            Color[] data = new Color[width * height];
            FillHorizontalLine(data, width, height, 0, Color.White);
            FillHorizontalLine(data, width, height, height - 1, Color.White);
            FillVerticalLine(data, width, height, 0, Color.White);
            FillVerticalLine(data, width, height, width - 1, Color.White);
            //            DrawRedDotInCenter(width, height, data);
            unitSelectionBoxTexture.SetData(data);
        }

        private void DrawRedDotInCenter(int width, int height, Color[] data)
        {
            int centerX = width / 2;
            int centerY = height / 2;
            int centerOffset = (centerY * width) + centerX;
            data[centerOffset] = Color.Red;
        }


        internal void Draw(SpriteBatch spriteBatch)
        {
            if (isDragSelectHappening)
            {
                UpdateBoundingRectangle();
                Vector2 origin = new Vector2(0, 0);
                Vector2 position = new Vector2(selectionBoxRectangle.X, selectionBoxRectangle.Y);
                spriteBatch.Draw(unitSelectionBoxTexture, position, null, Color.White, 0f, origin, defaultScale,
                    SpriteEffects.None, SpriteSortLayers.UNIT_SELECTION_BOX_DEPTH);
            }

        }



    }





}
