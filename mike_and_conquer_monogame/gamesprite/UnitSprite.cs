using System;
using System.Collections.Generic;
using mike_and_conquer_monogame.gameview;
using mike_and_conquer_monogame.openra;
using AnimationSequence = mike_and_conquer_monogame.util.AnimationSequence;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Color = Microsoft.Xna.Framework.Color;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using SpriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects;

using MikeAndConquerGame = mike_and_conquer_monogame.main.MikeAndConquerGame;

namespace mike_and_conquer_monogame.gamesprite
{
    public class UnitSprite
    {
        private readonly Dictionary<int, AnimationSequence> animationSequenceMap;
        private int currentAnimationSequenceIndex;

        private readonly List<UnitFrame> unitFrameList;

        private Texture2D currentTexture;

        private bool drawBoundingRectangle;
        private readonly SquareView boundingRectangleSquareView;

        public Vector2 middleOfSpriteInSpriteCoordinates;

        private bool animate;

        private ImmutablePalette palette;

        public bool drawShadow;

        private int width;
        public int Width
        {
            get { return width; }
        }

        private int height;
        public int Height
        {
            get { return height; }
        }


        public UnitSprite(string spriteListKey)
        {
            this.animationSequenceMap = new Dictionary<int, util.AnimationSequence>();
            unitFrameList = MikeAndConquerGame.instance.SpriteSheet.GetUnitFramesForShpFile(spriteListKey);


            middleOfSpriteInSpriteCoordinates = new Vector2();

            UnitFrame firstUnitFrame = unitFrameList[0];
            middleOfSpriteInSpriteCoordinates.X = firstUnitFrame.Texture.Width / 2;
            middleOfSpriteInSpriteCoordinates.Y = firstUnitFrame.Texture.Height / 2;
            this.width = firstUnitFrame.Texture.Width;
            this.height = firstUnitFrame.Texture.Height;
            // drawBoundingRectangle = true;
            drawBoundingRectangle = false;
            boundingRectangleSquareView = new SquareView(0, 0, width, height, CnCColorAsXnaColor.CncWhite_63_63_63, CnCColorAsXnaColor.CncRed_55_05_02);

            this.animate = true;
            int[] remap = { };
            palette = new ImmutablePalette(MikeAndConquerGame.CONTENT_DIRECTORY_PREFIX + "temperat.pal", remap);
            drawShadow = false;
        }

        public void SetCurrentAnimationSequenceIndex(int animationSequenceIndex)
        {
            if (currentAnimationSequenceIndex == animationSequenceIndex)
            {
                return;
            }

            currentAnimationSequenceIndex = animationSequenceIndex;

            AnimationSequence animationSequence = animationSequenceMap[currentAnimationSequenceIndex];
            animationSequence.SetCurrentFrameIndex(0);

            int currentAnimationImageIndex = animationSequence.GetCurrentFrame();
            currentTexture = unitFrameList[currentAnimationImageIndex].Texture;
        }

        public void SetFrameOfCurrentAnimationSequence(int frame)
        {
            AnimationSequence animationSequence = animationSequenceMap[currentAnimationSequenceIndex];
            animationSequence.SetCurrentFrameIndex(frame);
        }

        public void AddAnimationSequence(int key, AnimationSequence  animationSequence)
        {
            animationSequenceMap[key] = animationSequence;
        }



        public void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch, Vector2 positionInWorldCoordinates, float layerDepth)
        {
            AnimationSequence currentAnimationSequence = animationSequenceMap[currentAnimationSequenceIndex];
            if (animate)
            {
                currentAnimationSequence.Update();
            }

            int currentAnimationImageIndex = currentAnimationSequence.GetCurrentFrame();

            currentTexture = unitFrameList[currentAnimationImageIndex].Texture;

            const float defaultScale = 1;

            spriteBatch.Draw(currentTexture, positionInWorldCoordinates, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, layerDepth);

            if (drawBoundingRectangle)
            {
                boundingRectangleSquareView.Update(gameTime, (int)positionInWorldCoordinates.X, (int)positionInWorldCoordinates.Y);
                boundingRectangleSquareView.DrawNoShadow(gameTime, spriteBatch);
            }


        }

        public void DrawShadowOnly(GameTime gameTime, SpriteBatch spriteBatch, Vector2 positionInWorldCoordinates, float layerDepth)
        {
            AnimationSequence currentAnimationSequence = animationSequenceMap[currentAnimationSequenceIndex];
            if (animate)
            {
                currentAnimationSequence.Update();
            }

            int currentAnimationImageIndex = currentAnimationSequence.GetCurrentFrame();

            Texture2D shadowOnlyTexture = unitFrameList[currentAnimationImageIndex].ShadowOnlyTexture;

            float defaultScale = 1;

            int roundedX = (int)Math.Round(positionInWorldCoordinates.X, 0);
            int roundedY = (int)Math.Round(positionInWorldCoordinates.Y, 0);

            Vector2 snappedPositionInWordCoordinates = new Vector2(roundedX, roundedY);

            spriteBatch.Draw(shadowOnlyTexture, snappedPositionInWordCoordinates, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, layerDepth);
        }

        // How to draw shadows:
        // X       Write method that returns current tile, given a point on the map
        // X       Write method that returns color index of given point on the map
        //        Write method that then maps the color index to the shadow index color
        //        Write method that creates new texture for minigunner with shadow colors fixed up 
        // Create new texture of same size
        // Copy pixels over one by one
        // If pixel is the shadow green:
        //    determine the screen x,y of that pixel 
        //    determine the palette value of the existing screen background at that position
        //    map that background to the proper shadow pixel
        //    set that pixel in the new texture to that shadow pixel
        // Draw the texture


        // TODO:  Consider if we want to use
        // MonogameExtended DrawPoint() method to fill in dynamic shadow pixels
        // for units, rather than this method of directly manipulating texture data
        // DrawPoint() might be faster since it would be operating on VRAM
        private void UpdateShadowPixels(Vector2 positionInWorldCoordinates, int imageIndex)
        {
            Color[] texturePixelData = new Color[currentTexture.Width * currentTexture.Height];
            currentTexture.GetData(texturePixelData);
            List<int> shadowIndexList = unitFrameList[imageIndex].ShadowIndexList;


            int topLeftXOfSpriteInWorldCoordinates =
                (int)positionInWorldCoordinates.X - (int)middleOfSpriteInSpriteCoordinates.X;
            int topLeftYOfSpriteInWorldCoordinates =
                (int)positionInWorldCoordinates.Y - (int)middleOfSpriteInSpriteCoordinates.Y;


            texturePixelData = ShadowHelper.UpdateShadowPixels(
                topLeftXOfSpriteInWorldCoordinates,
                topLeftYOfSpriteInWorldCoordinates,
                texturePixelData,
                shadowIndexList,
                currentTexture.Width,
                this.palette
                );
            currentTexture.SetData(texturePixelData);

        }

        public void SetAnimate(bool animateFlag)
        {
            this.animate = animateFlag;
        }


    }





}
