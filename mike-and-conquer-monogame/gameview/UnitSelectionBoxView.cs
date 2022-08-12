
using mike_and_conquer_monogame.main;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Color = Microsoft.Xna.Framework.Color;

using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using SpriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects;


namespace mike_and_conquer.gameview
{

    public class UnitSelectionBoxView
    {

        private UnitSelectionBox unitSelectionBox;

        private Texture2D unitSelectionBoxTexture = null;
        float defaultScale = 1;

        public UnitSelectionBoxView(UnitSelectionBox unitSelectionBox)
        {
            this.unitSelectionBox = unitSelectionBox;
        }

        internal void FillHorizontalLine(Color[] data, int width, int height, int lineIndex, Color color)
        {
            int beginIndex = width * lineIndex;
            for (int i = beginIndex; i < (beginIndex + width); ++i)
            {
                data[i] = color;
            }
        }

        internal void FillVerticalLine(Color[] data, int width, int height, int lineIndex, Color color)
        {
            int beginIndex = lineIndex;
            for (int i = beginIndex; i < (width * height); i += width)
            {
                data[i] = color;
            }
        }


        void UpdateBoundingRectangle()
        {

            int width = unitSelectionBox.SelectionBoxRectangle.Right - unitSelectionBox.SelectionBoxRectangle.Left;
            int height = unitSelectionBox.SelectionBoxRectangle.Bottom - unitSelectionBox.SelectionBoxRectangle.Top;

            if (width < 1) width = 1;
            if (height < 1) height = 1;
            if (unitSelectionBoxTexture != null)
            {
                unitSelectionBoxTexture.Dispose();
            }

            unitSelectionBoxTexture = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, width, height);
            Color[] data = new Color[width * height];
            FillHorizontalLine(data, width, height, 0, Color.White);
            FillHorizontalLine(data, width, height, height - 1, Color.White);
            FillVerticalLine(data, width, height, 0, Color.White);
            FillVerticalLine(data, width, height, width - 1, Color.White);
//            DrawRedDotInCenter(width, height, data);
            unitSelectionBoxTexture.SetData(data);
        }

        private void DrawRedDotInCenter(int width, int height, Color[] data)
        {
            int centerX = width / 2;
            int centerY = height / 2;
            int centerOffset = (centerY * width) + centerX;
            data[centerOffset] = Color.Red;
        }


        internal void Draw( SpriteBatch spriteBatch)
        {
            if (unitSelectionBox.IsDragSelectHappening)
            {
                UpdateBoundingRectangle();
                Vector2 origin = new Vector2(0, 0);
                spriteBatch.Draw(unitSelectionBoxTexture, unitSelectionBox.Position, null, Color.White, 0f, origin, defaultScale,
                    SpriteEffects.None, SpriteSortLayers.UNIT_SELECTION_BOX_DEPTH);
            }

        }

    }





}
