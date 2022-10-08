using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
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
        private PartiallyVisibileMapTileMask partiallyVisibileMapTileMask;
        private static XnaVector2 middleOfSpriteInSpriteCoordinates;

        private int imageIndex;
        // private string textureKey;
        private bool isBlockingTerrain;


        public MapTileVisibility visibility;

        private Texture2D mapTileBorder;
        private Texture2D mapTileBlockingTerrainBorder;

        // private List<MapTileShroudMapping> mapTileShroudMappingList;

        private XnaRectangle boundingRectangle;
        private bool boundingRectangleInitialized;

        public int mapTileInstanceId;


        public enum MapTileVisibility
        {
            NotVisible,
            Visible
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


            // InitializeMapTileShroudMappingList();
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



        // TODO:  Consider pulling map shroud code into seprate class(es)
        // private void InitializeMapTileShroudMappingList()
        // {
        //     mapTileShroudMappingList = new List<MapTileShroudMapping>();
        //
        //
        //
        //     // Original
        //     // mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //     //     MapTileInstanceView.MapTileVisibility.PartiallyVisible,
        //     //     MapTileInstanceView.MapTileVisibility.NotVisible,
        //     //     MapTileInstanceView.MapTileVisibility.PartiallyVisible,
        //     //     MapTileInstanceView.MapTileVisibility.Visible,
        //     //     0));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         0));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         0));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         0));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         0));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         0));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         0));  // TODO:  No test fails if this mapping is not here
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         0));  // TODO:  No test fails if this mapping is not here
        //
        //
        //
        //
        //     // Original
        //     // mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //     //     MapTileVisibility.NotVisible,
        //     //     MapTileVisibility.NotVisible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.Visible,
        //     //     0));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.NotVisible,
        //         0));
        //
        //
        //     // Original
        //     // mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //     //     MapTileVisibility.Visible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.NotVisible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     1));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         1));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         1));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         1));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         1));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         1));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         1));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         1));
        //
        //
        //
        //
        //     // east: Visible,
        //     // south: Partial,
        //     // west: Partial,
        //     // north: Partial
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         1));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         1));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         1));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         1));  // TODO:  No test to justify this mapping
        //
        //
        //
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         1));
        //
        //
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         1));
        //
        //
        //
        //
        //     // Original
        //     // mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //     //     MapTileVisibility.Visible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.Visible,
        //     //     2));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         2));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         2));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         2));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         2));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         2));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         2));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         2));
        //
        //
        //
        //
        //
        //     // Original
        //     // mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.Visible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.NotVisible,
        //     //     3));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         3));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         3));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         3));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         3));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         3));
        //
        //
        //
        //
        //
        //
        //
        //     // Original
        //     // mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //     //     MapTileVisibility.NotVisible,
        //     //     MapTileVisibility.Visible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.NotVisible,
        //     //     3));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         3));
        //
        //
        //     // Original
        //     // mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //     //     MapTileVisibility.Visible,
        //     //     MapTileVisibility.Visible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     4));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         4));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         4));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         4));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         4));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         4));
        //
        //
        //     // Original
        //     // mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //     //     MapTileVisibility.NotVisible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.Visible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     5));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         5));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         5));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         5));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         5));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         5));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         5));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         5));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         5));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         5));
        //
        //
        //     // Original
        //     // mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.Visible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     5));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         5));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         5));
        //
        //
        //
        //
        //     // Original
        //     // mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.Visible,
        //     //     MapTileVisibility.Visible,
        //     //     6));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         6));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         6));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         6));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         6));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         6));
        //
        //
        //     // Original
        //     // mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.Visible,
        //     //     MapTileVisibility.Visible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     7));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         7));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         7));
        //
        //
        //
        //     // Original
        //     // mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //     //     MapTileVisibility.NotVisible,
        //     //     MapTileVisibility.Visible,
        //     //     MapTileVisibility.Visible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.NotVisible,
        //     //     MapTileVisibility.NotVisible,
        //     //     7));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         7));
        //
        //     // original
        //     // mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.NotVisible,
        //     //     MapTileVisibility.NotVisible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     8));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         8));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         8));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         8));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         8));
        //
        //     // Original
        //     // mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.NotVisible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.Visible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     8));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         8));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         8));  // TODO:  No test for this one
        //
        //
        //
        //     // Original
        //     // mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     8));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         8));
        //
        //
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         8));
        //
        //
        //     // Original
        //     // mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.NotVisible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.Visible,
        //     //     9));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         9));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         9));
        //
        //
        //     // Original
        //     // mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.NotVisible,
        //     //     MapTileVisibility.NotVisible,
        //     //     9));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         9));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         9));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         9));
        //
        //
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         9));
        //
        //     // east PartiallyVisible
        //     // south PartiallyVisible
        //     // west PartiallyVisible
        //     // north PartiallyVisible
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         9));
        //
        //
        //
        //     // Original
        //     // mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //     //     MapTileVisibility.NotVisible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.NotVisible,
        //     //     10));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         10));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         10));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         10));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         10));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         10));
        //
        //
        //
        //     // Original
        //     // mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //     //     MapTileVisibility.NotVisible,
        //     //     MapTileVisibility.NotVisible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     11));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         11));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         11));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         11));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         11));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         11));
        //
        //
        //
        //
        //     // Original
        //     // mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.NotVisible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     MapTileVisibility.PartiallyVisible,
        //     //     11));
        //     mapTileShroudMappingList.Add(new MapTileShroudMapping(
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.NotVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.Visible,
        //         MapTileVisibility.PartiallyVisible,
        //         MapTileVisibility.PartiallyVisible,
        //         11));
        //
        //
        //
        // }

        // private bool VisibilityMatches(Nullable<MapTileVisibility> expectedVisibility,
        //     Nullable<MapTileVisibility> actualVisibility)
        // {
        //     if (!expectedVisibility.HasValue)
        //     {
        //         return true;
        //     }
        //
        //     return expectedVisibility == actualVisibility;
        // }




        // private int FindMapTileShroudMapping(MapTileVisibility east,
        //     MapTileVisibility south,
        //     MapTileVisibility west,
        //     MapTileVisibility north,
        //     MapTileVisibility northEast,
        //     MapTileVisibility southEast,
        //     MapTileVisibility southWest,
        //     MapTileVisibility northWest)
        // {
        //     {
        //
        //     }
        //     foreach (MapTileShroudMapping mapping in mapTileShroudMappingList)
        //     {
        //
        //         if (mapping.east == east &&
        //             mapping.south == south &&
        //             mapping.west == west &&
        //             mapping.north == north &&
        //             // VisibilityMatches(mapping.northEast, northEast) &&
        //             // VisibilityMatches(mapping.southEast, southEast) &&
        //             // VisibilityMatches(mapping.southWest, southWest) &&
        //             // VisibilityMatches(mapping.northWest, northWest))
        //             mapping.northEast == northEast &&
        //             mapping.southEast == southEast &&
        //             mapping.southWest == southWest &&
        //             mapping.northWest == northWest)
        //
        //         {
        //
        //
        //             // if (!mapping.northEast.HasValue || !mapping.southEast.HasValue || !mapping.southWest.HasValue ||
        //             //     !mapping.northWest.HasValue)
        //             // {
        //             //
        //             //
        //             //     String nullEntryMessages = "\nMapping had null entries: \neast:" + mapping.east + "\n" +
        //             //                         "south:" + mapping.south + "\n" +
        //             //                         "west:" + mapping.west + "\n" +
        //             //                         "north:" + mapping.north + "\n" +
        //             //                         "shroudTileIndex=" + mapping.shroudTileIndex;
        //             //
        //             //     MikeAndConquerGame.instance.log.Information(nullEntryMessages);
        //             //
        //             //
        //             //     String matchingMapping = "\n\nmapTileShroudMappingList.Add(new MapTileShroudMapping( \n" +
        //             //                              "MapTileVisibility." + east + ", \n" +
        //             //                              "MapTileVisibility." + southEast + ", \n" +
        //             //                              "MapTileVisibility." + south + ", \n" +
        //             //                              "MapTileVisibility." + southWest + ", \n" +
        //             //                              "MapTileVisibility." + west + ", \n" +
        //             //                              "MapTileVisibility." + northWest + ", \n" +
        //             //                              "MapTileVisibility." + north + ", \n" +
        //             //                              "MapTileVisibility." + northEast + ", \n" +
        //             //                              mapping.shroudTileIndex + "));";
        //             //     MikeAndConquerGame.instance.log.Information(matchingMapping);
        //             //
        //             //
        //             //     if (east == MapTileVisibility.NotVisible &&
        //             //         south == MapTileVisibility.NotVisible &&
        //             //         west == MapTileVisibility.PartiallyVisible &&
        //             //         north == MapTileVisibility.Visible)
        //             //     {
        //             //         int xx = 3;
        //             //     }
        //             //
        //             //
        //             //
        //             // }
        //
        //
        //             return mapping.shroudTileIndex;
        //         }
        //     }
        //
        //
        //     //            throw new Exception("Didn't find match");
        //     String missingMapping = "\nMissing mapping:\nmapTileShroudMappingList.Add(new MapTileShroudMapping( \n" +
        //                              "MapTileVisibility." + east + ", \n" +
        //                              "MapTileVisibility." + southEast + ", \n" +
        //                              "MapTileVisibility." + south + ", \n" +
        //                              "MapTileVisibility." + southWest + ", \n" +
        //                              "MapTileVisibility." + west + ", \n" +
        //                              "MapTileVisibility." + northWest + ", \n" +
        //                              "MapTileVisibility." + north + ", \n" +
        //                              "MapTileVisibility." + northEast + ", \n" +
        //                              "?));";
        //
        //
        //     // MikeAndConquerGame.instance.log.Information(missingMapping);
        //     MikeAndConquerGame.instance.logger.LogInformation(missingMapping);
        //
        //     // throw new Exception("Didn't find match");
        //     return PartiallyVisibileMapTileMask.MISSING_MAPPING_INDEX;
        // }


        // private int DeterminePartiallyVisibleMaskTile()
        // {
        //     static char const CardShadow[16] = { -2, 0, 1, 2, 3, -1, 4, -1, 5, 6, -1, -1, 7, -1, -1, -1 };
        //     static char const DiagShadow[16] = { -2, 8, 9, -1, 10, -1, -1, -1, 11, -1, -1, -1, -1, -1, -1, -1 };
        //
        //
        //     if (!cellptr->Is_Mapped(house))
        //     {
        //
        //         /*
        //         **	Check the cardinal directions first. This will either result
        //         **	in a solution or the flag to check the diagonals.
        //         */
        //         index = 0;
        //         cellptr--;
        //         if (cellptr->Is_Mapped(house)) index |= 0x08;
        //         cellptr += MAP_CELL_W + 1;
        //         if (cellptr->Is_Mapped(house)) index |= 0x04;
        //         cellptr -= MAP_CELL_W - 1;
        //         if (cellptr->Is_Mapped(house)) index |= 0x02;
        //         cellptr -= MAP_CELL_W + 1;
        //         if (cellptr->Is_Mapped(house)) index |= 0x01;
        //         value = CardShadow[index];
        //
        //         /*
        //         **	The diagonals must be checked, since the cardinal directions
        //         **	did not yield a valid result.
        //         */
        //         if (value == -2)
        //         {
        //             index = 0;
        //             cellptr--;
        //             if (cellptr->Is_Mapped(house)) index |= 0x08;
        //             cellptr += MAP_CELL_W * 2;
        //             if (cellptr->Is_Mapped(house)) index |= 0x04;
        //             cellptr += 2;
        //             if (cellptr->Is_Mapped(house)) index |= 0x02;
        //             cellptr -= MAP_CELL_W * 2;
        //             if (cellptr->Is_Mapped(house)) index |= 0x01;
        //             value = DiagShadow[index];
        //         }
        //
        //
        //         throw new Exception("Not implemented");
        // }


       static readonly sbyte[] CardShadow = new sbyte[] { -2, 0, 1, 2, 3, -1, 4, -1, 5, 6, -1, -1, 7, -1, -1, -1 };
       static readonly sbyte[] DiagShadow = new sbyte[] { -2, 8, 9, -1, 10, -1, -1, -1, 11, -1, -1, -1, -1, -1, -1, -1 };

        private sbyte DeterminePartiallyVisibleMaskTile()
        {

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

                sbyte value = CardShadow[index];


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
                }


                return value;
                // Pickup here
                // Continue implementing this logic
                // Add diagonal logic
                //     And possibly also better handling of edge scenarios
                //
                // return CardShadow[index];

            }

            // if (!cellptr->Is_Mapped(house))
            // {
            //
            //     /*
            //     **	Check the cardinal directions first. This will either result
            //     **	in a solution or the flag to check the diagonals.
            //     */
            //     index = 0;
            //     cellptr--;
            //     if (cellptr->Is_Mapped(house)) index |= 0x08;
            //     cellptr += MAP_CELL_W + 1;
            //     if (cellptr->Is_Mapped(house)) index |= 0x04;
            //     cellptr -= MAP_CELL_W - 1;
            //     if (cellptr->Is_Mapped(house)) index |= 0x02;
            //     cellptr -= MAP_CELL_W + 1;
            //     if (cellptr->Is_Mapped(house)) index |= 0x01;
            //     value = CardShadow[index];
            //
            //     /*
            //     **	The diagonals must be checked, since the cardinal directions
            //     **	did not yield a valid result.
            //     */
            //     if (value == -2)
            //     {
            //         index = 0;
            //         cellptr--;
            //         if (cellptr->Is_Mapped(house)) index |= 0x08;
            //         cellptr += MAP_CELL_W * 2;
            //         if (cellptr->Is_Mapped(house)) index |= 0x04;
            //         cellptr += 2;
            //         if (cellptr->Is_Mapped(house)) index |= 0x02;
            //         cellptr -= MAP_CELL_W * 2;
            //         if (cellptr->Is_Mapped(house)) index |= 0x01;
            //         value = DiagShadow[index];
            //     }


            throw new Exception("Not implemented");
            }



            // private int DeterminePartiallyVisibleMaskTile()
            // {
            //
            //
            //
            //     MapTileInstanceView south = GameWorldView.instance.FindAdjacentMapTileInstanceViewAllowNull(
            //         this.mapTileLocation,
            //         MapTileLocation.TILE_LOCATION.SOUTH);
            //
            //     MapTileVisibility southVisibility = MapTileVisibility.NotVisible;
            //     if (south != null)
            //     {
            //         southVisibility = south.visibility;
            //     }
            //
            //
            //     // MapTileInstance north = GameWorld.instance.FindMapTileInstanceAllowNull(
            //     //     myMapTileInstance.MapTileLocation.CreateAdjacentMapTileLocation(GameWorld.TILE_LOCATION.NORTH));
            //
            //
            //     MapTileInstanceView north = GameWorldView.instance.FindAdjacentMapTileInstanceViewAllowNull(
            //         this.mapTileLocation,
            //         MapTileLocation.TILE_LOCATION.NORTH);
            //
            //
            //
            //     MapTileVisibility northVisibility = MapTileVisibility.NotVisible;
            //     if (north != null)
            //     {
            //         northVisibility = north.visibility;
            //     }
            //
            //     // MapTileInstance west = GameWorld.instance.FindMapTileInstanceAllowNull(
            //     //     myMapTileInstance.MapTileLocation.CreateAdjacentMapTileLocation(GameWorld.TILE_LOCATION.WEST));
            //     MapTileInstanceView west = GameWorldView.instance.FindAdjacentMapTileInstanceViewAllowNull(
            //         this.mapTileLocation,
            //         MapTileLocation.TILE_LOCATION.WEST);
            //
            //
            //     MapTileVisibility westVisibility = MapTileVisibility.NotVisible;
            //     if (west != null)
            //     {
            //         westVisibility = west.visibility;
            //     }
            //     
            //     // MapTileInstance east = GameWorld.instance.FindMapTileInstanceAllowNull(
            //     //     myMapTileInstance.MapTileLocation.CreateAdjacentMapTileLocation(GameWorld.TILE_LOCATION.EAST));
            //
            //     MapTileInstanceView east = GameWorldView.instance.FindAdjacentMapTileInstanceViewAllowNull(
            //         this.mapTileLocation,
            //         MapTileLocation.TILE_LOCATION.EAST);
            //
            //
            //     MapTileVisibility eastVisibility = MapTileVisibility.NotVisible;
            //     if (east != null)
            //     {
            //         eastVisibility = east.visibility;
            //     }
            //
            //     // MapTileInstance northEast = GameWorld.instance.FindMapTileInstanceAllowNull(
            //     //     myMapTileInstance.MapTileLocation.CreateAdjacentMapTileLocation(GameWorld.TILE_LOCATION.NORTH_EAST));
            //
            //     MapTileInstanceView northEast = GameWorldView.instance.FindAdjacentMapTileInstanceViewAllowNull(
            //         this.mapTileLocation,
            //         MapTileLocation.TILE_LOCATION.NORTH_EAST);
            //
            //
            //     MapTileVisibility northEastVisibility = MapTileVisibility.NotVisible;
            //     if (northEast != null)
            //     {
            //         northEastVisibility = northEast.visibility;
            //     }
            //
            //     // MapTileInstance southEast = GameWorld.instance.FindMapTileInstanceAllowNull(
            //     //     myMapTileInstance.MapTileLocation.CreateAdjacentMapTileLocation(GameWorld.TILE_LOCATION.SOUTH_EAST));
            //     MapTileInstanceView southEast = GameWorldView.instance.FindAdjacentMapTileInstanceViewAllowNull(
            //         this.mapTileLocation,
            //         MapTileLocation.TILE_LOCATION.SOUTH_EAST);
            //
            //
            //     MapTileVisibility southEastVisibility = MapTileVisibility.NotVisible;
            //     if (southEast != null)
            //     {
            //         southEastVisibility = southEast.visibility;
            //     }
            //     
            //     
            //     // MapTileInstance southWest = GameWorld.instance.FindMapTileInstanceAllowNull(
            //     //     myMapTileInstance.MapTileLocation.CreateAdjacentMapTileLocation(GameWorld.TILE_LOCATION.SOUTH_WEST));
            //
            //     MapTileInstanceView southWest = GameWorldView.instance.FindAdjacentMapTileInstanceViewAllowNull(
            //         this.mapTileLocation,
            //         MapTileLocation.TILE_LOCATION.SOUTH_WEST);
            //
            //     MapTileVisibility southWestVisibility = MapTileVisibility.NotVisible;
            //     if (southEast != null)
            //     {
            //         southWestVisibility = southWest.visibility;
            //     }
            //     
            //     // MapTileInstance northWest = GameWorld.instance.FindMapTileInstanceAllowNull(
            //     //     myMapTileInstance.MapTileLocation.CreateAdjacentMapTileLocation(GameWorld.TILE_LOCATION.NORTH_WEST));
            //
            //     MapTileInstanceView northWest = GameWorldView.instance.FindAdjacentMapTileInstanceViewAllowNull(
            //         this.mapTileLocation,
            //         MapTileLocation.TILE_LOCATION.NORTH_WEST);
            //
            //     MapTileVisibility northWestVisibility = MapTileVisibility.NotVisible;
            //     if (southEast != null)
            //     {
            //         northWestVisibility = northWest.visibility;
            //     }
            //     
            //     return FindMapTileShroudMapping(
            //         eastVisibility,
            //         southVisibility,
            //         westVisibility,
            //         northVisibility,
            //         northEastVisibility,
            //         southEastVisibility,
            //         southWestVisibility,
            //         northWestVisibility);
            //     
            //
            //
            // }



        internal void DrawVisbilityMask(GameTime gameTime, SpriteBatch spriteBatch)
        {
            float defaultScale = 1;
            
            XnaVector2 worldCoordinatesAsXnaVector2 =
                ConvertNumericsVector2ToXnaVector2(mapTileLocation.WorldCoordinatesAsVector2);


            if (mapTileLocation.XInWorldMapTileCoordinates == 24 && mapTileLocation.YInWorldMapTileCoordinates == 11)
            {
                int x = 3;
            }

            if (this.visibility == MapTileVisibility.Visible)
            {

                spriteBatch.Draw(visibleMask, worldCoordinatesAsXnaVector2, null, XnaColor.White, 0f,
                    middleOfSpriteInSpriteCoordinates, defaultScale, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1.0f);
            }
            else // if (this.visibility == MapTileVisibility.PartiallyVisible)
            {
                int index = DeterminePartiallyVisibleMaskTile();
                if (index >= 0)
                {
                    spriteBatch.Draw(partiallyVisibileMapTileMask.GetMask(index),
                        worldCoordinatesAsXnaVector2, null, XnaColor.White, 0f,
                        middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 1.0f);
                }
            }
        }

        // public bool ContainsPoint(int mouseX, int mouseY)
        // {
        //     int width = GameWorldView.MAP_TILE_WIDTH;
        //     int height = GameWorldView.MAP_TILE_HEIGHT;
        //
        //
        //     Point mapTileInstanceViewInWorldCoordinates =
        //         MapTileInstanceView.ConvertMapTileCoordinatesToWorldCoordinates(new Point(xInWorldMapTileCoordinates,
        //             yInWorldMapTileCoordinates));
        //
        //     int leftX = mapTileInstanceViewInWorldCoordinates.X - (width / 2);
        //     int topY = mapTileInstanceViewInWorldCoordinates.Y - (height / 2);
        //
        //     Rectangle boundRectangle = new Rectangle(leftX, topY, width, height);
        //     return boundRectangle.Contains(new Point(mouseX, mouseY));
        // }

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


        // public static XnaPoint ConvertMapTileCoordinatesToWorldCoordinates(XnaPoint pointInWorldMapSquareCoordinates)
        // {
        //
        //     int xInWorldCoordinates = (pointInWorldMapSquareCoordinates.X * GameWorldView.MAP_TILE_WIDTH) +
        //                               (GameWorldView.MAP_TILE_WIDTH / 2);
        //     int yInWorldCoordinates = pointInWorldMapSquareCoordinates.Y * GameWorldView.MAP_TILE_HEIGHT +
        //                               (GameWorldView.MAP_TILE_HEIGHT / 2);
        //
        //     return new XnaPoint(xInWorldCoordinates, yInWorldCoordinates);
        // }

    }
}
