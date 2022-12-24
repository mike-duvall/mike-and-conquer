namespace mike_and_conquer_simulation.events
{
    public class GDIBarracksPlacedEventData
    {
        public const string EventType = "GDIBarracksPlaced";


        public int X { get; }
        public int Y { get; }


        public GDIBarracksPlacedEventData( int x, int y) 
        {
            this.X = x;
            this.Y = y;
        }
    }
}



