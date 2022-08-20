namespace mike_and_conquer_monogame.gameview
{
    public class UnitSize
    {

        private int width;
        private int height;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public UnitSize(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }


    }
}

