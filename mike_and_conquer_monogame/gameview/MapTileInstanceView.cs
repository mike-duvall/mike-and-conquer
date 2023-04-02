
using System.Collections.Generic;

using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using XnaVector2 = Microsoft.Xna.Framework.Vector2;
using XnaRectangle = Microsoft.Xna.Framework.Rectangle;
using XnaColor = Microsoft.Xna.Framework.Color;
using GameTime = Microsoft.Xna.Framework.GameTime;
using XnaPoint = Microsoft.Xna.Framework.Point;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using SpriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects;

using TextureUtil = mike_and_conquer_monogame.util.TextureUtil;
using SingleTextureSprite = mike_and_conquer_monogame.gamesprite.SingleTextureSprite;
using PartiallyVisibileMapTileMask= mike_and_conquer_monogame.gamesprite.PartiallyVisibileMapTileMask;
using MapTileFrame = mike_and_conquer_monogame.gamesprite.MapTileFrame;
using MikeAndConquerGame = mike_and_conquer_monogame.main.MikeAndConquerGame;

using GameOptions = mike_and_conquer_monogame.main.GameOptions;
using SimulationMapTileLocation = mike_and_conquer_simulation.gameworld.MapTileLocation;

using NumericsVector2 = System.Numerics.Vector2;
using SystemDrawingPoint = System.Drawing.Point;
using MapTileLocation = mike_and_conquer_simulation.gameworld.MapTileLocation;

namespace mike_and_conquer_monogame.gameview
{
    public class MapTileInstanceView
    {
        public SingleTextureSprite singleTextureSprite;

        // TODO Refactor handling of map shroud masks.  Consider pulling out everything into separate class(es)
        private static Texture2D visibleMask = null;
        private Texture2D currentVisibilityMask = null;
        private PartiallyVisibileMapTileMask partiallyVisibileMapTileMask;
        private static XnaVector2 middleOfSpriteInSpriteCoordinates;

        private int imageIndex;
        // private string textureKey;
        private bool isBlockingTerrain;



        public bool IsBlockingTerrain
        {
            get { return isBlockingTerrain;}
        }


        public MapTileVisibility visibility;

        private Texture2D mapTileBorder;
        private Texture2D mapTileBlockingTerrainBorder;


        private XnaRectangle boundingRectangle;
        private bool boundingRectangleInitialized;

        public int mapTileInstanceId;


        public enum MapTileVisibility
        {
            NotVisible,
            Visible
        }

        public SimulationMapTileLocation SimulationMapTileLocation
        {
            get { return mapTileLocation; }
        }

        private SimulationMapTileLocation mapTileLocation;

        public MapTileInstanceView(int mapTileInstanceId,int xInWorldMapTileCoordinates, int yInWorldMapTileCoordinates, int imageIndex, string textureKey, bool isBlockingTerrain, MapTileVisibility mapTileVisibility)
        {

            this.mapTileInstanceId = mapTileInstanceId;
            this.mapTileLocation = SimulationMapTileLocation.CreateFromWorldMapTileCoordinates(
                xInWorldMapTileCoordinates,
                yInWorldMapTileCoordinates);
            this.imageIndex = imageIndex;
            // this.textureKey = textureKey;
            this.isBlockingTerrain = isBlockingTerrain;
            this.visibility = mapTileVisibility;
            List<MapTileFrame> mapTileFrameList =
                MikeAndConquerGame.instance.SpriteSheet.GetMapTileFrameForTmpFile(textureKey);
            this.singleTextureSprite = new SingleTextureSprite(mapTileFrameList[imageIndex].Texture);


            InitializeVisibleMask(mapTileFrameList);

            partiallyVisibileMapTileMask = new PartiallyVisibileMapTileMask();

            mapTileBorder = TextureUtil.CreateSpriteBorderRectangleTexture(
                XnaColor.White,
                this.singleTextureSprite.Width,
                this.singleTextureSprite.Height);

            mapTileBlockingTerrainBorder = TextureUtil.CreateSpriteBorderRectangleTexture(
                new XnaColor(127, 255, 255, 255),
                this.singleTextureSprite.Width,
                this.singleTextureSprite.Height);


        }


        private void InitializeVisibleMask(List<MapTileFrame> mapTileFrameList)
        {
            if (MapTileInstanceView.visibleMask == null)
            {
                visibleMask = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice,
                    mapTileFrameList[imageIndex].Texture.Width, mapTileFrameList[imageIndex].Texture.Height);

                int numPixels = mapTileFrameList[imageIndex].Texture.Width *
                                mapTileFrameList[imageIndex].Texture.Height;

                XnaColor[] textureData = new XnaColor[numPixels];
                visibleMask.GetData(textureData);

                for (int i = 0; i < numPixels; i++)
                {
                    XnaColor xnaColor = XnaColor.Transparent;
                    textureData[i] = xnaColor;
                }

                visibleMask.SetData(textureData);
                middleOfSpriteInSpriteCoordinates = new XnaVector2();

                middleOfSpriteInSpriteCoordinates.X = visibleMask.Width / 2;
                middleOfSpriteInSpriteCoordinates.Y = visibleMask.Height / 2;
            }
        }


