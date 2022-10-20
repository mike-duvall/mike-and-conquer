using Microsoft.Xna.Framework;
using mike_and_conquer_monogame.gamesprite;
using mike_and_conquer_monogame.main;
// using mike_and_conquer.gameobjects;
using AnimationSequence = mike_and_conquer_monogame.util.AnimationSequence;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

 
namespace mike_and_conquer_monogame.gameview
{
    public class MinigunnerView : UnitView
    {
        private UnitSprite unitSprite;
        private UnitSelectionCursor unitSelectionCursor;
        private DestinationSquare destinationSquare;
        // private Minigunner myMinigunner;
        private bool drawDestinationSquare;



        // public int XInWorldCoordinates { get; set; }
        // public int YInWorldCoordinates { get; set; }
        //

        enum AnimationSequences { STANDING_STILL, WALKING_UP, SHOOTING_UP };


        protected MinigunnerView(int unitId,string spriteListKey, int xInWorldCoordinates, int yInWorldCoordinates)
        {
            this.UnitId = unitId;
            this.XInWorldCoordinates = xInWorldCoordinates;
            this.YInWorldCoordinates = yInWorldCoordinates;
            this.unitSprite = new UnitSprite(spriteListKey);
            this.unitSprite.drawBoundingRectangle = false;
            this.unitSprite.drawShadow = true;

            this.unitSize = new UnitSize(12, 16);


            this.unitSelectionCursor = new UnitSelectionCursor(this, XInWorldCoordinates, YInWorldCoordinates);
            // this.destinationSquare = new DestinationSquare();
            this.drawDestinationSquare = false;
            SetupAnimations();
            this.selectionCursorOffset = new Point(-6, -10);
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


            AnimationSequence shootinUpAnimationSequence = new AnimationSequence(10);
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
        }


        internal override void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // if (myMinigunner.Health <= 0)
            // {
            //     return;
            // }


            // TODO:  move everything but actual drawing to Update() method
            // if (myMinigunner.state == Minigunner.State.IDLE)
            // {
                unitSprite.SetCurrentAnimationSequenceIndex((int)AnimationSequences.STANDING_STILL);
            // }
            // else if (myMinigunner.state == Minigunner.State.MOVING)
            // {
            //     unitSprite.SetCurrentAnimationSequenceIndex((int)AnimationSequences.WALKING_UP);
            // }
            // else if (myMinigunner.state == Minigunner.State.ATTACKING)
            // {
            //     unitSprite.SetCurrentAnimationSequenceIndex((int)AnimationSequences.SHOOTING_UP);
            // }


            // unitSprite.DrawNoShadow(gameTime, spriteBatch, myMinigunner.GameWorldLocation.WorldCoordinatesAsVector2, SpriteSortLayers.UNIT_DEPTH);
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
