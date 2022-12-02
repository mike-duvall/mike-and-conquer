

using UnitSprite = mike_and_conquer_monogame.gamesprite.UnitSprite;
using ShpFileColorMapper = mike_and_conquer_monogame.gamesprite.ShpFileColorMapper;
using GdiShpFileColorMapper = mike_and_conquer_monogame.gamesprite.GdiShpFileColorMapper;
using Vector2 = Microsoft.Xna.Framework.Vector2;

using AnimationSequence = mike_and_conquer_monogame.util.AnimationSequence;

using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;



namespace mike_and_conquer_monogame.gameview
{
    public class GDIConstructionYardView
    {

        // TODO:  Consider something other than UnitSprite in future
        private UnitSprite unitSprite;

        // private GDIConstructionYard myGdiConstructionYard;
        public int XInWorldCoordinates { get; set; }
        public int YInWorldCoordinates { get; set; }



        public const string SPRITE_KEY = "ConstructionYard";

        // TODO:  SHP_FILE_NAME and ShpFileColorMapper don't really belong in this view
        // Views should be agnostic about where the sprite data was loaded from
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
            // this.myGdiConstructionYard = constructionYard;

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



    }
}
