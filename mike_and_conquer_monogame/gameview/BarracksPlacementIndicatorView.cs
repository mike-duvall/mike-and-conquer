using System.Collections.Generic;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using SingleTextureSprite = mike_and_conquer_monogame.gamesprite.SingleTextureSprite;
using MapTileFrame = mike_and_conquer_monogame.gamesprite.MapTileFrame;
using MikeAndConquerGame = mike_and_conquer_monogame.main.MikeAndConquerGame;
using GameTime = Microsoft.Xna.Framework.GameTime;
using Color  = Microsoft.Xna.Framework.Color;

using XnaVector2 = Microsoft.Xna.Framework.Vector2;
using MonogameUtil =  mike_and_conquer_monogame.util.MonogameUtil;


namespace mike_and_conquer_monogame.gameview
{
    public class BarracksPlacementIndicatorView
    {

        private BarracksPlacementIndicator barracksPlacementIndicator;

        public SingleTextureSprite canPlaceBuildingSprite;
        private SingleTextureSprite canNotPlaceBuildingSprite;

        public static string FILE_NAME = "trans.icn";
//        private static Vector2 middleOfSpriteInSpriteCoordinates;

        public BarracksPlacementIndicatorView(BarracksPlacementIndicator barracksPlacementIndicator)
        {
            this.barracksPlacementIndicator = barracksPlacementIndicator;

            List<MapTileFrame> mapTileFrameList =
                MikeAndConquerGame.instance.SpriteSheet.GetMapTileFrameForTmpFile(FILE_NAME);

            // 0 == white
            // 1 == yellow
            // 2 == red
            this.canPlaceBuildingSprite = new SingleTextureSprite(mapTileFrameList[0].Texture);
            this.canNotPlaceBuildingSprite = new SingleTextureSprite(mapTileFrameList[2].Texture);
        }



        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (BuildingPlacementIndicatorTile tile in  barracksPlacementIndicator.BuildingBuildingPlacementIndicatorTiles)
            {
                XnaVector2 xnaVector2 =
                    MonogameUtil.ConvertSystemNumericsVector2ToXnaVector2(tile.MapTileLocation
                        .WorldCoordinatesAsVector2);

                if (tile.CanPlaceBuilding)
                {

                    canPlaceBuildingSprite.Draw(
                        gameTime,
                        spriteBatch,
                        xnaVector2,
                        SpriteSortLayers.MAP_SQUARE_DEPTH,
                        false,
                        Color.White);

                }
                else
                {
                    canNotPlaceBuildingSprite.Draw(
                        gameTime,
                        spriteBatch,
                        xnaVector2,
                        SpriteSortLayers.MAP_SQUARE_DEPTH,
                        false,
                        Color.White);

                }

            }
        }


    }
}