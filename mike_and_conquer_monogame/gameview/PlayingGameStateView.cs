

using GameTime = Microsoft.Xna.Framework.GameTime;
// using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace mike_and_conquer_monogame.gameview
{
    class PlayingGameStateView : GameStateView
    {

        public override void Update(GameTime gameTime)
        {

        }



        public override  void Draw(GameTime gameTime)
        {
            GameWorldView.instance.Draw(gameTime);
            // foreach (MapTileInstanceView basicMapSquareView in GameWorldView.instance.MapTileInstanceViewList)
            // {
            //     basicMapSquareView.Draw(gameTime, spriteBatch);
            // }
            //
            // TODO:  Consider pulling this code back into this class, from MikeAndConquerGame
//            GameWorldView.instance.GDIBarracksView.Draw(gameTime, spriteBatch);
            // GameWorldView.instance.GdiConstructionYardView.Draw(gameTime,spriteBatch);
//
//            foreach (MinigunnerView nextMinigunnerView in GameWorldView.instance.GdiMinigunnerViewList)
//            {
//                nextMinigunnerView.Draw(gameTime, spriteBatch);
//            }
//
//            foreach (MinigunnerView nextMinigunnerView in GameWorldView.instance.NodMinigunnerViewList)
//            {
//                nextMinigunnerView.Draw(gameTime, spriteBatch);
//            }
//
//            foreach (SandbagView nextSandbagView in GameWorldView.instance.SandbagViewList)
//            {
//                nextSandbagView.Draw(gameTime, spriteBatch);
//            }
//
            // foreach (TerrainView nextTerrainView in GameWorldView.instance.terrainViewList)
            // {
            //     nextTerrainView.DrawFull(gameTime, spriteBatch);
            // }
//
//            MikeAndConquerGame.instance.unitSelectionBox.Draw(gameTime, spriteBatch);


        }
    }
}
