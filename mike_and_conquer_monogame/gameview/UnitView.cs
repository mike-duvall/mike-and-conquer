using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mike_and_conquer_monogame.gamesprite;
using mike_and_conquer_simulation.events;
using SharpDX.Direct3D9;

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


        protected ClickDetectionRectangle clickDetectionRectangle;

        protected bool showClickDetectionRectangle = false;

        private readonly int clickDetectionRectangleYOffset;
        private readonly int clickDetectionRectangleXOffset;


        public Color color;

        protected UnitSize unitSize;

        protected PlannedPathView plannedPathView;

        // TODO:  Consider if UnitState needs to be a class, with state variables, such as attack target, destination, etc
        public enum UnitState { IDLE, MOVING, FIRING };

        protected UnitSprite unitSprite;


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


        protected UnitView(
            int unitId,
            int xInWorldCoordinates,
            int yInWorldCoordinates,
            int unitWidth,
            int unitHeight,
            int maxHealth,
            int health,
            int clickDetectionRectangleXOffset,
            int clickDetectionRectangleYOffset)
        {
            this.UnitId = unitId;
            this.XInWorldCoordinates = xInWorldCoordinates;
            this.YInWorldCoordinates = yInWorldCoordinates;
            this.MaxHealth = maxHealth;
            this.Health = health;
            this.unitSize = new UnitSize(unitWidth, unitHeight);

            this.clickDetectionRectangleXOffset = clickDetectionRectangleXOffset;
            this.clickDetectionRectangleYOffset = clickDetectionRectangleYOffset;

            clickDetectionRectangle = new ClickDetectionRectangle(
                this.XInWorldCoordinates + clickDetectionRectangleXOffset,
                this.YInWorldCoordinates + clickDetectionRectangleYOffset,
                this.unitSize.Width,
                this.unitSize.Height);

        }



        internal Rectangle CreateClickDetectionRectangle()
        {
            return clickDetectionRectangle.GetRectangle();

        }


        public bool ContainsPoint(int mouseX, int mouseY)
        {
            Rectangle clickDetectionRectangle = CreateClickDetectionRectangle();
            return clickDetectionRectangle.Contains(new Point(mouseX, mouseY));
        }


        internal abstract void DrawShadowOnly(GameTime gameTime, SpriteBatch spriteBatch);
        internal abstract void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch);

        internal abstract void UpdateInternal(GameTime gameTime);

        internal void Update(GameTime gameTime)
        {
            clickDetectionRectangle.Update(
                gameTime,
                XInWorldCoordinates + clickDetectionRectangleXOffset,
                YInWorldCoordinates + clickDetectionRectangleYOffset);
            UpdateInternal(gameTime);

        }


    }
}
