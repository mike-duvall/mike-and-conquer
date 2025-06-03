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
        private DestinationSquare destinationSquare;
        private bool drawDestinationSquare;


        private bool showClickDetectionRectangle;
        private ClickDetectionRectangle clickDetectionRectangle;


        private int clickDetectionRectangleYOffset = -5;
        private int clickDetectionRectangleXOffset = 0;

        enum AnimationSequences { STANDING_STILL, WALKING_UP, SHOOTING_UP };


        protected MinigunnerView(int unitId,string spriteListKey, int xInWorldCoordinates, int yInWorldCoordinates, int maxHealth, int health)
        {
            this.UnitId = unitId;
            this.XInWorldCoordinates = xInWorldCoordinates;
            this.YInWorldCoordinates = yInWorldCoordinates;
            this.MaxHealth = maxHealth;
            this.Health = health;
            this.unitSprite = new UnitSprite(spriteListKey);

            this.unitSize = new UnitSize(12, 16);


            this.unitSprite.drawShadow = true;


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


            // this.destinationSquare = new DestinationSquare();
            this.drawDestinationSquare = false;
            SetupAnimations();

            // this.selectionCursorOffset = new Point(0, -5);  // X is correct, y too high
            this.selectionCursorOffset = new Point(0, -4);

            // showClickDetectionRectangle = true;
            showClickDetectionRectangle = false;
            clickDetectionRectangle = new ClickDetectionRectangle(
                this.XInWorldCoordinates + clickDetectionRectangleXOffset,
                this.YInWorldCoordinates + clickDetectionRectangleYOffset,
                this.unitSize.Width + 1,
                this.unitSize.Height + 1);

        }

        internal override Rectangle CreateClickDetectionRectangle()
        {
            return clickDetectionRectangle.GetRectangle();

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



        internal override void Update(GameTime gameTime)
        {
            unitSelectionCursor.Update(gameTime);
            clickDetectionRectangle.Update(
                gameTime,
                XInWorldCoordinates + clickDetectionRectangleXOffset,
                YInWorldCoordinates + clickDetectionRectangleYOffset);
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