        //        internal int GetPaletteIndexOfCoordinate(int x, int y)
        //        {
        //            List<MapTileFrame> mapTileFrameList =
        //                MikeAndConquerGame.instance.SpriteSheet.GetMapTileFrameForTmpFile(textureKey);
        //            MapTileFrame mapTileFrame = mapTileFrameList[imageIndex];
        //            byte[] frameData = mapTileFrame.FrameData;
        //
        //            int frameDataIndex = y * canPlaceBuildingSprite.Width + x;
        //            return frameData[frameDataIndex];
        //        }





        internal XnaVector2 ConvertNumericsVector2ToXnaVector2(NumericsVector2 numericsVector2)
        {
            XnaVector2 xnaVector2 = new XnaVector2(
                numericsVector2.X,
                numericsVector2.Y);

            return xnaVector2;

        }



        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {


            XnaVector2 worldCoordinatesAsXnaVector2 =
                ConvertNumericsVector2ToXnaVector2(mapTileLocation.WorldCoordinatesAsVector2);

            singleTextureSprite.Draw(
                gameTime,
                spriteBatch,
                worldCoordinatesAsXnaVector2,
                SpriteSortLayers.MAP_SQUARE_DEPTH,
                false,
                XnaColor.White);

          
            if (GameOptions.instance.DrawBlockingTerrainBorder && this.isBlockingTerrain)
            {
                float defaultScale = 1;
                float layerDepth = 0;
                spriteBatch.Draw(mapTileBlockingTerrainBorder, worldCoordinatesAsXnaVector2, null,
                    XnaColor.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, layerDepth);
            }
            else if (GameOptions.instance.DrawTerrainBorder)
            {
                float defaultScale = 1;
                float layerDepth = 0;
                spriteBatch.Draw(mapTileBorder, worldCoordinatesAsXnaVector2, null, XnaColor.White,
                    0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, layerDepth);
            }


        }




        // From Cnc code:
            // * OUTPUT:  Returns with the shadow icon to use. -2= all black.                                *
            // *                                                -1= map cell.                                *
            // *                                                                                             *


        // "Card" means cardinal direction, which means, North, South, East, or West
        // Algorithm below lifted from real Cnc C++ algorithm
        // DISPLAY.CPP, line 1635, method:  int DisplayClass::Cell_Shadow(CELL cell, HouseClass *house)
        static readonly sbyte[] CardShadow = new sbyte[] { -2, 0, 1, 2, 3, -1, 4, -1, 5, 6, -1, -1, 7, -1, -1, -1 };
        static readonly sbyte[] DiagShadow = new sbyte[] { -2, 8, 9, -1, 10, -1, -1, -1, 11, -1, -1, -1, -1, -1, -1, -1 };

