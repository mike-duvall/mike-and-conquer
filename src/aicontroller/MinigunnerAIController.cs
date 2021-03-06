﻿
using GameTime = Microsoft.Xna.Framework.GameTime;
using System.Collections.Generic;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gameworld;



namespace mike_and_conquer.aicontroller
{
    public class MinigunnerAIController
    {
        private Minigunner myMinigunner;
        private Minigunner currentAttackTarget;

        private bool enemyStateIsSleeping;

        private static readonly int ENEMY_SLEEP_COUNTDOWN_TIMER_INITIAL_VALUE = 10;
        private int enemySleepCountdownTimer;


        public MinigunnerAIController(Minigunner minigunner)
        {
            this.myMinigunner = minigunner;
            this.enemyStateIsSleeping = true;
            this.enemySleepCountdownTimer = ENEMY_SLEEP_COUNTDOWN_TIMER_INITIAL_VALUE;

        }


        private Minigunner FindFirstNonDeadGdiMinigunner()
        {
            List<Minigunner> gdiMinigunners = (GameWorld.instance.GDIMinigunnerList);

            foreach (Minigunner nextMinigunner in gdiMinigunners)
            {
                if (nextMinigunner.Health > 0)
                {
                    return nextMinigunner;
                }
            }

            return null;

        }


        public void Update(GameTime gameTime)
        {
            if (!enemyStateIsSleeping)
            {

                if (myMinigunner.currentCommand == Minigunner.Command.NONE)
                {
                    Minigunner foundMinigunner = FindFirstNonDeadGdiMinigunner();
                    if (foundMinigunner != null)
                    {
                        myMinigunner.OrderToMoveToAndAttackEnemyUnit(foundMinigunner);
                    }

                }
            }
            else
            {
                enemySleepCountdownTimer--;
                if (enemySleepCountdownTimer <= 0)
                {
                    enemyStateIsSleeping = false;
                    currentAttackTarget = FindFirstNonDeadGdiMinigunner();
                    if (currentAttackTarget == null)
                    {
                        enemyStateIsSleeping = true;
                        enemySleepCountdownTimer = ENEMY_SLEEP_COUNTDOWN_TIMER_INITIAL_VALUE;
                    }

                }

            }

        }




    }
}
