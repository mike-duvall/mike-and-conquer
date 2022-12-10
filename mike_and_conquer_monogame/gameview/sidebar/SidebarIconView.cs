

// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;
// using mike_and_conquer.gamesprite;
// using mike_and_conquer.main;
// using mike_and_conquer.openra;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using Point = Microsoft.Xna.Framework.Point;
using Color = Microsoft.Xna.Framework.Color;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;

using MikeAndConquerGame = mike_and_conquer_monogame.main.MikeAndConquerGame;
using ImmutablePalette = mike_and_conquer_monogame.openra.ImmutablePalette;
using SidebarBuildIconSprite =  mike_and_conquer_monogame.gamesprite.SidebarBuildIconSprite;

namespace mike_and_conquer_monogame.gameview.sidebar
{
    public abstract class SidebarIconView
    {

        protected SidebarBuildIconSprite sidebarBuildIconSprite;
        // protected ReadyOverlay readyOverlay;


        private Point position;


        protected abstract string GetSpriteKey();

        public SidebarIconView(Point position)
        {
            // TODO:  At some point, instead of rendering this as real color texture
            // and handling the mapping of the unit build countdown timer in the code
            // Consider creating separate shader for the sidebar to handle that
            this.position = position;
            Texture2D textureInPaletteValues =
                MikeAndConquerGame.instance.SpriteSheet.GetUnitFramesForShpFile(GetSpriteKey())[0].Texture;
            Texture2D textureInRealColorValues = RenderPaletteTextureAsRealColors(textureInPaletteValues);
            sidebarBuildIconSprite =
                new SidebarBuildIconSprite(
                    textureInRealColorValues,
                    MikeAndConquerGame.instance.SpriteSheet.GetUnitFramesForShpFile(GetSpriteKey())[0].FrameData);
            
            // readyOverlay = new ReadyOverlay();

        }

        private Texture2D RenderPaletteTextureAsRealColors(Texture2D textureInPaletteValues)
        {
            int[] remap = { };

            ImmutablePalette palette = new ImmutablePalette(MikeAndConquerGame.CONTENT_DIRECTORY_PREFIX + "temperat.pal", remap);

            Texture2D textureInRealColorValues = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, textureInPaletteValues.Width, textureInPaletteValues.Height);
            int numPixels = textureInRealColorValues.Width * textureInRealColorValues.Height;


            Color[] paletteTexturePixelData = new Color[numPixels];
            textureInPaletteValues.GetData(paletteTexturePixelData);

            Color[] realColorTexturePixelData = new Color[numPixels];

            for (int i = 0; i < numPixels; i++)
            {
                float paletteIndexAsFloat = paletteTexturePixelData[i].R;
                int paletteIndex = (int) paletteIndexAsFloat;
                uint mappedColor = palette[paletteIndex];
                System.Drawing.Color systemColor = System.Drawing.Color.FromArgb((int)mappedColor);
                Color xnaColor = new Color(systemColor.R, systemColor.G, systemColor.B, systemColor.A);

//                    Color xnaColor = new Color(paletteIndex, 0, 0, 255);
                realColorTexturePixelData[i] = xnaColor;
            }

            textureInRealColorValues.SetData(realColorTexturePixelData);
            return textureInRealColorValues;


        }

        protected abstract bool IsBuilding();
        protected abstract int PercentBuildCompleted();

        // This code for updating the buildIcon with percent progress shading
        // Manually sets and restore GraphicsDevice.renderTarget, so it needs to happen in the "Update" 
        // part of the loop, rather than the "Draw" part.  I initial put it in Draw and it didn't work
        public void Update(GameTime gameTime)
        {
        
            if (IsBuilding())
            {
                sidebarBuildIconSprite.isBuilding = true;
                sidebarBuildIconSprite.SetPercentBuildComplete(PercentBuildCompleted());
            }
            else
            {
                 sidebarBuildIconSprite.isBuilding = false;
            }
        }



        public Point GetPosition()
        {
            return position;
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            sidebarBuildIconSprite.Draw(gameTime, spriteBatch, new Vector2(position.X, position.Y));
//            readyOverlay.Draw(gameTime,spriteBatch);
        }

    }
}
