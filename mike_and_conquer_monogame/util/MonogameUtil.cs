
namespace mike_and_conquer_monogame.util
{
    internal class MonogameUtil
    {

        internal static Microsoft.Xna.Framework.Vector2 ConvertSystemNumericsVector2ToXnaVector2(System.Numerics.Vector2 systemNumericsVector2)
        {
            Microsoft.Xna.Framework.Vector2 xnaVector2 = new Microsoft.Xna.Framework.Vector2(systemNumericsVector2.X, systemNumericsVector2.Y);
            return xnaVector2;
        }



    }
}
