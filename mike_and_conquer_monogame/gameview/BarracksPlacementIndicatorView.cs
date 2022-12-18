using System.Collections.Generic;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using SingleTextureSprite = mike_and_conquer_monogame.gamesprite.SingleTextureSprite;
using MapTileFrame = mike_and_conquer_monogame.gamesprite.MapTileFrame;
using MikeAndConquerGame = mike_and_conquer_monogame.main.MikeAndConquerGame;
using GameTime = Microsoft.Xna.Framework.GameTime;
using Color  = Microsoft.Xna.Framework.Color;

using XnaVector2 = Microsoft.Xna.Framework.Vector2;
using MonogameUtil =  mike_and_conquer_monogame.util.MonogameUtil;
using MapTileLocation = mike_and_conquer_simulation.gameworld.MapTileLocation;

using Point = Microsoft.Xna.Framework.Point;

namespace mike_and_conquer_monogame.gameview
{
    public class BarracksPlacementIndicatorView
    {

        public SingleTextureSprite canPlaceBuildingSprite;
        private SingleTextureSprite canNotPlaceBuildingSprite;

        public static string FILE_NAME = "trans.icn";
        //        private static Vector2 middleOfSpriteInSpriteCoordinates;


        private MapTileLocation mapTileLocation;

        public MapTileLocation MapTileLocation
        {
            get { return mapTileLocation; }
        }


        public BarracksPlacementIndicatorView(MapTileLocation mapTileLocation)
        {
            this.mapTileLocation = mapTileLocation;
            this.buildingBuildingPlacementIndicatorTiles = new List<BuildingPlacementIndicatorTile>();

            List<MapTileFrame> mapTileFrameList =
                MikeAndConquerGame.instance.SpriteSheet.GetMapTileFrameForTmpFile(FILE_NAME);

            // 0 == white
            // 1 == yellow
            // 2 == red
            this.canPlaceBuildingSprite = new SingleTextureSprite(mapTileFrameList[0].Texture);
            this.canNotPlaceBuildingSprite = new SingleTextureSprite(mapTileFrameList[2].Texture);

            AddTileAtRelativeLocation(0, 0);
            AddTileAtRelativeLocation(1, 0);

            AddTileAtRelativeLocation(0, 1);
            AddTileAtRelativeLocation(1, 1);

            AddTileAtRelativeLocation(0, 2);
            AddTileAtRelativeLocation(1, 2);

        }

        private void AddTileAtRelativeLocation(int x, int y)
        {
            BuildingPlacementIndicatorTile newTile = new BuildingPlacementIndicatorTile(this.MapTileLocation, x, y);
            buildingBuildingPlacementIndicatorTiles.Add(newTile);
        }

        public void UpdateLocationInWorldCoordinates(Point mouseLocationWordCoordinates)
        {
            mapTileLocation
                .XInWorldCoordinates(mouseLocationWordCoordinates.X)
                .YInWorldCoordinates(mouseLocationWordCoordinates.Y);

            foreach (BuildingPlacementIndicatorTile tile in buildingBuildingPlacementIndicatorTiles)
            {
                tile.UpdateLocation(mapTileLocation);
                tile.CanPlaceBuilding = false;
            }

            bool isAnyTileTouchingExistingBase = false;

            foreach (BuildingPlacementIndicatorTile tile in buildingBuildingPlacementIndicatorTiles)
            {
                Point xnaPoint = MonogameUtil.ConvertSystemDrawingPointToXnaPoint(tile.MapTileLocation.WorldCoordinatesAsPoint);
                if (GameWorldView.instance.IsPointAdjacentToConstructionYardAndClearForBuilding(xnaPoint))
                {
                    isAnyTileTouchingExistingBase = true;
                }
            }

            if (isAnyTileTouchingExistingBase)
            {
                foreach (BuildingPlacementIndicatorTile tile in buildingBuildingPlacementIndicatorTiles)
                {

                    if (GameWorldView.instance.IsValidMoveDestination(
                            tile.MapTileLocation.WorldCoordinatesAsPoint.X,
                            tile.MapTileLocation.WorldCoordinatesAsPoint.Y))
                    {
                        tile.CanPlaceBuilding = true;
                    }
                }
            }

        }



        private List<BuildingPlacementIndicatorTile> buildingBuildingPlacementIndicatorTiles;

        public List<BuildingPlacementIndicatorTile> BuildingBuildingPlacementIndicatorTiles
        {
            get { return buildingBuildingPlacementIndicatorTiles; }
        }



        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (BuildingPlacementIndicatorTile tile in  BuildingBuildingPlacementIndicatorTiles)
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

        public bool ValidBuildingLocation()
        {
            bool isValidBuildingLocation = true;

            foreach (BuildingPlacementIndicatorTile tile in buildingBuildingPlacementIndicatorTiles)
            {
                if (!tile.CanPlaceBuilding)
                {
                    isValidBuildingLocation = false;
                }
            }

            return isValidBuildingLocation;
        }



    }
}