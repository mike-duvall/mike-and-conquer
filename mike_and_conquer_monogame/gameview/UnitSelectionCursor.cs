using System;
using Microsoft.Xna.Framework;
using mike_and_conquer_monogame.main;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Color = Microsoft.Xna.Framework.Color;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using SpriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects;
using Boolean = System.Boolean;


namespace mike_and_conquer_monogame.gameview
{

    public class UnitSelectionCursor
    {

        Texture2D boundingRectangle;

        // private GameObject myGameObject;
        private readonly UnitView myUnitView;

        private readonly Texture2D selectionCursorTexture;
        private Vector2 selectionCursorPosition;


        // private Texture2D healthBarTexture;
        // private Texture2D healthBarShadowTexture;
        // Vector2 healthBarPosition;

        Boolean drawBoundingRectangle;

        private Vector2 origin;

        float defaultScale = 1;


        private UnitSelectionCursor()
        {

        }


        public UnitSelectionCursor(UnitView unitView, int x, int y)
        {
            this.myUnitView = unitView;
            origin = new Vector2();
            origin.X = 0;
            origin.Y = 0;

            this.selectionCursorTexture = InitializeSelectionCursor();

            boundingRectangle = InitializeBoundingRectangle();
            // healthBarTexture = null;

            drawBoundingRectangle = false;

            // healthBarShadowTexture = InitializeHealthBarShadow();
        }




        // TODO Delete these Fillxxxx methods
        // and replace with the Drawxxxx ones
        internal void FillHorizontalLine(Color[] data, int width, int height, int lineIndex, Color color)
        {
            int beginIndex = width * lineIndex;
            for (int i = beginIndex; i < (beginIndex + width); ++i)
            {
                data[i] = color;
            }
        }

        internal void FillHorizontalLine(Color[] data, int width, int height, int lineIndex, Color color, int start, int end)
        {
            int beginIndex = width * lineIndex;
            int relativeStart = beginIndex + start;
            int relativeEnd = beginIndex + end;
            for (int i = beginIndex; i < (beginIndex + width); ++i)
            {
                if (i >= relativeStart && i <= relativeEnd)
                {
                    data[i] = color;
                }

            }
        }



        internal void FillVerticalLine(Color[] data, int width, int height, int lineIndex, Color color)
        {
            int beginIndex = lineIndex;
            for (int i = beginIndex; i < (width * height); i += width)
            {
                data[i] = color;
            }
        }


        void DrawHorizontalLine(Color[] data, Color color, int width, int height, int startX, int startY, int length)
        {
            if (startX + length > width)
            {
                throw new Exception("Attempt to create line outside bounds of texture width");
            }
            int beginIndex = startX + (width * startY);

            for (int i = beginIndex; i < beginIndex + length; i++)
            {
                data[i] = color;
            }

        }

        void DrawVerticalLine(Color[] data, Color color, int width, int height, int startX, int startY, int length)
        {

            int dataIndex = startX + (width * startY);

            for (int i = 0; i < length; i++)
            {
                data[dataIndex] = color;
                dataIndex += width;
            }

        }


        internal Texture2D InitializeBoundingRectangle()
        {
            Texture2D rectangle = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, selectionCursorTexture.Width, selectionCursorTexture.Height);
            Color[] data = new Color[rectangle.Width * rectangle.Height];
            FillHorizontalLine(data, rectangle.Width, rectangle.Height, 0, Color.White);
            FillHorizontalLine(data, rectangle.Width, rectangle.Height, rectangle.Height - 1, Color.White);
            FillVerticalLine(data, rectangle.Width, rectangle.Height, 0, Color.White);
            FillVerticalLine(data, rectangle.Width, rectangle.Height, rectangle.Width - 1, Color.White);
            int centerX = rectangle.Width / 2;
            int centerY = rectangle.Height / 2;
            int centerOffset = (centerY * rectangle.Width) + centerX;

            data[centerOffset] = Color.Red;

            rectangle.SetData(data);
            return rectangle;
        }


        // private Texture2D InitializeHealthBar()
        // {
        //     
        //     int healthBarHeight = 4;
        //     int healthBarWidth = myGameObject.UnitSize.Width;
        //
        //     Texture2D rectangle =
        //         new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, healthBarWidth, healthBarHeight);
        //
        //     Color[] data = new Color[rectangle.Width * rectangle.Height];
        //
        //     Color cncPalleteColorBlack = new Color(0, 255, 255, 255);
        //     Color cncPalleteColorGreen = new Color(4, 255, 255, 255);
        //
        //     FillHorizontalLine(data, rectangle.Width, rectangle.Height, 0, cncPalleteColorBlack);
        //
        //     FillHorizontalLine(data, rectangle.Width, rectangle.Height, 3, cncPalleteColorBlack);
        //
        //     FillVerticalLine(data, rectangle.Width, rectangle.Height, 0, cncPalleteColorBlack);
        //     FillVerticalLine(data, rectangle.Width, rectangle.Height, healthBarWidth - 1, cncPalleteColorBlack);
        //
        //     int nonBorderHealthBarWidth = healthBarWidth - 2;
        //     int maxHealth = myGameObject.MaxHealth;
        //     float ratio = (float)nonBorderHealthBarWidth / (float)maxHealth;
        //     int unitHealth = myGameObject.Health;
        //     int healthBarLength = (int)(unitHealth * ratio);
        //
        //     FillHorizontalLine(data, rectangle.Width, rectangle.Height, 1, cncPalleteColorGreen, 1, healthBarLength);
        //     FillHorizontalLine(data, rectangle.Width, rectangle.Height, 2, cncPalleteColorGreen, 1, healthBarLength);
        //
        //     rectangle.SetData(data);
        //
        //     return rectangle;
        //
        // }

