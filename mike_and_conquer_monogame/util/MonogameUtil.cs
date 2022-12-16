
using XnaVector2 = Microsoft.Xna.Framework.Vector2;
using SystemNumericsVector2 = System.Numerics.Vector2;
using XnaPoint =  Microsoft.Xna.Framework.Point;
using SystemDrawingPoint = System.Drawing.Point;


namespace mike_and_conquer_monogame.util
{
    internal class MonogameUtil
    {

        internal static XnaVector2 ConvertSystemNumericsVector2ToXnaVector2(SystemNumericsVector2 systemNumericsVector2)
        {
            XnaVector2 xnaVector2 = new XnaVector2(systemNumericsVector2.X, systemNumericsVector2.Y);
            return xnaVector2;
        }


        public static XnaVector2 ConvertXnaPointToXnaVector2(XnaPoint xnaPoint)
        {
            return new XnaVector2(xnaPoint.X, xnaPoint.Y);
        }


        public static XnaPoint ConvertXnaVector2ToXnaPoint(XnaVector2 xnaVector2)
        {
            return new XnaPoint((int) xnaVector2.X, (int) xnaVector2.Y);
        }

        public static XnaPoint ConvertSystemDrawingPointToXnaPoint(SystemDrawingPoint systemDrawingPoint)
        {
            return new XnaPoint(systemDrawingPoint.X, systemDrawingPoint.Y);
        }
    }
}
