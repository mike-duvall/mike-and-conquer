
namespace mike_and_conquer_monogame.rest.domain
{
    class RestUIOptions
    {
        public float MapZoomLevel { get;  }
        public bool DrawShroud { get;  }

        public RestUIOptions(bool drawShroud, float mapZoomLevel)
        {
            this.DrawShroud = drawShroud;
            this.MapZoomLevel = mapZoomLevel;


        }

    }
}
