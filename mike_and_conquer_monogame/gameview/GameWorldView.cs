using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using mike_and_conquer_monogame.main;
using mike_and_conquer_simulation.events;
using mike_and_conquer_simulation.main;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

using XnaPoint = Microsoft.Xna.Framework.Point;
using XnaVector2 = Microsoft.Xna.Framework.Vector2;



using BlendState = Microsoft.Xna.Framework.Graphics.BlendState;
using DepthStencilState = Microsoft.Xna.Framework.Graphics.DepthStencilState;
using RasterizerState = Microsoft.Xna.Framework.Graphics.RasterizerState;
using Effect = Microsoft.Xna.Framework.Graphics.Effect;
using SpriteSortMode = Microsoft.Xna.Framework.Graphics.SpriteSortMode;
using SamplerState = Microsoft.Xna.Framework.Graphics.SamplerState;


using Camera2D = mike_and_conquer_monogame.main.Camera2D;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;

using Viewport = Microsoft.Xna.Framework.Graphics.Viewport;
// using BarracksSidebarIconView = mike_and_conquer.gameview.sidebar.BarracksSidebarIconView;
// using MinigunnerSidebarIconView = mike_and_conquer.gameview.sidebar.MinigunnerSidebarIconView;

using RenderTarget2D = Microsoft.Xna.Framework.Graphics.RenderTarget2D;

using ShadowMapper = mike_and_conquer_monogame.gamesprite.ShadowMapper;


using ImmutablePalette = mike_and_conquer_monogame.openra.ImmutablePalette;

using Matrix = Microsoft.Xna.Framework.Matrix;

namespace mike_and_conquer_monogame.gameview
{
    public class GameWorldView
    {

        // public GameCursor gameCursor;


        public static int MAP_TILE_WIDTH = 24;
        public static int MAP_TILE_HEIGHT = 24;
        public static int MAP_TILE_WIDTH_IN_LEPTONS = 256;



        internal UnitSelectionBox unitSelectionBox;

        private ShadowMapper shadowMapper;
        // private MinigunnerSidebarIconView minigunnerSidebarIconView;
        // private BarracksSidebarIconView barracksSidebarIconView;
        //
        //
        // public BarracksSidebarIconView BarracksSidebarIconView
        // {
        //     get { return barracksSidebarIconView; }
        // }
        //
        // public MinigunnerSidebarIconView MinigunnerSidebarIconView
        // {
        //     get { return minigunnerSidebarIconView; }
        // }

        public float MapZoom
        {
            get { return mapViewportCamera.Zoom; }
            set { mapViewportCamera.Zoom = value; }
        }

        public int ScreenHeight
        {
            get { return defaultViewport.Height; }
        }

        public int ScreenWidth
        {
            get { return defaultViewport.Width; }
        }


        private int numColumns;

        public int NumColumns
        {
            get { return numColumns; }
            set { numColumns = value; }
        }

        public int numRows;

        public int NumRows
        {
            get { return numRows; }
            set { numRows = value; }
        }

        private Viewport defaultViewport;
        private Camera2D mapViewportCamera;
        private Viewport sidebarViewport; // TODO: Make this private again
        private Viewport mapViewport;
        private Camera2D renderTargetCamera;
        private Camera2D sidebarViewportCamera;

        private Texture2D sidebarBackgroundRectangle;
        private SpriteBatch spriteBatch;

        private Texture2D tshadow13MrfTexture;
        private Texture2D tshadow14MrfTexture;
        private Texture2D tshadow15MrfTexture;
        private Texture2D tshadow16MrfTexture;

        private RenderTarget2D mapTileRenderTarget;
        private RenderTarget2D shadowOnlyRenderTarget;

        private RenderTarget2D mapTileAndShadowsRenderTarget;

//        private RenderTarget2D mapTileShadowsAndTreesRenderTarget;
        private RenderTarget2D mapTileVisibilityRenderTarget;
        private RenderTarget2D unitsAndTerrainRenderTarget;

        // TODO make this private?
        public bool redrawBaseMapTiles;

        private Effect mapTilePaletteMapperEffect;
        private Effect mapTileShadowMapperEffect;

        private Texture2D paletteTexture;
        private Texture2D tunitsMrfTexture;

        private Texture2D mapBackgroundRectangle;

        private int borderSize = 0;

        private KeyboardState oldKeyboardState;

        private List<UnitView> unitViewList;
        //
        // private GDIBarracksView gdiBarracksView;
        //
        // private GDIConstructionYardView gdiConstructionYardView;


        private List<MapTileInstanceView> mapTileInstanceViewList;

        // private List<SandbagView> sandbagViewList;
        //
        // private List<NodTurretView> nodTurretViewList;
        // private List<Projectile120mmView> projectile120MmViewList;
        //

        public List<MapTileInstanceView> MapTileInstanceViewList
        {
            get { return mapTileInstanceViewList; }
        }


        public List<UnitView> UnitViewList
        {
            get { return unitViewList; }
        }


        // public List<MinigunnerView> NodMinigunnerViewList
        // {
        //     get { return nodMinigunnerViewList; }
        // }
        //
        // public List<SandbagView> SandbagViewList
        // {
        //     get { return sandbagViewList; }
        // }
        //
        // public List<NodTurretView> NodTurretViewList
        // {
        //     get { return nodTurretViewList; }
        // }
        //
        //
        // public GDIBarracksView GDIBarracksView
        // {
        //     get { return gdiBarracksView; }
        // }
        //
        // public GDIConstructionYardView GdiConstructionYardView
        // {
        //     get { return gdiConstructionYardView; }
        // }



        public List<TerrainView> terrainViewList;


        public static GameWorldView instance;

        // private BarracksPlacementIndicatorView barracksPlacementIndicatorView;
        //
        // public BarracksPlacementIndicator barracksBarracksPlacementIndicator;


        public GameWorldView()
        {
            mapTileInstanceViewList = new List<MapTileInstanceView>();

            unitViewList = new List<UnitView>();
            // nodMinigunnerViewList = new List<MinigunnerView>();
            //
            // sandbagViewList = new List<SandbagView>();
            // nodTurretViewList = new List<NodTurretView>();
            // projectile120MmViewList = new List<Projectile120mmView>();
            terrainViewList = new List<TerrainView>();
            unitSelectionBox = new UnitSelectionBox();

            shadowMapper = new ShadowMapper();
            redrawBaseMapTiles = true;
            instance = this;

        }


        internal void Draw(GameTime gameTime)
        {
            MapZoom = GameOptions.instance.MapZoomLevel;
            DrawMap(gameTime);


            // DrawSidebar(gameTime);
            // DrawGameCursor(gameTime);
        }


