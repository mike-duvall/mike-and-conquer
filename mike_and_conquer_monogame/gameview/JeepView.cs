using mike_and_conquer_monogame.gamesprite;
using AnimationSequence = mike_and_conquer_monogame.util.AnimationSequence;

using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

 
namespace mike_and_conquer_monogame.gameview
{
    public class JeepView : UnitView
    {
        private UnitSprite unitSprite;
        // private UnitSelectionCursor unitSelectionCursor;
        private DestinationSquare destinationSquare;
        // private MCV myMCV;
        private bool drawDestinationSquare;

        //        enum AnimationSequences { STANDING_STILL, WALKING_UP, SHOOTING_UP };

        private static int JEEP_VIEW_CLICK_DETECTION_RECTANGLE_X_OFFSET = 0;
        private static int JEEP_VIEW_CLICK_DETECTION_RECTANGLE_Y_OFFSET = 1;

        private static int JEEP_UNIT_SIZE_WIDTH = 26;
        private static int JEEP_UNIT_SIZE_HEIGHT = 26;


        public const string SPRITE_KEY = "Jeep";
        public const string SHP_FILE_NAME = "Shp\\Jeep.shp";
        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();


        public JeepView(int unitId,  int xInWorldCoordinates, int yInWorldCoordinates) :
            base(
                unitId,
                xInWorldCoordinates,
                yInWorldCoordinates,
                JEEP_UNIT_SIZE_WIDTH,
                JEEP_UNIT_SIZE_HEIGHT,
                100,
                100,
                JEEP_VIEW_CLICK_DETECTION_RECTANGLE_X_OFFSET,
                JEEP_VIEW_CLICK_DETECTION_RECTANGLE_Y_OFFSET)
        {
            this.UnitId = unitId;
            this.XInWorldCoordinates = xInWorldCoordinates;
            this.YInWorldCoordinates = yInWorldCoordinates;

            this.unitSprite = new UnitSprite(SPRITE_KEY);
            this.unitSize = new UnitSize(JEEP_UNIT_SIZE_WIDTH, JEEP_UNIT_SIZE_HEIGHT);

            this.unitSprite.drawShadow = true;
            this.drawDestinationSquare = false;

            AnimationSequence animationSequence = new AnimationSequence(1);
            animationSequence.AddFrame(8);
            unitSprite.AddAnimationSequence(0, animationSequence);

        }


        internal override void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // if (myMCV.Health <= 0)
            // {
            //     return;
            // }

            // unitSprite.DrawNoShadow(gameTime, spriteBatch, myMCV.GameWorldLocation.WorldCoordinatesAsVector2, SpriteSortLayers.UNIT_DEPTH);


            Vector2 worldCoordinatesAsVector2 = new Vector2(
                XInWorldCoordinates,
                YInWorldCoordinates);

            unitSprite.DrawNoShadow(gameTime, spriteBatch, worldCoordinatesAsVector2, SpriteSortLayers.UNIT_DEPTH);


            // if (myMCV.selected)
            // {
            //     unitSelectionCursor.DrawNoShadow(gameTime, spriteBatch, SpriteSortLayers.UNIT_DEPTH);
            // }


        }

        internal override void UpdateInternal(GameTime gameTime)
        {
           
        }


        internal override void DrawShadowOnly(GameTime gameTime, SpriteBatch spriteBatch)
        {

            // if (myMCV.Health <= 0)
            // {
            //     return;
            // }

            // unitSprite.DrawShadowOnly(gameTime, spriteBatch, myMCV.GameWorldLocation.WorldCoordinatesAsVector2, SpriteSortLayers.UNIT_DEPTH);



            Vector2 worldCoordinatesAsVector2 = new Vector2(
                XInWorldCoordinates,
                YInWorldCoordinates);

            unitSprite.DrawShadowOnly(gameTime, spriteBatch, worldCoordinatesAsVector2, SpriteSortLayers.UNIT_DEPTH);

            // if (myMCV.selected)
            // {
            //     unitSelectionCursor.DrawShadowOnly(gameTime, spriteBatch, SpriteSortLayers.UNIT_DEPTH);
            // }

        }



        public void SetAnimate(bool animateFlag)
        {
            unitSprite.SetAnimate(animateFlag);
        }



    }
}
