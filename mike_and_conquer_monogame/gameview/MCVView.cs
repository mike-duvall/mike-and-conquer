using mike_and_conquer_monogame.gamesprite;
using AnimationSequence = mike_and_conquer_monogame.util.AnimationSequence;

using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


using XnaPoint = Microsoft.Xna.Framework.Point;

namespace mike_and_conquer_monogame.gameview
{
    public class MCVView : UnitView
    {
        private UnitSprite unitSprite;
        private UnitSelectionCursor unitSelectionCursor;
        private DestinationSquare destinationSquare;
        private bool drawDestinationSquare;

        //        enum AnimationSequences { STANDING_STILL, WALKING_UP, SHOOTING_UP };

        public const string SPRITE_KEY = "MCV";
        public const string SHP_FILE_NAME = "Shp\\mcv.shp";
        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();



        public MCVView(int unitId,  int xInWorldCoordinates, int yInWorldCoordinates)
        {
            // this.myMCV = mcv;
            this.UnitId = unitId;
            this.XInWorldCoordinates = xInWorldCoordinates;
            this.YInWorldCoordinates = yInWorldCoordinates;

            this.unitSprite = new UnitSprite(SPRITE_KEY);
            this.unitSprite.drawBoundingRectangle = true;
            this.unitSprite.drawShadow = true;
            // this.mcvSelectionBox = new MCVSelectionBox();

            this.unitSize = new UnitSize(36, 36);
            this.selectionCursorOffset = new XnaPoint(-18, -14);

            this.unitSelectionCursor = new UnitSelectionCursor(this, XInWorldCoordinates, YInWorldCoordinates);

            // this.destinationSquare = new DestinationSquare();
            this.drawDestinationSquare = false;

            AnimationSequence animationSequence = new AnimationSequence(1);
            animationSequence.AddFrame(0);
            unitSprite.AddAnimationSequence(0, animationSequence);

        }


        internal override void Update(GameTime gameTime)
        {
            unitSelectionCursor.Update(gameTime);
        }


        internal override void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // if (myMCV.Health <= 0)
            // {
            //     return;
            // }


            Vector2 worldCoordinatesAsVector2 = new Vector2(
                XInWorldCoordinates,
                YInWorldCoordinates);

            unitSprite.DrawNoShadow(gameTime, spriteBatch, worldCoordinatesAsVector2, SpriteSortLayers.UNIT_DEPTH);


            if (Selected)
            {
                unitSelectionCursor.DrawNoShadow(gameTime, spriteBatch, SpriteSortLayers.UNIT_DEPTH);
            }


        }


        internal override void DrawShadowOnly(GameTime gameTime, SpriteBatch spriteBatch)
        {

            // if (myMCV.Health <= 0)
            // {
            //     return;
            // }


            Vector2 worldCoordinatesAsVector2 = new Vector2(
                XInWorldCoordinates,
                YInWorldCoordinates);

            unitSprite.DrawShadowOnly(gameTime, spriteBatch, worldCoordinatesAsVector2, SpriteSortLayers.UNIT_DEPTH);


            if (Selected)
            {
                unitSelectionCursor.DrawNoShadow(gameTime, spriteBatch, SpriteSortLayers.UNIT_DEPTH);
            }


        }



        public void SetAnimate(bool animateFlag)
        {
            unitSprite.SetAnimate(animateFlag);
        }



    }
}