        public UnitView GetUnitViewById(int unitId)
        {

            foreach (UnitView unitView in this.unitViewList)
            {
                if (unitView.UnitId == unitId)
                {
                    return unitView;
                }
            }

            return null;

        }


        private void SetupSidebarViewportAndCamera()
        {
            sidebarViewport = new Viewport();
            sidebarViewport.X = mapViewport.Width + 2;
            sidebarViewport.Y = 0;
            sidebarViewport.Width = defaultViewport.Width - mapViewport.Width - 5;
            sidebarViewport.Height = defaultViewport.Height;
            sidebarViewport.MinDepth = 0;
            sidebarViewport.MaxDepth = 1;

            sidebarViewportCamera = new Camera2D(sidebarViewport);
            sidebarViewportCamera.Zoom = 3.0f;
            //            sidebarViewportCamera.Zoom = 1.5f;

            float scaledHalfViewportWidth = CalculateLeftmostScrollX(sidebarViewport, sidebarViewportCamera.Zoom, 0);
            float scaledHalfViewportHeight = CalculateTopmostScrollY(sidebarViewport, sidebarViewportCamera.Zoom, 0);

            sidebarViewportCamera.Location = new XnaVector2(scaledHalfViewportWidth, scaledHalfViewportHeight);
        }


        public float CalculateLeftmostScrollX()
        {
            int viewportWidth = mapViewport.Width;
            int halfViewportWidth = viewportWidth / 2;
            float scaledHalfViewportWidth = halfViewportWidth / mapViewportCamera.Zoom;
            return scaledHalfViewportWidth - borderSize;
        }

        // TODO Unduplicate this code?
        public float CalculateLeftmostScrollX(Viewport viewport, float zoom, int borderSize)
        {
            int viewportWidth = viewport.Width;
            int halfViewportWidth = viewportWidth / 2;
            float scaledHalfViewportWidth = halfViewportWidth / zoom;
            return scaledHalfViewportWidth - borderSize;
        }

        private float CalculateRightmostScrollX()
        {
            // int widthOfMapInWorldSpace = GameWorld.instance.gameMap.numColumns * GameWorld.MAP_TILE_WIDTH;
            int widthOfMapInWorldSpace = this.numColumns * GameWorldView.MAP_TILE_WIDTH;

            int viewportWidth = mapViewport.Width;
            int halfViewportWidth = viewportWidth / 2;

            float scaledHalfViewportWidth = halfViewportWidth / mapViewportCamera.Zoom;
            float amountToScrollHorizontally = widthOfMapInWorldSpace - scaledHalfViewportWidth;
            return amountToScrollHorizontally + borderSize;
        }

        public float CalculateTopmostScrollY()
        {
            int viewportHeight = mapViewport.Height;
            int halfViewportHeight = viewportHeight / 2;
            float scaledHalfViewportHeight = halfViewportHeight / mapViewportCamera.Zoom;
            return scaledHalfViewportHeight - borderSize;
        }

        // TODO Unduplicate this code?
        public float CalculateTopmostScrollY(Viewport viewport, float zoom, int borderSize)
        {
            int viewportHeight = viewport.Height;
            int halfViewportHeight = viewportHeight / 2;
            float scaledHalfViewportHeight = halfViewportHeight / zoom;
            return scaledHalfViewportHeight - borderSize;
        }



        private void SetupMapViewportAndCamera()
        {
            mapViewport = new Viewport();
            mapViewport.X = 0;
            mapViewport.Y = 0;
            mapViewport.Width = (int) (defaultViewport.Width * 0.8f);
            mapViewport.Height = defaultViewport.Height;
            mapViewport.MinDepth = 0;
            mapViewport.MaxDepth = 1;

            this.mapViewportCamera = new Camera2D(mapViewport);

            this.mapViewportCamera.Zoom = GameOptions.instance.MapZoomLevel;
            this.mapViewportCamera.Location =
                new XnaVector2(CalculateLeftmostScrollX(), CalculateTopmostScrollY());

            this.renderTargetCamera = new Camera2D(mapViewport);
            this.renderTargetCamera.Zoom = 1.0f;
            this.renderTargetCamera.Location =
                new XnaVector2(CalculateLeftmostScrollX(mapViewport, renderTargetCamera.Zoom, borderSize),
                    CalculateTopmostScrollY(mapViewport, renderTargetCamera.Zoom, borderSize));

        }

        private void DrawMap(GameTime gameTime)
        {

            MikeAndConquerGame.instance.GraphicsDevice.Viewport = mapViewport;


            UpdateMapTileRenderTarget(gameTime); // mapTileRenderTarget:  Just map tiles, as palette values
            UpdateShadowOnlyRenderTarget(gameTime); // shadowOnlyRenderTarget:  shadows of units and trees, as palette values
            UpdateMapTileAndShadowsRenderTarget(); // mapTileAndShadowsRenderTarget:  Drawing mapTileRenderTarget with shadowOnlyRenderTarget shadows mapped to it, as palette values
            UpdateMapTileVisibilityRenderTarget(gameTime); // mapTileVisibilityRenderTarget
            UpdateUnitsAndTerrainRenderTarget(gameTime); //    unitsAndTerrainRenderTarget:    draw mapTileAndShadowsRenderTarget, then units and terrain
            DrawAndApplyPaletteAndMapTileVisbility();

            //            DrawMrf16Texture();
            //            DrawVisibilityMaskAsTest();
            //            DrawShadowShapeAsTest();
        }


        // private void DrawGameCursor(GameTime gameTime)
        // {
        //     MikeAndConquerGame.instance.GraphicsDevice.Viewport = defaultViewport;
        //     const BlendState nullBlendState = null;
        //     const DepthStencilState nullDepthStencilState = null;
        //     const RasterizerState nullRasterizerState = null;
        //     const Effect nullEffect = null;
        //
        //     spriteBatch.Begin(
        //         SpriteSortMode.Deferred,
        //         nullBlendState,
        //         SamplerState.PointClamp,
        //         nullDepthStencilState,
        //         nullRasterizerState,
        //         nullEffect);
        //     gameCursor.Draw(gameTime, spriteBatch);
        //     spriteBatch.End();
        //
        //
        // }


