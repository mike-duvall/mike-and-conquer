using System.Collections.Generic;
using mike_and_conquer_monogame.main;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Color = Microsoft.Xna.Framework.Color;

namespace mike_and_conquer_monogame.util
{
    public class TextureUtil
    {
        public static Texture2D CopyTexture(Texture2D sourceTexture)
        {
            Color[] sourceTexturePixelData = new Color[sourceTexture.Width * sourceTexture.Height];
            sourceTexture.GetData(sourceTexturePixelData);

            Texture2D copyTexture = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, sourceTexture.Width,
                sourceTexture.Height);

            Color[] copyTexturePixelData = new Color[copyTexture.Width * copyTexture.Height];
            copyTexture.GetData(copyTexturePixelData);

            int index = 0;
            foreach (Color color in sourceTexturePixelData)
            {
                copyTexturePixelData[index] = color;
                index++;
            }

            copyTexture.SetData(copyTexturePixelData);

            return copyTexture;
        }

        public static Texture2D UpdateShadowPixelsToTransparent(Texture2D texture, List<int> shadowIndexList)
        {
            Color[] texturePixelData = new Color[texture.Width * texture.Height];
            texture.GetData(texturePixelData);

            foreach (int shadowIndex in shadowIndexList)
            {
                texturePixelData[shadowIndex] = Color.Transparent;
            }

            texture.SetData(texturePixelData);

            return texture;
        }


        public static Texture2D CreateSpriteBorderRectangleTexture(Color color, int width, int height)
        {

            string borderRectangleName = "BorderRectangle-Color-R-" + color.R + "-G-" + color.G + "-B-" + color.B +
                                         "-width-" + width + "-height-" + height;

            Texture2D rectangleTexture = MikeAndConquerGame.instance.SpriteSheet.GetTextureForKey(borderRectangleName);

            if (rectangleTexture == null)
            {

                rectangleTexture = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, width, height);
                Color[] data = new Color[rectangleTexture.Width * rectangleTexture.Height];
                FillHorizontalLine(data, rectangleTexture.Width, rectangleTexture.Height, 0, color);
                FillHorizontalLine(data, rectangleTexture.Width, rectangleTexture.Height, rectangleTexture.Height - 1, color);
                FillVerticalLine(data, rectangleTexture.Width, rectangleTexture.Height, 0, color);
                FillVerticalLine(data, rectangleTexture.Width, rectangleTexture.Height, rectangleTexture.Width - 1, color);

                int centerX = (rectangleTexture.Width / 2);
                int centerY = (rectangleTexture.Height / 2);

                // Check how this works for even sized sprites with true center

                int centerOffset = (centerY * rectangleTexture.Width) + centerX;
                data[centerOffset] = Color.Red;

                rectangleTexture.SetData(data);

                MikeAndConquerGame.instance.SpriteSheet.SetTextureForKey(borderRectangleName, rectangleTexture);
            }

            return rectangleTexture;
        }


        public static Texture2D CreateSpriteCenterRectangleTexture(Color color, int width, int height)
        {

            string borderRectangleName = "CenterRectangle-Color-R-" + color.R + "-G-" + color.G + "-B-" + color.B +
                                         "-width-" + width + "-height-" + height;

            Texture2D rectangleTexture = MikeAndConquerGame.instance.SpriteSheet.GetTextureForKey(borderRectangleName);

            if (rectangleTexture == null)
            {

                rectangleTexture = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, width, height);
                Color[] data = new Color[rectangleTexture.Width * rectangleTexture.Height];

                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = color;
                }

                rectangleTexture.SetData(data);

                MikeAndConquerGame.instance.SpriteSheet.SetTextureForKey(borderRectangleName, rectangleTexture);
            }

            return rectangleTexture;
        }



        private static void FillHorizontalLine(Color[] data, int width, int height, int lineIndex, Color color)
        {
            int beginIndex = width * lineIndex;
            for (int i = beginIndex; i < (beginIndex + width); ++i)
            {
                data[i] = color;
            }
        }

        // private static void FillHorizontalLineSub(Color[] data, int width, int height, int lineIndex, Color color, int subStart, int subEnd)
        // {
        //     int length = subEnd - subStart;
        //     int beginIndex = width * lineIndex;
        //     for (int i = beginIndex + subStart; i < (beginIndex + subStart + length); ++i)
        //     {
        //         data[i] = color;
        //     }
        // }



        private static void FillVerticalLine(Color[] data, int width, int height, int lineIndex, Color color)
        {
            int beginIndex = lineIndex;
            for (int i = beginIndex; i < (width * height); i += width)
            {
                data[i] = color;
            }
        }




    }
}
