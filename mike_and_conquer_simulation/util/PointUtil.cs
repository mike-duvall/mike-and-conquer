


using Point = System.Drawing.Point;
using Vector2 = System.Numerics.Vector2;

namespace mike_and_conquer_simulation.util
{
    public class PointUtil
    {

        public static System.Drawing.Point ConvertVector2ToPoint(System.Numerics.Vector2 aVector2)
        {
            return new Point((int) aVector2.X, (int) aVector2.Y);
        }

        public static Vector2 ConvertPointToVector2(Point aPoint)
        {
            return new Vector2(aPoint.X, aPoint.Y);
        }


    }
}