        // private void DrawSidebar(GameTime gameTime)
        // {
        //     MikeAndConquerGame.instance.GraphicsDevice.Viewport = sidebarViewport;
        //
        //     const BlendState nullBlendState = null;
        //     const DepthStencilState nullDepthStencilState = null;
        //     const RasterizerState nullRasterizerState = null;
        //     const Effect nullEffect = null;
        //
        //     spriteBatch.Begin(
        //         SpriteSortMode.Deferred,
        //         nullBlendState,
        //         SamplerState.PointClamp,
        //         nullDepthStencilState,
        //         nullRasterizerState,
        //         nullEffect,
        //         sidebarViewportCamera.TransformMatrix);
        //
        //     spriteBatch.Draw(sidebarBackgroundRectangle,
        //         new Rectangle(0, 0, sidebarViewport.Width / 2, sidebarViewport.Height / 2), Color.White);
        //
        //     if (minigunnerSidebarIconView != null)
        //     {
        //         minigunnerSidebarIconView.Draw(gameTime, spriteBatch);
        //     }
        //
        //     if (barracksSidebarIconView != null)
        //     {
        //         barracksSidebarIconView.Draw(gameTime, spriteBatch);
        //     }
        //
        //     spriteBatch.End();
        // }


        private void UpdateMapTileRenderTarget(GameTime gameTime)
        {

            const BlendState nullBlendState = null;
            const DepthStencilState nullDepthStencilState = null;
            const RasterizerState nullRasterizerState = null;
            const Effect nullEffect = null;


            if (mapTileRenderTarget == null)
            {
                mapTileRenderTarget = new RenderTarget2D(MikeAndConquerGame.instance.GraphicsDevice,
                    mapViewport.Width, mapViewport.Height);

            }

            if (redrawBaseMapTiles)
            {
                redrawBaseMapTiles = false;
                MikeAndConquerGame.instance.GraphicsDevice.SetRenderTarget(mapTileRenderTarget);
                MikeAndConquerGame.instance.GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin(
                    SpriteSortMode.Immediate,
                    nullBlendState,
                    SamplerState.PointClamp,
                    nullDepthStencilState,
                    nullRasterizerState,
                    nullEffect,
                    renderTargetCamera.TransformMatrix);

                foreach (MapTileInstanceView mapTileInstanceView in GameWorldView.instance.MapTileInstanceViewList)
                {
                    mapTileInstanceView.Draw(gameTime, spriteBatch);
                }

                spriteBatch.End();

            }



        }

        private void UpdateShadowOnlyRenderTarget(GameTime gameTime)
        {

            const BlendState nullBlendState = null;
            const DepthStencilState nullDepthStencilState = null;
            const RasterizerState nullRasterizerState = null;
            const Effect nullEffect = null;

            if (shadowOnlyRenderTarget == null)
            {
                shadowOnlyRenderTarget = new RenderTarget2D(MikeAndConquerGame.instance.GraphicsDevice,
                    mapViewport.Width, mapViewport.Height);
            }

            MikeAndConquerGame.instance.GraphicsDevice.SetRenderTarget(shadowOnlyRenderTarget);

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                nullBlendState,
                SamplerState.PointClamp,
                nullDepthStencilState,
                nullRasterizerState,
                nullEffect,
                renderTargetCamera.TransformMatrix);

            // if (GameWorldView.instance.GDIBarracksView != null)
            // {
            //     GameWorldView.instance.GDIBarracksView.DrawShadowOnly(gameTime, spriteBatch);
            // }
            //
            // if (GameWorldView.instance.GdiConstructionYardView != null)
            // {
            //     GameWorldView.instance.GdiConstructionYardView.DrawShadowOnly(gameTime, spriteBatch);
            // }


            // foreach (SandbagView nextSandbagView in GameWorldView.instance.SandbagViewList)
            // {
            //     nextSandbagView.DrawShadowOnly(gameTime, spriteBatch);
            // }
            //
            // foreach (NodTurretView nextNodTurretView in GameWorldView.instance.NodTurretViewList)
            // {
            //     nextNodTurretView.DrawShadowOnly(gameTime, spriteBatch);
            // }
            //
            //
            foreach (UnitView unitView in GameWorldView.instance.UnitViewList)
            {
                unitView.DrawShadowOnly(gameTime, spriteBatch);
            }


            foreach (TerrainView nextTerrainView in GameWorldView.instance.terrainViewList)
            {
                nextTerrainView.DrawShadowOnly(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }

        private void UpdateMapTileAndShadowsRenderTarget()
        {

            const BlendState nullBlendState = null;
            const DepthStencilState nullDepthStencilState = null;
            const RasterizerState nullRasterizerState = null;
            const Effect nullEffect = null;


            if (mapTileAndShadowsRenderTarget == null)
            {
                mapTileAndShadowsRenderTarget = new RenderTarget2D(
                    MikeAndConquerGame.instance.GraphicsDevice,
                    mapViewport.Width, mapViewport.Height);
            }

            MikeAndConquerGame.instance.GraphicsDevice.SetRenderTarget(mapTileAndShadowsRenderTarget);

            MikeAndConquerGame.instance.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                nullBlendState,
                SamplerState.PointClamp,
                nullDepthStencilState,
                nullRasterizerState,
                nullEffect,
                renderTargetCamera.TransformMatrix);


            mapTileShadowMapperEffect.Parameters["ShadowTexture"].SetValue(shadowOnlyRenderTarget);
            mapTileShadowMapperEffect.Parameters["UnitMrfTexture"].SetValue(tunitsMrfTexture);
            mapTileShadowMapperEffect.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Draw(mapTileRenderTarget, new Rectangle(0, 0, mapViewport.Width, mapViewport.Height),
                Color.White);
            spriteBatch.End();
        }

        private void UpdateMapTileVisibilityRenderTarget(GameTime gameTime)
        {


            // Setting blendstate to Opaque because we want the 
            // transparent pixels (alpha = 0) to be preserved in
            // mapTileVisibilityRenderTarget, because the shader
            // uses alpha to determine whether to render the underlying tile
            // Without setting it to Opaque, alpha was getting set to 0 for 
            // the pertinent pixels
            BlendState blendState = BlendState.Opaque;
            const DepthStencilState nullDepthStencilState = null;
            const RasterizerState nullRasterizerState = null;
            const Effect nullEffect = null;


            if (mapTileVisibilityRenderTarget == null)
            {
                mapTileVisibilityRenderTarget = new RenderTarget2D(
                    MikeAndConquerGame.instance.GraphicsDevice,
                    mapViewport.Width, mapViewport.Height);
            }

            MikeAndConquerGame.instance.GraphicsDevice.SetRenderTarget(mapTileVisibilityRenderTarget);

            //            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                blendState,
                SamplerState.PointClamp,
                nullDepthStencilState,
                nullRasterizerState,
                nullEffect,
                renderTargetCamera.TransformMatrix);