        // internal Texture2D InitializeHealthBarShadow()
        // {
        //     int healthBarHeight = 4;
        //     // int healthBarWidth = 12;  // This is hard coded for minigunner
        //     int healthBarWidth = myGameObject.UnitSize.Width;  
        //
        //     Texture2D rectangle =
        //         new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, healthBarWidth, healthBarHeight);
        //
        //     Color[] data = new Color[rectangle.Width * rectangle.Height];
        //
        //     Color cncPalleteColorShadow = new Color(255, 255, 255, 255);
        //
        //     FillHorizontalLine(data, rectangle.Width, rectangle.Height, 1, cncPalleteColorShadow);
        //     FillHorizontalLine(data, rectangle.Width, rectangle.Height, 2, cncPalleteColorShadow);
        //
        //     rectangle.SetData(data);
        //     return rectangle;
        // }



        private Texture2D CreateUnitSelectionTexture(int width, int height, int horizontalLength, int verticalLength)
        {

            string unitSelectionTextureKey = "UnitSelectionTexture-width-" + width + "-height-" + height;


            Texture2D unitSelectionTexture = MikeAndConquerGame.instance.SpriteSheet.GetTextureForKey(unitSelectionTextureKey);

            if (unitSelectionTexture == null)
            {
                Color cncPalleteColorWhite = new Color(255, 255, 255, 255);

                unitSelectionTexture =
                    new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, width, height);

                Color[] data = new Color[unitSelectionTexture.Width * unitSelectionTexture.Height];

                int startX = 0;
                int startY = 0;

                // top left
                DrawHorizontalLine(data, cncPalleteColorWhite, width, height, startX, startY, horizontalLength);
                DrawVerticalLine(data, cncPalleteColorWhite, width, height, startX, startY, verticalLength);

                // bottom left
                startY = height - verticalLength;
                DrawVerticalLine(data, cncPalleteColorWhite, width, height, startX, startY, verticalLength);
                startY = height - 1;
                DrawHorizontalLine(data, cncPalleteColorWhite, width, height, startX, startY, horizontalLength);

                // top right
                startX = width - horizontalLength;
                startY = 0;
                DrawHorizontalLine(data, cncPalleteColorWhite, width, height, startX, startY, horizontalLength);
                startX = width - 1;
                DrawVerticalLine(data, cncPalleteColorWhite, width, height, startX, startY, verticalLength);

                // bottom right
                startY = height - verticalLength;
                DrawVerticalLine(data, cncPalleteColorWhite, width, height, startX, startY, verticalLength);
                startX = width - horizontalLength;

                startY = height - 1;
                DrawHorizontalLine(data, cncPalleteColorWhite, width, height, startX, startY, horizontalLength);

                unitSelectionTexture.SetData(data);

                MikeAndConquerGame.instance.SpriteSheet.SetTextureForKey(unitSelectionTextureKey, unitSelectionTexture);

            }


            return unitSelectionTexture;
        }

        Texture2D InitializeSelectionCursor()
        {
            UnitSize unitSize = myUnitView.UnitSize;

            int width = unitSize.Width + 1;
            int height = unitSize.Height - 4 + 1;

            int horizontalLength = (unitSize.Width / 5) + 1;
            int verticalLength = (unitSize.Height / 5) + 1;

            return CreateUnitSelectionTexture(width, height, horizontalLength, verticalLength);

        }



        public void Update(GameTime gameTime)
        {

            // if (healthBarTexture != null)
            // {
            //     healthBarTexture.Dispose();
            // }
            // healthBarTexture = InitializeHealthBar();

            Point selectionCursorOffset = myUnitView.SelectionCursorOffset;
            // GameWorldLocation gameWorldLocation = myGameObject.GameWorldLocation;
            //
            //
            // selectionCursorPosition = new Vector2(
            //     gameWorldLocation.WorldCoordinatesAsVector2.X + selectionCursorOffset.X,
            //     gameWorldLocation.WorldCoordinatesAsVector2.Y + selectionCursorOffset.Y);


            selectionCursorPosition = new Vector2(
                myUnitView.XInWorldCoordinates + selectionCursorOffset.X,
                myUnitView.YInWorldCoordinates + selectionCursorOffset.Y);


            // healthBarPosition = selectionCursorPosition;
            // healthBarPosition.Y -= 4;

        }

        internal void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch, float layerDepth)
        {

            spriteBatch.Draw(selectionCursorTexture, selectionCursorPosition, null, Color.White, 0f, origin, defaultScale, SpriteEffects.None, layerDepth);
            // if (drawBoundingRectangle)
            // {
            //     
            //     spriteBatch.Draw(boundingRectangle, selectionCursorPosition, null, Color.White, 0f, origin, defaultScale, SpriteEffects.None, 0f);
            // }

            // spriteBatch.Draw(healthBarTexture, healthBarPosition, null, Color.White, 0f, origin, defaultScale, SpriteEffects.None, layerDepth);
        }

        internal void DrawShadowOnly(GameTime gameTime, SpriteBatch spriteBatch, float layerDepth)
        {
            // spriteBatch.Draw(healthBarShadowTexture, healthBarPosition, null, Color.White, 0f, origin, defaultScale, SpriteEffects.None, layerDepth);
        }

    }


}
