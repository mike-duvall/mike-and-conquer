
using Microsoft.Xna.Framework.Graphics;

namespace mike_and_conquer_monogame.gamesprite
{
    public class MapTileFrame
    {

        private Texture2D mapTileTexture2D;

        public Texture2D Texture
        {
            get { return mapTileTexture2D; }
        }

        public MapTileFrame(Texture2D mapTileTexture2D)
        {
            this.mapTileTexture2D = mapTileTexture2D;
        }

    }
}