            if (GameOptions.instance.DrawShroud)
            {
                foreach (MapTileInstanceView basicMapSquareView in GameWorldView.instance.MapTileInstanceViewList)
                {
                    basicMapSquareView.DrawVisbilityMask(gameTime, spriteBatch);
                }
            }

            spriteBatch.End();
        }

        private void UpdateUnitsAndTerrainRenderTarget(GameTime gameTime)
        {

            const BlendState nullBlendState = null;
            const DepthStencilState nullDepthStencilState = null;
            const RasterizerState nullRasterizerState = null;
            const Effect nullEffect = null;


            if (unitsAndTerrainRenderTarget == null)
            {
                unitsAndTerrainRenderTarget = new RenderTarget2D(
                    MikeAndConquerGame.instance.GraphicsDevice,
                    mapViewport.Width, mapViewport.Height);
            }

            MikeAndConquerGame.instance.GraphicsDevice.SetRenderTarget(unitsAndTerrainRenderTarget);
            MikeAndConquerGame.instance.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                nullBlendState,
                SamplerState.PointClamp,
                nullDepthStencilState,
                nullRasterizerState,
                nullEffect,
                renderTargetCamera.TransformMatrix);

            spriteBatch.Draw(mapTileAndShadowsRenderTarget, new Rectangle(0, 0, mapViewport.Width, mapViewport.Height),
                Color.White);


            // if (barracksPlacementIndicatorView != null)
            // {
            //     barracksPlacementIndicatorView.Draw(gameTime, spriteBatch);
            // }
            //
            //
            //
            // if (GameWorldView.instance.GDIBarracksView != null)
            // {
            //     GameWorldView.instance.GDIBarracksView.DrawNoShadow(gameTime, spriteBatch);
            // }
            //
            // if (GameWorldView.instance.GdiConstructionYardView != null)
            // {
            //     GameWorldView.instance.GdiConstructionYardView.DrawNoShadow(gameTime, spriteBatch);
            // }
            //
            // foreach (SandbagView nextSandbagView in GameWorldView.instance.SandbagViewList)
            // {
            //     nextSandbagView.DrawNoShadow(gameTime, spriteBatch);
            // }
            //
            // foreach (NodTurretView nodTurretView in GameWorldView.instance.NodTurretViewList)
            // {
            //     nodTurretView.DrawNoShadow(gameTime, spriteBatch);
            // }
            //
            // foreach (Projectile120mmView projectile120MmView in projectile120MmViewList)
            // {
            //     projectile120MmView.DrawNoShadow(gameTime, spriteBatch);
            // }
            //
            //


            foreach (UnitView unitView in this.UnitViewList)
            {
                unitView.DrawNoShadow(gameTime, spriteBatch);
            }


            foreach (TerrainView nextTerrainView in GameWorldView.instance.terrainViewList)
            {
                nextTerrainView.DrawNoShadow(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }

        private void DrawAndApplyPaletteAndMapTileVisbility()
        {
            const BlendState nullBlendState = null;
            const DepthStencilState nullDepthStencilState = null;
            const RasterizerState nullRasterizerState = null;
            const Effect nullEffect = null;


            MikeAndConquerGame.instance.GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                nullBlendState,
                SamplerState.PointClamp,
                nullDepthStencilState,
                nullRasterizerState,
                nullEffect,
                mapViewportCamera.TransformMatrix);


            mapTilePaletteMapperEffect.Parameters["PaletteTexture"].SetValue(paletteTexture);
            mapTilePaletteMapperEffect.Parameters["MapTileVisibilityTexture"].SetValue(mapTileVisibilityRenderTarget);
            mapTilePaletteMapperEffect.Parameters["DrawShroud"].SetValue(GameOptions.instance.DrawShroud);
            mapTilePaletteMapperEffect.Parameters["Value13MrfTexture"].SetValue(tshadow13MrfTexture);
            mapTilePaletteMapperEffect.Parameters["Value14MrfTexture"].SetValue(tshadow14MrfTexture);
            mapTilePaletteMapperEffect.Parameters["Value15MrfTexture"].SetValue(tshadow15MrfTexture);
            mapTilePaletteMapperEffect.Parameters["Value16MrfTexture"].SetValue(tshadow16MrfTexture);

            mapTilePaletteMapperEffect.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Draw(unitsAndTerrainRenderTarget, new Rectangle(0, 0, mapViewport.Width, mapViewport.Height),
                Color.White);


            spriteBatch.End();

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                nullBlendState,
                SamplerState.PointClamp,
                nullDepthStencilState,
                nullRasterizerState,
                nullEffect,
                mapViewportCamera.TransformMatrix);

            unitSelectionBox.Draw(spriteBatch);

