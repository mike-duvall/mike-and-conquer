using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mike_and_conquer_simulation.events;

namespace mike_and_conquer_monogame.gameview
{
    public abstract class UnitView
    {
        public int XInWorldCoordinates { get; set; }
        public int YInWorldCoordinates { get; set; }

        public int UnitId { get; set; }

        public bool Selected { get; set; }

        public int MaxHealth { get; set; }

        public int Health { get; set; }


        public Color color;

        protected UnitSize unitSize;

        protected PlannedPathView plannedPathView;

        // TODO:  Consider if UnitState needs to be a class, with state variables, such as attack target, destination, etc
        public enum UnitState { IDLE, MOVING, FIRING };


        public UnitState CurrentUnitState
        {
            get;
            set;
        }


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

            int unitWidth = this.unitSize.Width;
            int unitHeight = this.unitSize.Height;

            int x = (int)(XInWorldCoordinates - (unitWidth / 2));
            // int y = (int)(YInWorldCoordinates - unitHeight) + (int)(1);
            int y = (int)(YInWorldCoordinates - (unitWidth / 2));


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
