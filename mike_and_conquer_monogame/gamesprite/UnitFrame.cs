using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mike_and_conquer_monogame.main;
using mike_and_conquer_monogame.util;

namespace mike_and_conquer_monogame.gamesprite
{
    public class UnitFrame
    {
        private Texture2D texture;
        private Texture2D shadowOnlyTexture;
        private byte[] frameData;
        private List<int> shadowIndexList;


        public Texture2D Texture
        {
            get { return texture; }
        }

        public Texture2D ShadowOnlyTexture
        {
            get { return shadowOnlyTexture; }
        }

        public List<int> ShadowIndexList
        {
            get { return shadowIndexList; }
        }

        public byte[] FrameData
        {
            get { return frameData; }
        }


        public UnitFrame(Texture2D texture, byte[] frameData, List<int> shadowIndexList)
        {
            this.texture = texture;
            this.frameData = frameData;
            this.shadowIndexList = shadowIndexList;
            InitializeShadowOnlyTexture();
            InitializeNoShadowTexture();
        }

        private void InitializeShadowOnlyTexture()
        {
            shadowOnlyTexture =
                new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, texture.Width, texture.Height);

            Color[] texturePixelData = new Color[shadowOnlyTexture.Width * shadowOnlyTexture.Height];
            shadowOnlyTexture.GetData(texturePixelData);


            foreach (int shadowIndex in shadowIndexList)
            {
                Color xnaColor = ShadowHelper.SHADOW_COLOR;
                texturePixelData[shadowIndex] = xnaColor;
            }

            shadowOnlyTexture.SetData(texturePixelData);

        }

        private void InitializeNoShadowTexture()
        { 
            texture = TextureUtil.UpdateShadowPixelsToTransparent(texture, shadowIndexList);
        }



    }
}
