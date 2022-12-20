

using ShpFileColorMapper = mike_and_conquer_monogame.gamesprite.ShpFileColorMapper;
using GdiShpFileColorMapper = mike_and_conquer_monogame.gamesprite.GdiShpFileColorMapper;
using Point = Microsoft.Xna.Framework.Point;
using GDIBarracksView = mike_and_conquer_monogame.gameview.GDIBarracksView;

using MikeAndConquerGame =  mike_and_conquer_monogame.main.MikeAndConquerGame;



namespace mike_and_conquer_monogame.gameview.sidebar
{
    public class MinigunnerSidebarIconView : SidebarIconView
    {

        public const string SPRITE_KEY = "MinigunnerIcon";
        public const string SHP_FILE_NAME = "SideBar/e1icnh.tem";
        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();

        public MinigunnerSidebarIconView(Point position) : base(position)
        {
        }

        protected override string GetSpriteKey()
        {
            return SPRITE_KEY;
        }

        protected override bool IsBuilding()
        {
            GDIBarracksView barracksView = GameWorldView.instance.GDIBarracksView;
            return barracksView.IsBuildingMinigunner;
        }

        protected override int PercentBuildCompleted()
        {
            GDIBarracksView barracks = GameWorldView.instance.GDIBarracksView;
            return barracks.PercentMinigunnerBuildComplete;
        }


    }
}