            spriteBatch.End();

        }




        public void HandleReset()
        {
            unitViewList.Clear();
            mapTileInstanceViewList.Clear();
            terrainViewList.Clear();
            // nodMinigunnerViewList.Clear();
            // sandbagViewList.Clear();
            // nodTurretViewList.Clear();
            // projectile120MmViewList.Clear();
             // gdiConstructionYardView = null;
             // gdiBarracksView = null;
             // barracksSidebarIconView = null;
             // minigunnerSidebarIconView = null;
        }


        // public void AddMapTileInstanceView(MapTileInstance mapTileInstance)
        // {
        //     MapTileInstanceView mapTileInstanceView = new MapTileInstanceView(mapTileInstance);
        //     this.mapTileInstanceViewList.Add(mapTileInstanceView);
        // }
        //
        // public void AddTerrainItemView(TerrainItem terrainItem)
        // {
        //     TerrainView terrainView = new TerrainView(terrainItem);
        //     this.terrainViewList.Add(terrainView);
        // }


        // public void AddGDIMinigunnerView(Minigunner newMinigunner)
        // {
        //     MinigunnerView newMinigunnerView = new GdiMinigunnerView(newMinigunner);
        //     gdiMinigunnerViewList.Add(newMinigunnerView);
        //
        // }
        //
        // public void AddSandbagView(Sandbag newSandbag)
        // {
        //     SandbagView newSandbagView = new SandbagView(newSandbag);
        //     sandbagViewList.Add(newSandbagView);
        // }


        // public void AddNodTurretView(NodTurret nodTurret)
        // {
        //     NodTurretView newNodTurretView = new NodTurretView(nodTurret);
        //     nodTurretViewList.Add(newNodTurretView);
        // }
        //
        // public void AddGDIBarracksView(GDIBarracks gdiBarracks)
        // {
        //     gdiBarracksView = new GDIBarracksView(gdiBarracks);
        //     minigunnerSidebarIconView = new MinigunnerSidebarIconView(new Point(112, 24));
        // }
        //
        // public void AddGDIConstructionYardView(GDIConstructionYard gdiConstructionYard)
        // {
        //     gdiConstructionYardView = new GDIConstructionYardView(gdiConstructionYard);
        //     barracksSidebarIconView = new BarracksSidebarIconView(new Point(32, 24));
        // }



        // public void AddMCVView(MCV mcv)
        // {
        //     mcvView = new MCVView(mcv);
        // }
        //
        //
        // public void AddNodMinigunnerView(Minigunner newMinigunner)
        // {
        //     MinigunnerView newMinigunnerView = new NodMinigunnerView(newMinigunner);
        //     nodMinigunnerViewList.Add(newMinigunnerView);
        //
        // }


        // public MapTileInstanceView FindMapSquareView(int xWorldCoordinate, int yWorldCoordinate)
        // {
        //
        //     foreach (MapTileInstanceView nextBasicMapSquareView in this.mapTileInstanceViewList)
        //     {
        //         MapTileInstance mapTileInstance = nextBasicMapSquareView.myMapTileInstance;
        //         if (mapTileInstance.ContainsPoint(new Point(xWorldCoordinate, yWorldCoordinate)))
        //         {
        //             return nextBasicMapSquareView;
        //         }
        //     }
        //     throw new Exception("Unable to find MapTileInstance at coordinates, x:" + xWorldCoordinate + ", y:" + yWorldCoordinate);
        //
        // }
        //
        //
        // private void CreateBasicMapSquareViews()
        // {
        //     foreach (MapTileInstance mapTileInstance in GameWorld.instance.gameMap.MapTileInstanceList)
        //     {
        //         AddMapTileInstanceView(mapTileInstance);
        //     }
        // }
        //
        // private void CreateTerrainItemViews()
        // {
        //     foreach (TerrainItem terrainItem in GameWorld.instance.terrainItemList)
        //     {
        //         AddTerrainItemView(terrainItem);
        //     }
        // }


        public void LoadContent()
        {

            // CreateBasicMapSquareViews();
//            CreateTerrainItemViews();

            spriteBatch = new SpriteBatch(MikeAndConquerGame.instance.GraphicsDevice);
            // gameCursor = new GameCursor(1, 1);

            this.defaultViewport = MikeAndConquerGame.instance.GraphicsDevice.Viewport;
            SetupMapViewportAndCamera();
            SetupSidebarViewportAndCamera();

            sidebarBackgroundRectangle = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, 1, 1);
            sidebarBackgroundRectangle.SetData(new[] {Color.LightSkyBlue});

            mapBackgroundRectangle = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, 1, 1);
            mapBackgroundRectangle.SetData(new[] {Color.MediumSeaGreen});

            this.mapTilePaletteMapperEffect =
                MikeAndConquerGame.instance.Content.Load<Effect>("Effects\\MapTilePaletteMapperEffect");
            this.mapTileShadowMapperEffect =
                MikeAndConquerGame.instance.Content.Load<Effect>("Effects\\MapTileShadowMapperEffect");

            this.paletteTexture = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, 256, 1);
            int[] remap = { };
            ImmutablePalette palette =
                new ImmutablePalette(MikeAndConquerGame.CONTENT_DIRECTORY_PREFIX + "temperat.pal", remap);
            int numPixels = 256;
            Color[] texturePixelData = new Color[numPixels];

            for (int i = 0; i < numPixels; i++)
            {
                uint mappedColor = palette[i];

                // NOTE:  This tweak of this red color was needed to make
                // rendering of the Nod Turrets show the right color
                // The red pixel value was off by 4.  Not sure why, palette seems to be getting read correctly
                // Tweaking the color here on the fly since the palette is Immutable
                if (i == 0x7e && mappedColor == 0xffdc1408)
                {
                    mappedColor = 0xffdc1008;
                }

                System.Drawing.Color systemColor = System.Drawing.Color.FromArgb((int) mappedColor);

                byte alpha = 255;
                Color xnaColor = new Color(systemColor.R, systemColor.G, systemColor.B, alpha);
                texturePixelData[i] = xnaColor;
            }

            paletteTexture.SetData(texturePixelData);


            LoadTUnitsMrfTexture();
            LoadTShadow13MrfTexture();
            LoadTShadow14MrfTexture();
            LoadTShadow15MrfTexture();
            LoadTShadow16MrfTexture();

