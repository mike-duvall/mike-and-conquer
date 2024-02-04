

// using mike_and_conquer.gameobjects;

using mike_and_conquer_monogame.gamesprite;

namespace mike_and_conquer_monogame.gameview
{
    public class NodMinigunnerView : MinigunnerView
    {

        public const string SPRITE_KEY = "NODMinigunner";

        // TODO:  SHP_FILE_NAME and ShpFileColorMapper don't really belong in this view
        // Views should be agnostic about where the sprite data was loaded from
        public const string SHP_FILE_NAME = "e1.shp";
        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new NodShpFileColorMapper();


        public NodMinigunnerView(int unitId,  int xInWorldCoordinates, int yInWorldCoordinates, int maxHealth, int health)
            : base( unitId, SPRITE_KEY, xInWorldCoordinates, yInWorldCoordinates, maxHealth, health)
        {

        }

    }
}
