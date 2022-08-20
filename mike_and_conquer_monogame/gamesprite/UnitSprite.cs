using System;
using System.Collections.Generic;
using mike_and_conquer_monogame.openra;
using AnimationSequence = mike_and_conquer_monogame.util.AnimationSequence;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Boolean = System.Boolean;
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

        Dictionary<int, AnimationSequence> animationSequenceMap;
        int currentAnimationSequenceIndex;

        private List<UnitFrame> unitFrameList;

        Texture2D currentTexture;

        Texture2D spriteBorderRectangleTexture;
        public Boolean drawBoundingRectangle;

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

            spriteBorderRectangleTexture = CreateSpriteBorderRectangleTexture();

            middleOfSpriteInSpriteCoordinates = new Vector2();

            UnitFrame firstUnitFrame = unitFrameList[0];
            middleOfSpriteInSpriteCoordinates.X = firstUnitFrame.Texture.Width / 2;
            middleOfSpriteInSpriteCoordinates.Y = firstUnitFrame.Texture.Height / 2;
            this.width = firstUnitFrame.Texture.Width;
            this.height = firstUnitFrame.Texture.Height;
            drawBoundingRectangle = false;
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


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 positionInWorldCoordinates, float layerDepth)
        {
            AnimationSequence currentAnimationSequence = animationSequenceMap[currentAnimationSequenceIndex];
            if (animate)
            {
                currentAnimationSequence.Update();
            }

            int currentAnimationImageIndex = currentAnimationSequence.GetCurrentFrame();
            currentTexture = unitFrameList[currentAnimationImageIndex].Texture;

            float defaultScale = 1;

            if (drawShadow)
            {
                UpdateShadowPixels(positionInWorldCoordinates, currentAnimationImageIndex);
            }

            spriteBatch.Draw(currentTexture, positionInWorldCoordinates, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, layerDepth);

            if (drawBoundingRectangle)
            {
                spriteBatch.Draw(spriteBorderRectangleTexture, positionInWorldCoordinates, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 0f);
            }

        }

        public void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch, Vector2 positionInWorldCoordinates, float layerDepth)
        {
            AnimationSequence currentAnimationSequence = animationSequenceMap[currentAnimationSequenceIndex];
            if (animate)
            {
                currentAnimationSequence.Update();
            }

            int currentAnimationImageIndex = currentAnimationSequence.GetCurrentFrame();

//            int currentAnimationImageIndex = 0;

            currentTexture = unitFrameList[currentAnimationImageIndex].Texture;

            float defaultScale = 1;

            spriteBatch.Draw(currentTexture, positionInWorldCoordinates, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, layerDepth);

            if (drawBoundingRectangle)
            {
                // TODO use snapped coordinates?
                spriteBatch.Draw(spriteBorderRectangleTexture, positionInWorldCoordinates, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 0f);
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

//            int currentAnimationImageIndex = 0;

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


        internal Texture2D CreateSpriteBorderRectangleTexture()
        {

            Color color = Color.White;
            string borderRectangleName = "BorderRectangle-Color-R-" + color.R + "-G-" + color.G + "-B-" + color.B +
                                         "-width-" + width + "-height-" + height;

            Texture2D borderRectangleTexture = MikeAndConquerGame.instance.SpriteSheet.GetTextureForKey(borderRectangleName);

            if (borderRectangleTexture == null)
            {

                borderRectangleTexture =
                    new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, unitFrameList[0].Texture.Width,
                        unitFrameList[0].Texture.Height);
                Color[] data = new Color[borderRectangleTexture.Width * borderRectangleTexture.Height];
                FillHorizontalLine(data, borderRectangleTexture.Width, borderRectangleTexture.Height, 0, Color.White);
                FillHorizontalLine(data, borderRectangleTexture.Width, borderRectangleTexture.Height, borderRectangleTexture.Height - 1, Color.White);
                FillVerticalLine(data, borderRectangleTexture.Width, borderRectangleTexture.Height, 0, Color.White);
                FillVerticalLine(data, borderRectangleTexture.Width, borderRectangleTexture.Height, borderRectangleTexture.Width - 1, Color.White);

                int centerX = (borderRectangleTexture.Width / 2) ;
                int centerY = (borderRectangleTexture.Height / 2);

                // Check how this works for even sized sprites with true center

                int centerOffset = (centerY * borderRectangleTexture.Width) + centerX;

                data[centerOffset] = Color.Red;

                borderRectangleTexture.SetData(data);
                MikeAndConquerGame.instance.SpriteSheet.SetTextureForKey(borderRectangleName, borderRectangleTexture);
            }

            return borderRectangleTexture;

        }


        internal void FillHorizontalLine(Color[] data, int width, int height, int lineIndex, Color color)
        {
            int beginIndex = width * lineIndex;
            for (int i = beginIndex; i < (beginIndex + width); ++i)
            {
                data[i] = color;
            }
        }

        internal void FillVerticalLine(Color[] data, int width, int height, int lineIndex, Color color)
        {
            int beginIndex = lineIndex;
            for (int i = beginIndex; i < (width * height); i += width)
            {
                data[i] = color;
            }
        }

        public void SetAnimate(bool animateFlag)
        {
            this.animate = animateFlag;
        }


    }





}
