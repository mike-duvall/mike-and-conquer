

using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using ShpFileColorMapper = mike_and_conquer_monogame.gamesprite.ShpFileColorMapper;
using GdiShpFileColorMapper = mike_and_conquer_monogame.gamesprite.GdiShpFileColorMapper;
using Point = Microsoft.Xna.Framework.Point;
using GameTime = Microsoft.Xna.Framework.GameTime;


namespace mike_and_conquer_monogame.gameview.sidebar
{
    public class BarracksSidebarIconView : SidebarIconView
    {

        public const string SPRITE_KEY = "BarracksSidebarIcon";
        public const string SHP_FILE_NAME = "SideBar/pyleicnh.tem";
        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();

        public BarracksSidebarIconView(Point position) : base(position)
        {
        }

        protected override string GetSpriteKey()
        {
            return SPRITE_KEY;
        }

        protected override bool IsBuilding()
        {
            // TODO:  Make GDIConstructionYard and Barrackas implement Buildable interface?
            // With methods for IsBulding() and PercentBuildCompleted() ?
            return GameWorldView.instance.GDIConstructionYardView.IsBuildingBarracks;
        }


        protected override int PercentBuildCompleted()
        {
            return GameWorldView.instance.GDIConstructionYardView.PercentBarracksBuildComplete;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime,spriteBatch);
            if (GameWorldView.instance.GDIConstructionYardView.IsBarracksReadyToPlace)
            {
                readyOverlay.Draw(gameTime, spriteBatch);
            }

        }


    }
}
