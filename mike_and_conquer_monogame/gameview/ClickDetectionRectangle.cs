using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mike_and_conquer_monogame.gameview
{
    public class ClickDetectionRectangle
    {

        private int xInWorldCoordinates;
        private int yInWorldCoordinates;
        private int width;
        private int height;

        SquareView squareView;

        public ClickDetectionRectangle(int xInWorldCoordinates, int yInWorldCoordinates, int width, int height)
        {
            this.xInWorldCoordinates = xInWorldCoordinates;
            this.yInWorldCoordinates = yInWorldCoordinates;
            this.width = width;
            this.height = height;

            squareView = new SquareView(
                xInWorldCoordinates,
                yInWorldCoordinates,
                width,
                height,
                CnCColorAsXnaColor.CncTeal_14_38_36,
                CnCColorAsXnaColor.CncRed_55_05_02);
        }



        public Rectangle GetRectangle( )
        {
            int clickDetectionRectnagleX = (int)(xInWorldCoordinates - (width / 2));
            int clickDetectionRectangleY = (int)(yInWorldCoordinates - (height / 2));

            return new Rectangle(clickDetectionRectnagleX, clickDetectionRectangleY, width, height);
        }

        public bool Contains(int x, int y)
        {
            return x >= xInWorldCoordinates && x <= xInWorldCoordinates + width && y >= yInWorldCoordinates && y <= yInWorldCoordinates + height;
        }

        internal void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch)
        {
            squareView.DrawNoShadow(gameTime, spriteBatch);
        }

        internal void Update(GameTime gameTime, int xInWorldCoordinates, int yInWorldCoordinates)
        {
            this.xInWorldCoordinates = xInWorldCoordinates;
            this.yInWorldCoordinates = yInWorldCoordinates;
            squareView.Update(gameTime, xInWorldCoordinates, yInWorldCoordinates);
        }

    }
}
