using System.Collections.Generic;
using mike_and_conquer_monogame.openra;
// using mike_and_conquer.openra;
using Color = Microsoft.Xna.Framework.Color;


namespace mike_and_conquer_monogame.gamesprite
{
    public class ShadowHelper
    {
        public static Color SHADOW_COLOR = new Color(255, 0, 0, 255);


        // TODO:  Consider if this makes sense as separate method in helper, given that shadow mapper is now happening in the shader
        public static Color[] UpdateShadowPixels(
            int topLeftXOfSpriteInWorldCoordinates,
            int topLeftYOfSpriteInWorldCoordinates,
            Color[] texturePixelData,
            List<int> shadowIndexList,
            int width,
            ImmutablePalette palette)
        {

            foreach (int shadowIndex in shadowIndexList)
            {
                texturePixelData[shadowIndex] = SHADOW_COLOR;
            }

            return texturePixelData;
        }

    }



}