        private sbyte DeterminePartiallyVisibleMaskTile()
        {
            sbyte value = -2;

            if (this.visibility != MapTileVisibility.Visible)
            {
                int index = 0;
                MapTileInstanceView west = GameWorldView.instance.FindAdjacentMapTileInstanceViewAllowNull(
                    this.mapTileLocation,
                    MapTileLocation.TILE_LOCATION.WEST);


                if (west != null && west.visibility == MapTileVisibility.Visible)
                {
                    index |= 0x08;

                }

                MapTileInstanceView south = GameWorldView.instance.FindAdjacentMapTileInstanceViewAllowNull(
                    this.mapTileLocation,
                    MapTileLocation.TILE_LOCATION.SOUTH);


                if (south != null && south.visibility == MapTileVisibility.Visible)
                {
                    index |= 0x04;

                }
                MapTileInstanceView east = GameWorldView.instance.FindAdjacentMapTileInstanceViewAllowNull(
                    this.mapTileLocation,
                    MapTileLocation.TILE_LOCATION.EAST);

                if (east != null && east.visibility == MapTileVisibility.Visible)
                {
                    index |= 0x02;
                }

                MapTileInstanceView north = GameWorldView.instance.FindAdjacentMapTileInstanceViewAllowNull(
                    this.mapTileLocation,
                    MapTileLocation.TILE_LOCATION.NORTH);

                if (north!= null && north.visibility == MapTileVisibility.Visible)
                {
                    index |= 0x01;
                }

                value = CardShadow[index];


                if (value == -2)
                {
                    index = 0;
                    // Last place was North, so
                    // cellptr--;  // NORTHWEST
                    // if (cellptr->Is_Mapped(house)) index |= 0x08;
                    MapTileInstanceView northWest = GameWorldView.instance.FindAdjacentMapTileInstanceViewAllowNull(
                        this.mapTileLocation,
                        MapTileLocation.TILE_LOCATION.NORTH_WEST);

                    if (northWest != null && northWest.visibility == MapTileVisibility.Visible)
                    {
                        index |= 0x08;
                    }


                    // cellptr += MAP_CELL_W * 2; // SOUTHWEST
                    // if (cellptr->Is_Mapped(house)) index |= 0x04;
                    MapTileInstanceView southWest = GameWorldView.instance.FindAdjacentMapTileInstanceViewAllowNull(
                        this.mapTileLocation,
                        MapTileLocation.TILE_LOCATION.SOUTH_WEST);

                    if (southWest != null && southWest.visibility == MapTileVisibility.Visible)
                    {
                        index |= 0x04;
                    }



                    // cellptr += 2;  // SOUTHEAST
                    // if (cellptr->Is_Mapped(house)) index |= 0x02;
                    MapTileInstanceView southEast = GameWorldView.instance.FindAdjacentMapTileInstanceViewAllowNull(
                        this.mapTileLocation,
                        MapTileLocation.TILE_LOCATION.SOUTH_EAST);

                    if (southEast != null && southEast.visibility == MapTileVisibility.Visible)
                    {
                        index |= 0x02;
                    }


                    // cellptr -= MAP_CELL_W * 2;  // NORTHEAST
                    // if (cellptr->Is_Mapped(house)) index |= 0x01;
                    MapTileInstanceView northEast = GameWorldView.instance.FindAdjacentMapTileInstanceViewAllowNull(
                        this.mapTileLocation,
                        MapTileLocation.TILE_LOCATION.NORTH_EAST);

                    if (northEast != null && northEast.visibility == MapTileVisibility.Visible)
                    {
                        index |= 0x01;
                    }



                    value = DiagShadow[index];

                }

                if (value == -1)
                {
                    this.visibility = MapTileVisibility.Visible;
                    // TODO: Possibly insert code or signal here to re-check nearby map tiles for visibility
                }

            }

            return value;

        }


        internal void UpdateVisbilityMask()
        {
            if (this.visibility == MapTileVisibility.Visible)
            {
                this.currentVisibilityMask = visibleMask;

            }
            else // if (this.visibility == MapTileVisibility.PartiallyVisible)
            {
                int index = DeterminePartiallyVisibleMaskTile();
                if (index >= 0)
                {
                    this.currentVisibilityMask = partiallyVisibileMapTileMask.GetMask(index);
                }
            }

        }

        internal void DrawVisbilityMask(GameTime gameTime, SpriteBatch spriteBatch)
        {
            float defaultScale = 1;
            
            XnaVector2 worldCoordinatesAsXnaVector2 =
                ConvertNumericsVector2ToXnaVector2(mapTileLocation.WorldCoordinatesAsVector2);

            if (currentVisibilityMask != null)
            {
                spriteBatch.Draw(currentVisibilityMask,
                    worldCoordinatesAsXnaVector2, null, XnaColor.White, 0f,
                    middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 1.0f);

            }
        }


        public bool ContainsPoint(int mouseX, int mouseY)
        {
            return GetBoundingRectangle().Contains(new XnaPoint(mouseX, mouseY));
        }


        private XnaRectangle GetBoundingRectangle()
        {

            if (boundingRectangleInitialized == false)
            {
                int width = GameWorldView.MAP_TILE_WIDTH;
                int height = GameWorldView.MAP_TILE_HEIGHT;

                SystemDrawingPoint worldCoordinatesAsSystemDrawingPoint = mapTileLocation.WorldCoordinatesAsPoint;

                int leftX = worldCoordinatesAsSystemDrawingPoint.X - (width / 2);
                int topY = worldCoordinatesAsSystemDrawingPoint.Y - (height / 2);

                boundingRectangle = new XnaRectangle(leftX, topY, width, height);
                boundingRectangleInitialized = true;
            }

            return boundingRectangle;

        }



        public XnaPoint GetCenter()
        {
            // int xInWorldMapTileCoordinates = mapTileLocation.WorldMapTileCoordinatesAsPoint.X;
            // int yInWorldMapTileCoordinates = mapTileLocation.WorldMapTileCoordinatesAsPoint.Y;
            //
            // return MapTileInstanceView.ConvertMapTileCoordinatesToWorldCoordinates(new XnaPoint(xInWorldMapTileCoordinates,
            //     yInWorldMapTileCoordinates));
            int xInWorldCoordinates = (mapTileLocation.XInWorldMapTileCoordinates * GameWorldView.MAP_TILE_WIDTH) +
                                      (GameWorldView.MAP_TILE_WIDTH / 2);
            int yInWorldCoordinates = mapTileLocation.YInWorldMapTileCoordinates * GameWorldView.MAP_TILE_HEIGHT +
                                      (GameWorldView.MAP_TILE_HEIGHT / 2);

            return new XnaPoint(xInWorldCoordinates, yInWorldCoordinates);


        }

    }
}
