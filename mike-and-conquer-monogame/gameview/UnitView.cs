using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mike_and_conquer.gameview;
using mike_and_conquer_monogame.gameview;
using mike_and_conquer_simulation.events;

namespace mike_and_conquer_monogame.main
{
    public abstract class UnitView
    {
        public int XInWorldCoordinates { get; set; }
        public int YInWorldCoordinates { get; set; }

        public int UnitId { get; set; }

        public bool Selected { get; set; }


        public String type;

        public Color color;

        protected UnitSize unitSize;

        protected PlannedPathView plannedPathView;


        public UnitSize UnitSize
        {
            get { return unitSize; }
        }

        protected Point selectionCursorOffset;
        public Point SelectionCursorOffset
        {
            get { return selectionCursorOffset; }
        }

        public void CreatePlannedPathView(List<PathStep> pathStepList)
        {
            plannedPathView = new PlannedPathView(pathStepList);

        }

        public void RemovePlannedPathStepView(PathStep pathStep)
        {
            plannedPathView.RemoveFromPlannedPath(pathStep.X,pathStep.Y);
        }


        internal Rectangle CreateClickDetectionRectangle()
        {

            int unitWidth = 12;
            int unitHeight = 12;

            int x = (int)(XInWorldCoordinates - (unitWidth / 2));
            int y = (int)(YInWorldCoordinates - unitHeight) + (int)(1);


            // TODO: Is this a memory leak?
            // Thinking not, as it's just a struct with two values and helper functions
            // As opposed to something consumes resources on the graphics card?
            // It doesn't have a Dispose method
            Rectangle rectangle = new Rectangle(x, y, unitWidth, unitHeight);
            return rectangle;
        }


        public bool ContainsPoint(int mouseX, int mouseY)
        {
            Rectangle clickDetectionRectangle = CreateClickDetectionRectangle();
            return clickDetectionRectangle.Contains(new Point(mouseX, mouseY));
        }


        internal abstract void DrawShadowOnly(GameTime gameTime, SpriteBatch spriteBatch);
        internal abstract void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch);

        internal abstract void Update(GameTime gameTime);



    }
}
