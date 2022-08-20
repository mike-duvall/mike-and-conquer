




namespace mike_and_conquer_monogame.gameview
{
    public class MapTileShroudMapping
    {



        public MapTileInstanceView.MapTileVisibility east;
        public MapTileInstanceView.MapTileVisibility southEast;
        public MapTileInstanceView.MapTileVisibility south;
        public MapTileInstanceView.MapTileVisibility southWest;
        public MapTileInstanceView.MapTileVisibility west;
        public MapTileInstanceView.MapTileVisibility northWest;
        public MapTileInstanceView.MapTileVisibility north;
        public MapTileInstanceView.MapTileVisibility northEast;

        public int shroudTileIndex;
        

        public MapTileShroudMapping(
            MapTileInstanceView.MapTileVisibility east,
            MapTileInstanceView.MapTileVisibility southEast,
            MapTileInstanceView.MapTileVisibility south,
            MapTileInstanceView.MapTileVisibility southWest,
            MapTileInstanceView.MapTileVisibility west,
            MapTileInstanceView.MapTileVisibility northWest,
            MapTileInstanceView.MapTileVisibility north,
            MapTileInstanceView.MapTileVisibility northEast,

            int shroudTileIndex)
        {
            this.east = east;
            this.south = south;
            this.west = west;
            this.north = north;
            this.northEast = northEast;
            this.southEast = southEast;
            this.southWest = southWest;
            this.northWest = northWest;
            this.shroudTileIndex = shroudTileIndex;
        }





    }
}