using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mike_and_conquer_simulation.events;

namespace mike_and_conquer_monogame.gameview
{
    public class PlannedPathView
    {
        private List<PlannedPathStepView> plannedPathStepViews;



        public PlannedPathView(List<PathStep> pathStepList)
        {
            plannedPathStepViews = new List<PlannedPathStepView>();
            foreach (PathStep pathStep in pathStepList)
            {
                PlannedPathStepView plannedPathStepView = new PlannedPathStepView(pathStep.X, pathStep.Y);
                plannedPathStepViews.Add(plannedPathStepView);
            }



        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (PlannedPathStepView plannedPathStepView in plannedPathStepViews)
            {
                plannedPathStepView.Draw(spriteBatch);
            }
        }

        public void RemoveFromPlannedPath(int xInWorldCoordinates, int yInWorldCoordinates)
        {
            int xInMapTileCoordinates = xInWorldCoordinates / 24;
            int yInMapTileCoordinates = yInWorldCoordinates / 24;

            PlannedPathStepView plannedPathStepViewToRemove = null;

            foreach (PlannedPathStepView plannedPathStepView in plannedPathStepViews)
            {
                if (plannedPathStepView.X == xInMapTileCoordinates &&
                    plannedPathStepView.Y == yInMapTileCoordinates)
                {
                    plannedPathStepViewToRemove = plannedPathStepView;
                    break;
                }
            }

            if (plannedPathStepViewToRemove != null)
            {
                plannedPathStepViews.Remove(plannedPathStepViewToRemove);
            }
            
        }
    }
}
