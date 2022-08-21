using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mike_and_conquer_monogame.util;
using mike_and_conquer_simulation.main;

namespace mike_and_conquer_monogame.gameview
{
    class PlannedPathStepView
    {

        private Texture2D pathIcon;

        private static Vector2 middleOfPathSpriteInSpriteCoordinates;

        public int X { get; }
        public int Y { get; }

        public PlannedPathStepView(int x, int y)
        {
            this.X = x;
            this.Y = y;

            int pathIconWidth = GameWorldView.MAP_TILE_WIDTH / 4;
            int pathIconHeight = GameWorldView.MAP_TILE_HEIGHT / 4;

            middleOfPathSpriteInSpriteCoordinates = new Vector2();
            middleOfPathSpriteInSpriteCoordinates.X = pathIconWidth / 2;
            middleOfPathSpriteInSpriteCoordinates.Y = pathIconHeight / 2;

            pathIcon = TextureUtil.CreateSpriteCenterRectangleTexture(
                new Color(0x7e, 0, 0),   // R component get's mapped to pallette color, which is a red in this case
                pathIconWidth,
                pathIconHeight);


        }

        public void Draw(SpriteBatch spriteBatch)
        {

            Point pointInWorldCoordinates =
                GameWorldView.ConvertMapTileCoordinatesToWorldCoordinates(new Point(this.X, this.Y));

            Vector2 worldCoordinatesAsXnaVector2 = new Vector2(pointInWorldCoordinates.X, pointInWorldCoordinates.Y);

            float defaultScale = 1;
            float layerDepth = 0;
            spriteBatch.Draw(pathIcon, worldCoordinatesAsXnaVector2, null, Color.White,
                0f, middleOfPathSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, layerDepth);

        }
    }

}
