using mike_and_conquer_monogame.gamesprite;
using mike_and_conquer_monogame.main;
using AnimationSequence = mike_and_conquer_monogame.util.AnimationSequence;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Vector2 = Microsoft.Xna.Framework.Vector2;



namespace mike_and_conquer_monogame.gameview
{
    public class MinigunnerView : UnitView
    {
        private UnitSelectionCursor unitSelectionCursor;


        private static int MINIGUNNER_VIEW_CLICK_DETECTION_RECTANGLE_X_OFFSET = 0;
        private static int MINIGUNNER_VIEW_CLICK_DETECTION_RECTANGLE_Y_OFFSET = -5;

        private static int MINIGUNNER_UNIT_SIZE_WIDTH = 12;
        private static int MINIGUNNER_UNIT_SIZE_HEIGHT = 16;

        enum AnimationSequences { STANDING_STILL, WALKING_UP, SHOOTING_UP };

        protected MinigunnerView(
            int unitId,
            string spriteListKey,
            int xInWorldCoordinates,
            int yInWorldCoordinates,
            int maxHealth,
            int health) :

                base(unitId,
                    xInWorldCoordinates,
                    yInWorldCoordinates,
                    MINIGUNNER_UNIT_SIZE_WIDTH,
                    MINIGUNNER_UNIT_SIZE_HEIGHT,
                    maxHealth,
                    health,
                    MINIGUNNER_VIEW_CLICK_DETECTION_RECTANGLE_X_OFFSET,
                    MINIGUNNER_VIEW_CLICK_DETECTION_RECTANGLE_Y_OFFSET)
        {
            this.unitSprite = new UnitSprite(spriteListKey);
            this.unitSize = new UnitSize(12, 16);

            this.unitSelectionCursor = new UnitSelectionCursor(
                this,
                13,
                13,
                3,
                4,
                12,
                6,
                6,
                XInWorldCoordinates,
                YInWorldCoordinates);


            SetupAnimations();

            this.selectionCursorOffset = new Point(0, -4);

            // showClickDetectionRectangle = true;

        }


        private void SetupAnimations()
        {
            AnimationSequence walkingUpAnimationSequence = new AnimationSequence(40);
            walkingUpAnimationSequence.AddFrame(16);
            walkingUpAnimationSequence.AddFrame(17);
            walkingUpAnimationSequence.AddFrame(18);
            walkingUpAnimationSequence.AddFrame(19);
            walkingUpAnimationSequence.AddFrame(20);
            walkingUpAnimationSequence.AddFrame(21);

            unitSprite.AddAnimationSequence((int)AnimationSequences.WALKING_UP, walkingUpAnimationSequence);

            AnimationSequence standingStillAnimationSequence = new AnimationSequence(10);
            standingStillAnimationSequence.AddFrame(0);
            unitSprite.AddAnimationSequence((int)AnimationSequences.STANDING_STILL, standingStillAnimationSequence);
            unitSprite.SetCurrentAnimationSequenceIndex((int)AnimationSequences.STANDING_STILL);


            AnimationSequence shootinUpAnimationSequence = new AnimationSequence(100);
            shootinUpAnimationSequence.AddFrame(65);
            shootinUpAnimationSequence.AddFrame(66);
            shootinUpAnimationSequence.AddFrame(67);
            shootinUpAnimationSequence.AddFrame(68);
            shootinUpAnimationSequence.AddFrame(69);
            shootinUpAnimationSequence.AddFrame(70);
            shootinUpAnimationSequence.AddFrame(71);
            shootinUpAnimationSequence.AddFrame(72);
            unitSprite.AddAnimationSequence((int)AnimationSequences.SHOOTING_UP, shootinUpAnimationSequence);
        }



        internal override void UpdateInternal(GameTime gameTime)
        {
            unitSelectionCursor.Update(gameTime);
        }


        internal override void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // if (myMinigunner.Health <= 0)
            // {
            //     return;
            // }


            // TODO:  move everything but actual drawing to Update() method

            if (CurrentUnitState == UnitState.FIRING)
            {
                unitSprite.SetCurrentAnimationSequenceIndex((int)AnimationSequences.SHOOTING_UP);
            }
            else if (CurrentUnitState == UnitState.MOVING)
            {
                unitSprite.SetCurrentAnimationSequenceIndex((int)AnimationSequences.WALKING_UP);
            }
            else
            {
                unitSprite.SetCurrentAnimationSequenceIndex((int)AnimationSequences.STANDING_STILL);
            }


            Vector2 worldCoordinatesAsVector2 = new Vector2(
                XInWorldCoordinates,
                YInWorldCoordinates);

            unitSprite.DrawNoShadow(gameTime, spriteBatch, worldCoordinatesAsVector2, SpriteSortLayers.UNIT_DEPTH);

            if (GameOptions.instance.DrawPaths && plannedPathView != null)
            {
                plannedPathView.Draw(spriteBatch);
            }


            if (Selected)
            {
                unitSelectionCursor.DrawNoShadow(gameTime, spriteBatch, SpriteSortLayers.UNIT_DEPTH);
            }

            if(showClickDetectionRectangle)
            {
                clickDetectionRectangle.DrawNoShadow(gameTime, spriteBatch);
            }

        }


        internal override void DrawShadowOnly(GameTime gameTime, SpriteBatch spriteBatch)
        {

            // if (myMinigunner.Health <= 0)
            // {
            //     return;
            // }


            // unitSprite.DrawShadowOnly(gameTime, spriteBatch, myMinigunner.GameWorldLocation.WorldCoordinatesAsVector2, SpriteSortLayers.UNIT_DEPTH);

            Vector2 worldCoordinatesAsVector2 = new Vector2(
                XInWorldCoordinates,
                YInWorldCoordinates);

            unitSprite.DrawShadowOnly(gameTime, spriteBatch, worldCoordinatesAsVector2, SpriteSortLayers.UNIT_DEPTH);


            if (Selected)
            {
                unitSelectionCursor.DrawShadowOnly(gameTime, spriteBatch, SpriteSortLayers.UNIT_DEPTH);
            }

        }


        public void SetAnimate(bool animateFlag)
        {
            unitSprite.SetAnimate(animateFlag);
        }


    }
}
