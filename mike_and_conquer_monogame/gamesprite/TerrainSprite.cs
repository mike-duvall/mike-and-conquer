using System.Collections.Generic;
using Microsoft.Xna.Framework;
using mike_and_conquer_monogame.gameview;
using mike_and_conquer_monogame.main;
using mike_and_conquer_monogame.openra;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Boolean = System.Boolean;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Color = Microsoft.Xna.Framework.Color;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using SpriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects;

namespace mike_and_conquer_monogame.gamesprite
{
    public class TerrainSprite
    {

        private List<UnitFrame> unitFrameList;
        private Texture2D noShadowTexture;
        private Texture2D shadowOnlytexture2D;

        private Texture2D spriteBorderRectangleTexture;

        public Boolean drawBoundingRectangle;

        private Vector2 spriteOrigin;

        private ImmutablePalette palette;

        private int unitFrameImageIndex;

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


        private const string BorderTextureKey = "TerrainSpriteBorderTexture";

        public TerrainSprite(string spriteListKey, Point position )
        {
            unitFrameList = MikeAndConquerGame.instance.SpriteSheet.GetUnitFramesForShpFile(spriteListKey);
            unitFrameImageIndex = 0;

            spriteBorderRectangleTexture = MikeAndConquerGame.instance.SpriteSheet.GetTextureForKey(BorderTextureKey);

            if (spriteBorderRectangleTexture == null)
            {
                spriteBorderRectangleTexture = CreateSpriteBorderRectangleTexture();
                MikeAndConquerGame.instance.SpriteSheet.SetTextureForKey(BorderTextureKey, spriteBorderRectangleTexture);

            }

            spriteOrigin = new Vector2(GameWorldView.MAP_TILE_WIDTH / 2, GameWorldView.MAP_TILE_HEIGHT / 2);

            UnitFrame firstUnitFrame = unitFrameList[unitFrameImageIndex];
            this.width = firstUnitFrame.Texture.Width;
            this.height = firstUnitFrame.Texture.Height;
            drawBoundingRectangle = false;
            int[] remap = { };
            palette = new ImmutablePalette(MikeAndConquerGame.CONTENT_DIRECTORY_PREFIX + "temperat.pal", remap);

            InitializeNoShadowTexture();
            InitializeShadowOnlyTexture(position);
        }

        private void InitializeNoShadowTexture()
        {
            noShadowTexture = unitFrameList[0].Texture;
        }


        private void InitializeShadowOnlyTexture(Point positionInWorldCoordinates)
        {
            shadowOnlytexture2D = unitFrameList[0].ShadowOnlyTexture;
        }

        // This old Draw() method maps the shadow pixels on the fly and is too slow
        // Keeping around for reference for now
        // Instead should use the separate DrawShadowOnly() and DrawNoShadow() methods
        // to draw the pre-rendered shadows, which is much faster
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 positionInWorldCoordinates)
        {
            int currentAnimationImageIndex = 0;
        
            float defaultScale = 1;
        
            UpdateShadowPixels(positionInWorldCoordinates, currentAnimationImageIndex);
        
            spriteBatch.Draw(noShadowTexture, positionInWorldCoordinates, null, Color.White, 0f, spriteOrigin, defaultScale, SpriteEffects.None, 0f);
        
            if (drawBoundingRectangle)
            {
                spriteBatch.Draw(spriteBorderRectangleTexture, positionInWorldCoordinates, null, Color.White, 0f, spriteOrigin, defaultScale, SpriteEffects.None, 0f);
            }
        }


        public void DrawShadowOnly(GameTime gameTime, SpriteBatch spriteBatch, Vector2 positionInWorldCoordinates)
        {

            float defaultScale = 1;
            spriteBatch.Draw(shadowOnlytexture2D, positionInWorldCoordinates, null, Color.White, 0f, spriteOrigin,
                defaultScale, SpriteEffects.None, SpriteSortLayers.TERRAIN_SHADOW_DEPTH);

        }

        public void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch, Vector2 positionInWorldCoordinates, float layerDepthOffset)
        {
            float defaultScale = 1;

            spriteBatch.Draw(noShadowTexture, positionInWorldCoordinates, null, Color.White, 0f, spriteOrigin,
                defaultScale, SpriteEffects.None, SpriteSortLayers.TERRAIN_DEPTH - layerDepthOffset);

            if (drawBoundingRectangle)
            {
                spriteBatch.Draw(spriteBorderRectangleTexture, positionInWorldCoordinates, null, Color.White, 0f, spriteOrigin, defaultScale, SpriteEffects.None, SpriteSortLayers.TERRAIN_DEPTH);
            }
        }



        private void UpdateShadowPixels(Vector2 positionInWorldCoordinates, int imageIndex)
        {
            Color[] texturePixelData = new Color[noShadowTexture.Width * noShadowTexture.Height];
            noShadowTexture.GetData(texturePixelData);


            List<int> shadowIndexList = unitFrameList[imageIndex].ShadowIndexList;
            int topLeftXOfSpriteInWorldCoordinates = (int) positionInWorldCoordinates.X;
            int topLeftYOfSpriteInWorldCoordinates = (int) positionInWorldCoordinates.Y;

            Color[] texturePixelDatWithShadowsUpdated = ShadowHelper.UpdateShadowPixels(
                topLeftXOfSpriteInWorldCoordinates,
                topLeftYOfSpriteInWorldCoordinates,
                texturePixelData,
                shadowIndexList,
                shadowOnlytexture2D.Width,
                this.palette
            );

            noShadowTexture.SetData(texturePixelDatWithShadowsUpdated);

        }


        internal Texture2D CreateSpriteBorderRectangleTexture()
        {
            Texture2D rectangle =
                new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, unitFrameList[0].Texture.Width,
                    unitFrameList[0].Texture.Height);
            Color[] data = new Color[rectangle.Width * rectangle.Height];
            FillHorizontalLine(data, rectangle.Width, rectangle.Height, 0, Color.White);
            FillHorizontalLine(data, rectangle.Width, rectangle.Height, rectangle.Height - 1, Color.White);
            FillVerticalLine(data, rectangle.Width, rectangle.Height, 0, Color.White);
            FillVerticalLine(data, rectangle.Width, rectangle.Height, rectangle.Width - 1, Color.White);

            int centerX = (rectangle.Width / 2) ;
            int centerY = (rectangle.Height / 2);

            // Check how this works for even sized sprites with true center

            int centerOffset = (centerY * rectangle.Width) + centerX;

            data[centerOffset] = Color.Red;

            rectangle.SetData(data);
            return rectangle;

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

    }





}
