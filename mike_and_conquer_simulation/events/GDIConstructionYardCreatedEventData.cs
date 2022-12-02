namespace mike_and_conquer_simulation.events
{
    public class GDIConstructionYardCreatedEventData 
    {
        public const string EventType = "GDIConstructionYardCreated";


        public int X { get; }
        public int Y { get; }


        public GDIConstructionYardCreatedEventData( int x, int y) 
        {
            this.X = x;
            this.Y = y;
        }
    }
}



