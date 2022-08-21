using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mike_and_conquer_monogame.main;

namespace mike_and_conquer_monogame.gamesprite
{
    class PartiallyVisibileMapTileMask
    {

        public const string SPRITE_KEY = "MapTileShadow";
        public const string SHP_FILE_NAME = "shadow.shp";

        private static List<UnitFrame> shadowFrameList;
        private static Texture2D blankMask;
        public static int MISSING_MAPPING_INDEX = 666;


        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();


        public PartiallyVisibileMapTileMask()
        {
            if (shadowFrameList == null)
            {
                shadowFrameList =
                    MikeAndConquerGame.instance.SpriteSheet.GetUnitFramesForShpFile(SPRITE_KEY);
                InitializeMissingMappingMask();
            }

        }

        private static void InitializeMissingMappingMask()
        {
            const string BlankMaskTextureKey = "BlankMaskTextureKey";

            blankMask = MikeAndConquerGame.instance.SpriteSheet.GetTextureForKey(BlankMaskTextureKey);

            if (blankMask == null)
            {
                blankMask = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, 24, 24);
                Color[] colorData = new Color[blankMask.Width * blankMask.Height];
                int numPixels = 24 * 24;
                for (int i = 0; i < numPixels; i++)
                {
                    Color xnaColor = new Color(16, 0, 0, 1);
                    colorData[i] = xnaColor;
                }

                blankMask.SetData(colorData);

            }
        }


        public Texture2D GetMask(int index)
        {
            if (index == MISSING_MAPPING_INDEX)
            {
                return blankMask;
            }
            else
            {
                return shadowFrameList[index].Texture;
            }

        }
    }
}