//            LoadTmpFile(BarracksPlacementIndicatorView.FILE_NAME);
        }

        private void LoadTUnitsMrfTexture()
        {

            int numPixels = 256;
            tunitsMrfTexture = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, 256, 1);
            Color[] texturePixelData = new Color[numPixels];

            for (int i = 0; i < 256; i++)
            {
                byte alpha = 255;
                int mrfValue = this.shadowMapper.MapUnitsShadowPaletteIndex(i);
                byte byteMrfValue = (byte) mrfValue;
                Color xnaColor = new Color(byteMrfValue, (byte) 0, (byte) 0, alpha);
                texturePixelData[i] = xnaColor;
            }

            tunitsMrfTexture.SetData(texturePixelData);
        }

        private void LoadTShadow13MrfTexture()
        {
            int numPixels = 256;
            tshadow13MrfTexture = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, 256, 1);
            Color[] texturePixelData = new Color[numPixels];

            for (int i = 0; i < 256; i++)
            {
                byte alpha = 255;
                int mrfValue = this.shadowMapper.MapMapTile13PaletteIndex(i);
                byte byteMrfValue = (byte) mrfValue;
                Color xnaColor = new Color(byteMrfValue, (byte) 0, (byte) 0, alpha);
                texturePixelData[i] = xnaColor;
            }

            tshadow13MrfTexture.SetData(texturePixelData);
        }

        private void LoadTShadow14MrfTexture()
        {
            int numPixels = 256;
            tshadow14MrfTexture = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, 256, 1);
            Color[] texturePixelData = new Color[numPixels];

            for (int i = 0; i < 256; i++)
            {
                byte alpha = 255;
                int mrfValue = this.shadowMapper.MapMapTile14PaletteIndex(i);
                byte byteMrfValue = (byte) mrfValue;
                Color xnaColor = new Color(byteMrfValue, (byte) 0, (byte) 0, alpha);
                texturePixelData[i] = xnaColor;
            }

            tshadow14MrfTexture.SetData(texturePixelData);
        }

        private void LoadTShadow15MrfTexture()
        {
            int numPixels = 256;
            tshadow15MrfTexture = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, 256, 1);
            Color[] texturePixelData = new Color[numPixels];

            for (int i = 0; i < 256; i++)
            {
                byte alpha = 255;
                int mrfValue = this.shadowMapper.MapMapTile15PaletteIndex(i);
                byte byteMrfValue = (byte) mrfValue;
                Color xnaColor = new Color(byteMrfValue, (byte) 0, (byte) 0, alpha);
                texturePixelData[i] = xnaColor;
            }

            tshadow15MrfTexture.SetData(texturePixelData);
        }


        private void LoadTShadow16MrfTexture()
        {
            int numPixels = 256;
            tshadow16MrfTexture = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, 256, 1);
            Color[] texturePixelData = new Color[numPixels];

            for (int i = 0; i < 256; i++)
            {
                byte alpha = 255;
                int mrfValue = this.shadowMapper.MapMapTile16PaletteIndex(i);
                byte byteMrfValue = (byte) mrfValue;
                Color xnaColor = new Color(byteMrfValue, (byte) 0, (byte) 0, alpha);
                texturePixelData[i] = xnaColor;
            }

            tshadow16MrfTexture.SetData(texturePixelData);
        }

        private float CalculateBottommostScrollY()
        {
            // int heightOfMapInWorldSpace = GameWorld.instance.gameMap.numRows * GameWorld.MAP_TILE_HEIGHT;
            int heightOfMapInWorldSpace = this.numRows * GameWorldView.MAP_TILE_HEIGHT;
            int viewportHeight = mapViewport.Height;
            int halfViewportHeight = viewportHeight / 2;
            float scaledHalfViewportHeight = halfViewportHeight / mapViewportCamera.Zoom;
            float amountToScrollVertically = heightOfMapInWorldSpace - scaledHalfViewportHeight;
            return amountToScrollVertically + borderSize;
        }


        private void SnapMapCameraToBounds()
        {
            float newX = this.mapViewportCamera.Location.X;
            float newY = this.mapViewportCamera.Location.Y;

            // TODO:  Consider if we store these as class variables
            // and only recalculate when the zoom changes
            float rightMostScrollX = CalculateRightmostScrollX();
            float leftMostScrollX = CalculateLeftmostScrollX();
            float topmostScrollY = CalculateTopmostScrollY();
            float bottommostScrollY = CalculateBottommostScrollY();
            if (newX > rightMostScrollX)
            {
                newX = rightMostScrollX;
            }

            if (newY > bottommostScrollY)
            {
                newY = bottommostScrollY;
            }

            // Check for leftmost and topmost last, which makes it snap to top left corner
            // if zoom is such that entire map fits on current screen
            if (newX < leftMostScrollX)
            {
                newX = leftMostScrollX;
            }

            if (newY < topmostScrollY)
            {
                newY = topmostScrollY;
            }

            this.mapViewportCamera.Location = new XnaVector2(newX, newY);

        }


        public void Update(GameTime gameTime, KeyboardState newKeyboardState)
        {
            if (newKeyboardState.IsKeyDown(Keys.B))
            {
                borderSize = 1;
            }

            if (newKeyboardState.IsKeyDown(Keys.N))
            {
                borderSize = 0;
            }

            if (newKeyboardState.IsKeyDown(Keys.I))
            {
                this.mapViewportCamera.Location = new XnaVector2(CalculateLeftmostScrollX(), CalculateTopmostScrollY());
            }

            if (newKeyboardState.IsKeyDown(Keys.P))
            {
                this.mapViewportCamera.Location =
                    new XnaVector2(CalculateRightmostScrollX(), CalculateTopmostScrollY());
            }

            if (newKeyboardState.IsKeyDown(Keys.M))
            {
                this.mapViewportCamera.Location =
                    new XnaVector2(CalculateLeftmostScrollX(), CalculateBottommostScrollY());
            }

            if (newKeyboardState.IsKeyDown(Keys.OemPeriod))
            {
                this.mapViewportCamera.Location =
                    new XnaVector2(CalculateRightmostScrollX(), CalculateBottommostScrollY());
            }

            if (!oldKeyboardState.IsKeyDown(Keys.Y) && newKeyboardState.IsKeyDown(Keys.Y))
            {
                GameOptions.instance.ToggleDrawTerrainBorder();
                redrawBaseMapTiles = true;
            }

            if (!oldKeyboardState.IsKeyDown(Keys.H) && newKeyboardState.IsKeyDown(Keys.H))
            {
                GameOptions.instance.ToggleDrawBlockingTerrainBorder();
                redrawBaseMapTiles = true;
            }

            if (!oldKeyboardState.IsKeyDown(Keys.S) && newKeyboardState.IsKeyDown(Keys.S))
            {
                GameOptions.instance.ToggleDrawShroud();
            }


            // if (!oldKeyboardState.IsKeyDown(Keys.Q) && newKeyboardState.IsKeyDown(Keys.Q))
            // {
            //     GameOptions.instance.CurrentGameSpeedDivisor() -= 10;
            //     if (GameOptions.instance.CurrentGameSpeedDivisor() < 1)
            //     {
            //         GameOptions.instance.CurrentGameSpeedDivisor() = 1;
            //     }
            // }
            //
            // if (!oldKeyboardState.IsKeyDown(Keys.W) && newKeyboardState.IsKeyDown(Keys.W))
            // {
            //     GameOptions.instance.GameSpeedDelayDivisor += 10;
            // }


            // this.mapViewportCamera.Rotation = testRotation;
            //                                    testRotation += 0.01f;

            // KeyboardState newKeyboardState = Keyboard.GetState();  // get the newest state

            int originalX = (int) this.mapViewportCamera.Location.X;
            int originalY = (int) this.mapViewportCamera.Location.Y;

            HandleMapScrolling(originalY, originalX, newKeyboardState);
            oldKeyboardState = newKeyboardState;

            MikeAndConquerGame.instance.SwitchToNewGameStateViewIfNeeded();
            // gameCursor.Update(gameTime);

            // if (GameWorld.instance.GDIBarracks != null)
            // {
            //     minigunnerSidebarIconView.Update(gameTime);
            // }
            //
            // if (GameWorld.instance.GDIConstructionYard != null)
            // {
            //     barracksSidebarIconView.Update(gameTime);
            // }
            //
            // foreach (NodTurretView nodTurretView in nodTurretViewList)
            // {
            //     nodTurretView.Update(gameTime);
            // }
            
            foreach (UnitView unitView in unitViewList)
            {
                unitView.Update(gameTime);
            }
            
            // if (mcvView != null)
            // {
            //     mcvView.Update(gameTime);
            // }
        }


        private void HandleMapScrolling(int originalY, int originalX, KeyboardState newKeyboardState)
        {
            int scrollAmount = 10;
            int mouseScrollThreshold = 30;

            Microsoft.Xna.Framework.Input.MouseState mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();

            float zoomChangeAmount = 0.2f;
            if (mouseState.Position.X > defaultViewport.Width - mouseScrollThreshold)
            {
                int newX = (int) (this.mapViewportCamera.Location.X + 2);
                this.mapViewportCamera.Location = new XnaVector2(newX, originalY);
            }
            else if (mouseState.Position.X < mouseScrollThreshold)
            {
                int newX = (int) (this.mapViewportCamera.Location.X - 2);
                this.mapViewportCamera.Location = new XnaVector2(newX, originalY);
            }
            else if (mouseState.Position.Y > defaultViewport.Height - mouseScrollThreshold)
            {
                int newY = (int) (this.mapViewportCamera.Location.Y + 2);
                this.mapViewportCamera.Location = new XnaVector2(originalX, newY);
            }
            else if (mouseState.Position.Y < mouseScrollThreshold)
            {
                int newY = (int) (this.mapViewportCamera.Location.Y - 2);
                this.mapViewportCamera.Location = new XnaVector2(originalX, newY);
            }

            else if (oldKeyboardState.IsKeyUp(Keys.Right) && newKeyboardState.IsKeyDown(Keys.Right))
            {
                int newX = (int) (this.mapViewportCamera.Location.X + scrollAmount);
                this.mapViewportCamera.Location = new XnaVector2(newX, originalY);
            }
            else if (oldKeyboardState.IsKeyUp(Keys.Left) && newKeyboardState.IsKeyDown(Keys.Left))
            {
                int newX = (int) (this.mapViewportCamera.Location.X - scrollAmount);
                this.mapViewportCamera.Location = new XnaVector2(newX, originalY);
            }
            else if (oldKeyboardState.IsKeyUp(Keys.Down) && newKeyboardState.IsKeyDown(Keys.Down))
            {
                int newY = (int) (this.mapViewportCamera.Location.Y + scrollAmount);
                this.mapViewportCamera.Location = new XnaVector2(originalX, newY);
            }
            else if (oldKeyboardState.IsKeyUp(Keys.Up) && newKeyboardState.IsKeyDown(Keys.Up))
            {
                int newY = (int) (this.mapViewportCamera.Location.Y - scrollAmount);
                this.mapViewportCamera.Location = new XnaVector2(originalX, newY);
            }
            else if (oldKeyboardState.IsKeyUp(Keys.OemPlus) && newKeyboardState.IsKeyDown(Keys.OemPlus))
            {
                // TODO:  This is bogus having to update both the option and the actual camera zoom to the new zoom level
                // Should be hidden behind some kind setter method?
                float newZoom = GameOptions.instance.MapZoomLevel + zoomChangeAmount;
                GameOptions.instance.MapZoomLevel = newZoom;
                this.mapViewportCamera.Zoom = GameOptions.instance.MapZoomLevel;
            }
            else if (oldKeyboardState.IsKeyUp(Keys.OemMinus) && newKeyboardState.IsKeyDown(Keys.OemMinus))
            {
                float newZoom = GameOptions.instance.MapZoomLevel - zoomChangeAmount;
                GameOptions.instance.MapZoomLevel = newZoom;
                this.mapViewportCamera.Zoom = GameOptions.instance.MapZoomLevel;

            }

            SnapMapCameraToBounds();
        }

        private void DrawShadowShapeAsTest()
        {
            const BlendState nullBlendState = null;
            const DepthStencilState nullDepthStencilState = null;
            const RasterizerState nullRasterizerState = null;
            const Effect nullEffect = null;


            MikeAndConquerGame.instance.GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                nullBlendState,
                SamplerState.PointClamp,
                nullDepthStencilState,
                nullRasterizerState,
                nullEffect,
                mapViewportCamera.TransformMatrix);


            //            spriteBatch.Draw(PartiallyVisibileMapTileMask.PartiallyVisibleMask, new Rectangle(0, 0, 24,24),
            //                Color.White);
            spriteBatch.End();
        }


        private void DrawMrf16Texture()
        {
            const BlendState nullBlendState = null;
            const DepthStencilState nullDepthStencilState = null;
            const RasterizerState nullRasterizerState = null;
            const Effect nullEffect = null;

            MikeAndConquerGame.instance.GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                nullBlendState,
                SamplerState.PointClamp,
                nullDepthStencilState,
                nullRasterizerState,
                nullEffect,
                mapViewportCamera.TransformMatrix);

            spriteBatch.Draw(tshadow16MrfTexture, new XnaVector2(0, 0), Color.White);
            spriteBatch.End();
        }


        private void DrawVisibilityMaskAsTest()
        {
            const BlendState nullBlendState = null;
            const DepthStencilState nullDepthStencilState = null;
            const RasterizerState nullRasterizerState = null;
            const Effect nullEffect = null;


            MikeAndConquerGame.instance.GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                nullBlendState,
                SamplerState.PointClamp,
                nullDepthStencilState,
                nullRasterizerState,
                nullEffect,
                mapViewportCamera.TransformMatrix);


            spriteBatch.Draw(mapTileVisibilityRenderTarget, new Rectangle(0, 0, mapViewport.Width, mapViewport.Height),
                Color.White);
            spriteBatch.End();
        }

        public XnaVector2 ConvertWorldCoordinatesToScreenCoordinates(XnaVector2 positionInWorldCoordinates)
        {
            return XnaVector2.Transform(positionInWorldCoordinates, mapViewportCamera.TransformMatrix);
        }

        public XnaVector2 ConvertWorldCoordinatesToScreenCoordinatesForSidebar(XnaVector2 positionInWorldCoordinates)
        {
            // TODO:  Consider if above code could better be done with call to Viewport.Project()
            // OR, should this be done by the Camera class?
            XnaVector2 positionInCameraViewportCoordinates = XnaVector2.Transform(positionInWorldCoordinates,
                sidebarViewportCamera.TransformMatrix);
            positionInCameraViewportCoordinates.X += sidebarViewport.X;
            return positionInCameraViewportCoordinates;
        }

        // public XnaPoint ConvertScreenLocationToWorldLocation(XnaPoint screenLocation)
        // {
        //     XnaVector2 screenLocationAsPoint = PointUtil.ConvertPointToXnaVector2(screenLocation);
        //     XnaVector2 resultXnaVector2 =  XnaVector2.Transform(screenLocationAsPoint, Matrix.Invert(mapViewportCamera.TransformMatrix));
        //     return PointUtil.ConvertXnaVector2ToPoint(resultXnaVector2);
        // }



        public XnaVector2 ConvertScreenLocationToWorldLocation(XnaVector2 screenLocation)
        {
            return XnaVector2.Transform(screenLocation, Matrix.Invert(mapViewportCamera.TransformMatrix));
        }

        public XnaVector2 ConvertScreenLocationToSidebarLocation(XnaVector2 screenLocation)
        {
            screenLocation.X = screenLocation.X - sidebarViewport.X;
            XnaVector2 result =
                XnaVector2.Transform(screenLocation, Matrix.Invert(sidebarViewportCamera.TransformMatrix));
            return result;
        }


        // public MapTileInstanceView FindMapTileInstanceAllowNull(MapTileLocation mapTileLocation)
        // {
        //
        //     foreach (MapTileInstanceView nextBasicMapSquare in mapTileInstanceViewList)
        //     {
        //         if (nextBasicMapSquare.ContainsPoint(mapTileLocation.WorldCoordinatesAsPoint))
        //         {
        //             return nextBasicMapSquare;
        //         }
        //     }
        //
        //     return null;
        //
        // }




        // public void Notify_PlacingBarracks()
        // {
        //     if (barracksPlacementIndicatorView == null)
        //     {
        //         barracksBarracksPlacementIndicator = new BarracksPlacementIndicator(
        //             MapTileLocation.CreateFromWorldCoordinatesInXnaVector2(GameWorld.instance.GDIConstructionYard.MapTileLocation.WorldCoordinatesAsXnaVector2));
        //
        //         barracksPlacementIndicatorView = new BarracksPlacementIndicatorView(barracksBarracksPlacementIndicator);
        //     }
        //
        // }
        //
        // public void Notify_PlacingBarracksWithMouseOverMap(Point mouseLocationInScreenCoordinates)
        // {
        //     Point mouseLocationWordCoordinates =
        //         ConvertScreenLocationToWorldLocation(mouseLocationInScreenCoordinates);
        //
        //     barracksBarracksPlacementIndicator.UpdateLocationInWorldCoordinates(mouseLocationWordCoordinates);
        // }

        // public void Notify_DonePlacingBarracks()
        // {
        //     barracksPlacementIndicatorView = null;
        //     barracksBarracksPlacementIndicator = null;
        // }
        //
        // public void AddProjectile120mmView(Projectile120mm projectile120Mm)
        // {
        //     projectile120MmViewList.Add(new Projectile120mmView(projectile120Mm));
        // }

        // public void RemoveProjectile120mmView(int id)
        // {
        //     foreach (Projectile120mmView view in projectile120MmViewList)
        //     {
        //         if (view.Id == id)
        //         {
        //             projectile120MmViewList.Remove(view);
        //             return;
        //         }
        //     }
        //
        // }
        public void AddMapTileInstanceView(int xInWorldMapTileCoordinates, int yInWorldMapTileCoordinates,
            byte imageIndex, string textureKey, bool isBlockingTerrain,
            MapTileInstanceView.MapTileVisibility visibilityEnumValue)
        {
            MapTileInstanceView view = new MapTileInstanceView(
                xInWorldMapTileCoordinates,
                yInWorldMapTileCoordinates,
                imageIndex,
                textureKey,
                isBlockingTerrain,
                visibilityEnumValue);

            mapTileInstanceViewList.Add(view);
        }

        public void AddTerrainItemView(int xInWorldMapTileCoordinates, int yInWorldMapTileCoordinates,
            string terrainItemType)
        {
            TerrainView terrainView =
                new TerrainView(xInWorldMapTileCoordinates, yInWorldMapTileCoordinates, terrainItemType);
            terrainViewList.Add(terrainView);

        }

        //        private List<UnitView> unitViewList;

        public void AddMinigunnerView(int id, int x, int y)
        {
            GdiMinigunnerView view = new GdiMinigunnerView(id, x, y);
            unitViewList.Add(view);
        }

        public void AddMCVView(int id, int x, int y)
        {
            MCVView view = new MCVView(id, x, y);
            unitViewList.Add(view);
        }


        public void AddJeepView(int id, int x, int y)
        {
            unitViewList.Add(new JeepView(id, x, y));
        }



        public static XnaPoint ConvertMapTileCoordinatesToWorldCoordinates(XnaPoint pointInWorldMapSquareCoordinates)
        {

            int xInWorldCoordinates = (pointInWorldMapSquareCoordinates.X * GameWorldView.MAP_TILE_WIDTH) +
                                      (GameWorldView.MAP_TILE_WIDTH / 2);
            int yInWorldCoordinates = pointInWorldMapSquareCoordinates.Y * GameWorldView.MAP_TILE_HEIGHT +
                                      (GameWorldView.MAP_TILE_HEIGHT / 2);

            return new XnaPoint(xInWorldCoordinates, yInWorldCoordinates);
        }


        public MapTileInstanceView FindMapTileInstanceView(int mouseX, int mouseY)
        {
            foreach (MapTileInstanceView mapTileInstanceView in this.MapTileInstanceViewList)
            {
                if (mapTileInstanceView.ContainsPoint(mouseX, mouseY))
                {
                    return mapTileInstanceView;
                }
            }
            throw new Exception("Unable to find MapTileInstance at coordinates, x:" + mouseX + ", y:" + mouseY);

        }

        private MapTileInstanceView FindMapTileInstanceViewFromWorldMapTileCoordinates(int xInWorldMapTileCoordinates,
            int yInWorldMapTileCoordinates)
        {
            GameWorldLocationBuilder builder = new GameWorldLocationBuilder();
            GameWorldLocation location = builder
                .WorldMapTileCoordinatesX(xInWorldMapTileCoordinates)
                .WorldMapTileCoordinatesY(yInWorldMapTileCoordinates)
                .build();

            return FindMapTileInstanceView((int) location.X, (int) location.Y);
        }


        private UnitView FindUnitById(int unitId)
        {
            UnitView foundUnitView = null;
            foreach (UnitView unitView in unitViewList)
            {
                if (unitView.UnitId == unitId)
                {
                    foundUnitView = unitView;
                    break;
                }
            }

            return foundUnitView;

        }

        public void CreatePlannedPathView(int unitId, List<PathStep> pathStepList)
        {
            UnitView unitView = FindUnitById(unitId);
            unitView.CreatePlannedPathView(pathStepList);
        }

        public void RemovePlannedStepView(int unitId, PathStep pathStep)
        {
            UnitView unitView = FindUnitById(unitId);
            unitView.RemovePlannedPathStepView(pathStep);
        }


        public bool IsAUnitViewSelected()
        {
            foreach (UnitView unitView in unitViewList)
            {
                if (unitView.Selected)
                {
                    return true;
                }
            }
            return false;
        }




    }
}
