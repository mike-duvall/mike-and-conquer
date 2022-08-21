using mike_and_conquer_monogame.gamesprite;
using GameTime = Microsoft.Xna.Framework.GameTime;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

using MikeAndConquerGame = mike_and_conquer_monogame.main.MikeAndConquerGame;


namespace mike_and_conquer_monogame.gameview
{
    class DestinationSquare
    {

        public const string SPRITE_KEY = "DestinationSquare";

        private SingleTextureSprite sprite;
        public Vector2 position;

        public DestinationSquare()
        {
            sprite = new SingleTextureSprite(MikeAndConquerGame.instance.SpriteSheet.GetTextureForKey(SPRITE_KEY));
        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch, position, 1.0f);
        }
    }
}
