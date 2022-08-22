


namespace mike_and_conquer_simulation.main
{
    public class GameWorldLocation
    {
        private float xInWorldCoordinates;
        private float yInWorldCoordinates;


        public float X
        {
            get => xInWorldCoordinates;
            set => xInWorldCoordinates = value;
        }

        public float Y
        {
            get => yInWorldCoordinates;
            set => yInWorldCoordinates = value;
        }




        private GameWorldLocation(float x, float y)
        {
            this.xInWorldCoordinates = x;
            this.yInWorldCoordinates = y;
        }

        public GameWorldLocation(GameWorldLocationBuilder builder)
        {
            this.xInWorldCoordinates = builder.WorldCoordinatesX();
            this.yInWorldCoordinates = builder.WorldCoordinatesY();
        }

        public static GameWorldLocation CreateFromWorldCoordinates(float x, float y)
        {
            return new GameWorldLocation(x, y);
        }



    }
}
