﻿using mike_and_conquer.gameview;
using mike_and_conquer.gameworld;
using GameTime = Microsoft.Xna.Framework.GameTime;


namespace mike_and_conquer.gamestate
{
    class MissionFailedGameState : GameState
    {

        public MissionFailedGameState()
        {

            foreach (MinigunnerView nextMinigunnerView in GameWorldView.instance.GdiMinigunnerViewList)
            {
                nextMinigunnerView.SetAnimate(false);
            }

            foreach (MinigunnerView nextMinigunnerView in GameWorldView.instance.NodMinigunnerViewList)
            {
                nextMinigunnerView.SetAnimate(false);
            }

        }

        public override string GetName()
        {
            return "Mission Failed";
        }

        public override GameState Update(GameTime gameTime)
        {
            GameState nextGameState = GameWorld.instance.ProcessGameEvents();
            if (nextGameState != null)
            {
                return nextGameState;
            }
            else
            {
                return this;
            }
        }



    }
}
