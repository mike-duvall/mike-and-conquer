using mike_and_conquer_monogame.util;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Color = Microsoft.Xna.Framework.Color;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using SpriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects;


namespace mike_and_conquer_monogame.gamesprite
{
    public class SingleTextureSprite
    {

        Texture2D texture;
        Texture2D spriteBorderRectangleTexture;
        public Vector2 middleOfSpriteInSpriteCoordinates;


        public int Width
        {
            get { return texture.Width; }
        }

        public int Height
        {
            get { return texture.Height; }
        }

        public SingleTextureSprite(Texture2D texture)
        {
            this.texture = texture;
            spriteBorderRectangleTexture = TextureUtil.CreateSpriteBorderRectangleTexture(Color.White, Width,Height);
            middleOfSpriteInSpriteCoordinates = new Vector2();

            middleOfSpriteInSpriteCoordinates.X = Width / 2;
            middleOfSpriteInSpriteCoordinates.Y = Height / 2;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 positionInWorldCoordinates,
            float layerDepth)
        {
            this.Draw(gameTime, spriteBatch, positionInWorldCoordinates, layerDepth, false, Color.White);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 positionInWorldCoordinates, float layerDepth, bool drawBoundingRectangle, Color boundingRectColor)
        {

            float defaultScale = 1;
            spriteBatch.Draw(texture, positionInWorldCoordinates, null, Color.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, layerDepth);

            if (drawBoundingRectangle)
            {
                spriteBatch.Draw(spriteBorderRectangleTexture, positionInWorldCoordinates, null, boundingRectColor, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 0f);
            }

        }

    }





}
