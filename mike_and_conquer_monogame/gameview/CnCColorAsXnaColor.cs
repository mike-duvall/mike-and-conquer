using XnaColor = Microsoft.Xna.Framework.Color;


// When rendering colors in MikeAndConquer, colors in textures are converted to the actual real Cnc color by 
// reading the R component of the color and using that value to map to an actual color in the Cnc color palette
// This class provides a convenient pre-mapping of specific Cnc colors to Xna colors
// for use in manually created textures, such as used by drawing bounding rectangles, etc
namespace mike_and_conquer_monogame.gameview
{
    internal class CnCColorAsXnaColor
    {

        public static XnaColor CncRed_55_05_02 = new XnaColor(0x7e, 0, 0);
        public static XnaColor CncYellow_63_52_18 = new XnaColor(0x95, 0, 0);
        public static XnaColor CncTeal_14_38_36 = new XnaColor(0xdd, 0, 0);

    }
}
