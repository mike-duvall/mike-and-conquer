


using UnitSprite = mike_and_conquer_monogame.gamesprite.UnitSprite;
using ShpFileColorMapper = mike_and_conquer_monogame.gamesprite.ShpFileColorMapper;
using GdiShpFileColorMapper = mike_and_conquer_monogame.gamesprite.GdiShpFileColorMapper;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

using SystemDrawingPoint = System.Drawing.Point;
using XnaPoint = Microsoft.Xna.Framework.Point;


using AnimationSequence = mike_and_conquer_monogame.util.AnimationSequence;

using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

using SimulationMapTileLocation = mike_and_conquer_simulation.gameworld.MapTileLocation;

namespace mike_and_conquer_monogame.gameview
{
    public class GDIConstructionYardView
    {

        // TODO:  Consider something other than UnitSprite in future
        private UnitSprite unitSprite;

        public int XInWorldCoordinates { get; set; }
        public int YInWorldCoordinates { get; set; }

        public bool IsBuildingBarracks { get; set; }
        public int PercentBarracksBuildComplete { get; set; }

        public bool IsBarracksReadyToPlace { get; set; }


        public const string SPRITE_KEY = "ConstructionYard";

        public const string SHP_FILE_NAME = "Shp\\fact.shp";
        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();


        public GDIConstructionYardView(int unitId, int xInWorldCoordinates, int yInWorldCoordinates)
        {
            this.XInWorldCoordinates = xInWorldCoordinates;
            this.YInWorldCoordinates = yInWorldCoordinates;

            this.unitSprite = new UnitSprite(SPRITE_KEY);
            this.unitSprite.drawShadow = true;
            this.unitSprite.drawBoundingRectangle = false;
            this.unitSprite.middleOfSpriteInSpriteCoordinates.Y += (GameWorldView.MAP_TILE_HEIGHT / 2);

            SetupAnimations();
        }


        private void SetupAnimations()
        {
            AnimationSequence animationSequence = new AnimationSequence(1);
            animationSequence.AddFrame(0);
            unitSprite.AddAnimationSequence(0, animationSequence);
        }


        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            Vector2 worldCoordinatesAsVector2 = new Vector2(
                XInWorldCoordinates,
                YInWorldCoordinates);


            unitSprite.Draw(gameTime, spriteBatch, worldCoordinatesAsVector2,
                SpriteSortLayers.BUILDING_DEPTH);
        }

        internal void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 worldCoordinatesAsVector2 = new Vector2(
                XInWorldCoordinates,
                YInWorldCoordinates);

            unitSprite.DrawNoShadow(gameTime, spriteBatch, worldCoordinatesAsVector2, SpriteSortLayers.UNIT_DEPTH);
        }


        internal void DrawShadowOnly(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 worldCoordinatesAsVector2 = new Vector2(
                XInWorldCoordinates,
                YInWorldCoordinates);

            unitSprite.DrawShadowOnly(gameTime, spriteBatch, worldCoordinatesAsVector2, SpriteSortLayers.UNIT_DEPTH);
        }

        // internal void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch)
        // {
        //     if (myMCV.health <= 0)
        //     {
        //         return;
        //     }
        //
        //     mcvSelectionBox.position = new Vector2(myMCV.positionInWorldCoordinates.X, myMCV.positionInWorldCoordinates.Y);
        //
        //     unitSprite.DrawNoShadow(gameTime, spriteBatch, myMCV.positionInWorldCoordinates, SpriteSortLayers.UNIT_DEPTH);
        //
        //     if (myMCV.selected)
        //     {
        //         mcvSelectionBox.Draw(gameTime, spriteBatch);
        //     }
        //
        // }


        // public bool ContainsPoint(SimulationMapTileLocation worldCoordinatesAsPoint)
        // {
        //     throw new System.NotImplementedException();
        // }


        public bool ContainsPoint(SimulationMapTileLocation simulationMapTileLocation)
        {
            int width = 72;
            int height = 48;

            // int leftX = (int)mapTileLocation.WorldCoordinatesAsVector2.X - (width / 2);
            // int topY = (int)mapTileLocation.WorldCoordinatesAsVector2.Y - (height / 2);

            SystemDrawingPoint locationAsPoint = simulationMapTileLocation.WorldCoordinatesAsPoint;

            // int leftX = locationAsPoint.X - (width / 2);
            // int topY = locationAsPoint.Y - (height / 2);
            int leftX = this.XInWorldCoordinates - (width / 2);
            int topY = this.YInWorldCoordinates - (height / 2);

            XnaPoint xnaPoint = new XnaPoint();
            xnaPoint.X = locationAsPoint.X;
            xnaPoint.Y = locationAsPoint.Y;


            Rectangle boundRectangle = new Rectangle(leftX, topY, width, height);
            return boundRectangle.Contains(xnaPoint);


        }



    }
}
