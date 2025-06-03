

using mike_and_conquer_monogame.main;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Color = Microsoft.Xna.Framework.Color;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace mike_and_conquer_monogame.gameview
{
    internal class SquareView
    {
        private Texture2D squareViewTexture;

        public int XInWorldCoordinates { get; set; }
        public int YInWorldCoordinates { get; set; }



        public SquareView(int x, int y, int width, int height, Color mainColor, Color centerDotColor)
        {
            this.XInWorldCoordinates = x;
            this.YInWorldCoordinates = y;

            string textureName = "HollowSquare-Color-R-" + mainColor.R + "-G-" + mainColor.G + "-B-" + mainColor.B +
                                         "-width-" + width + "-height-" + height;

            squareViewTexture = MikeAndConquerGame.instance.SpriteSheet.GetTextureForKey(textureName);

            if (squareViewTexture == null)
            {

                squareViewTexture =
                    new Texture2D(
                        MikeAndConquerGame.instance.GraphicsDevice,
                        width,
                        height);

                Color[] data = new Color[squareViewTexture.Width * squareViewTexture.Height];
                FillHorizontalLine(data, squareViewTexture.Width, squareViewTexture.Height, 0, mainColor);
                FillHorizontalLine(data, squareViewTexture.Width, squareViewTexture.Height, squareViewTexture.Height - 1, mainColor);
                FillVerticalLine(data, squareViewTexture.Width, squareViewTexture.Height, 0, mainColor);
                FillVerticalLine(data, squareViewTexture.Width, squareViewTexture.Height, squareViewTexture.Width - 1, mainColor);

                int centerX = (squareViewTexture.Width / 2);
                int centerY = (squareViewTexture.Height / 2);

                // Check how this works for even sized sprites with true center

                int centerOffset = (centerY * squareViewTexture.Width) + centerX;

                data[centerOffset] = centerDotColor;

                squareViewTexture.SetData(data);
                MikeAndConquerGame.instance.SpriteSheet.SetTextureForKey(textureName, squareViewTexture);
            }

        }

        internal void Update(GameTime gameTime, int newX, int newY)
        {
            XInWorldCoordinates = newX;
            YInWorldCoordinates = newY;
        }

        internal void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch)
        {
            float defaultScale = 1.0f;
            float layerDepth = 0.0f;
            Vector2 selectionCursorPosition = new Vector2(XInWorldCoordinates, YInWorldCoordinates);
            Vector2 middleOfUnitSelectionCursor = new Vector2(squareViewTexture.Width / 2, squareViewTexture.Height / 2);

            spriteBatch.Draw(squareViewTexture, selectionCursorPosition, null, Color.White, 0f, middleOfUnitSelectionCursor, defaultScale,
                SpriteEffects.None, layerDepth);
        }


        internal void FillHorizontalLine(Color[] data, int width, int height, int lineIndex, Color color)
        {
            int beginIndex = width * lineIndex;
            for (int i = beginIndex; i < (beginIndex + width); ++i)
            {
                data[i] = color;
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




    }
}
