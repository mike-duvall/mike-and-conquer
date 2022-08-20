using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mike_and_conquer_monogame.main;
using mike_and_conquer_monogame.openra;

//using System.Linq;


namespace mike_and_conquer_monogame.gamesprite
{
    public class SpriteSheet
    {

        private Dictionary<string, List<UnitFrame>> unitFrameMap;
        private Dictionary<string, List<MapTileFrame>> mapTileFrameMap;
        private Dictionary<string, Texture2D> individualTextureMap;

        public SpriteSheet()
        {
            unitFrameMap = new Dictionary<string, List<UnitFrame>>();
            mapTileFrameMap = new Dictionary<string, List<MapTileFrame>>();
            individualTextureMap = new Dictionary<string, Texture2D>();
        }

        public List<UnitFrame> GetUnitFramesForShpFile(string shpFileName)
        {
            try
            {
                return unitFrameMap[shpFileName];
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                System.Diagnostics.Debug.WriteLine("Key:" + shpFileName);
                throw e;
            }
        }


        public List<MapTileFrame> GetMapTileFrameForTmpFile(string tmpFileName)
        {
            return mapTileFrameMap[tmpFileName];
        }

        public Texture2D GetTextureForKey(string key)
        {
            if (individualTextureMap.ContainsKey(key))
            {
                return individualTextureMap[key];
            }
            else
            {
                return null;
            }

        }

        public void SetTextureForKey(string key, Texture2D texture)
        {
            if (individualTextureMap.ContainsKey(key))
            {
                individualTextureMap.Remove(key);
            }
            individualTextureMap.Add(key, texture);
        }


        public void LoadUnitFramesFromSpriteFrames(string spriteKey, openra.IReadOnlyList<ISpriteFrame> spriteFrameList, ShpFileColorMapper shpFileColorMapper)
        {

            int[] remap = { };
            ImmutablePalette palette = new ImmutablePalette(MikeAndConquerGame.CONTENT_DIRECTORY_PREFIX + "temperat.pal", remap);
            List<UnitFrame> unitFrameList = new List<UnitFrame>();


            foreach (ISpriteFrame frame in spriteFrameList)
            {
                byte[] frameData = frame.Data;
                List<int> shadowIndexList = new List<int>();

                Texture2D texture2D = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, frame.Size.Width, frame.Size.Height);
                int numPixels = texture2D.Width * texture2D.Height;
                Color[] texturePixelData = new Color[numPixels];

                for (int i = 0; i < numPixels; i++)
                {
                    int basePaletteIndex = frameData[i];
                    int mappedPaletteIndex = shpFileColorMapper.MapColorIndex(basePaletteIndex);
                    uint mappedColor = palette[mappedPaletteIndex];
                    
                    System.Drawing.Color systemColor = System.Drawing.Color.FromArgb((int)mappedColor);
                    Color xnaColorMapped = new Color(systemColor.R, systemColor.G, systemColor.B, systemColor.A);

                    // TODO - Investigate just setting alpha based on the hard coded palette value for black
                    // Defacto, appears that sometimes Alpha is 255 and sometimes it is 0
                    Color xnaColor = new Color(mappedPaletteIndex, 0, 0, xnaColorMapped.A);
                    texturePixelData[i] = xnaColor;

                    if (mappedPaletteIndex == 4)
                    {
                        shadowIndexList.Add(i);
                    }
                }

                texture2D.SetData(texturePixelData);
                UnitFrame unitFrame = new UnitFrame(texture2D, frameData, shadowIndexList);
                unitFrameList.Add(unitFrame);
            }

            unitFrameMap.Add(spriteKey, unitFrameList);

        }


        // TODO:  Revisit why we need to separate methods.  Can they at least share code?
        public void LoadMapTileFramesFromSpriteFrames(string tmpFileName, ISpriteFrame[] spriteFrameArray)
        {

            int[] remap = { };

            List<MapTileFrame> mapTileFrameList = new List<MapTileFrame>();

            ImmutablePalette palette = new ImmutablePalette(MikeAndConquerGame.CONTENT_DIRECTORY_PREFIX + "temperat.pal", remap);

            foreach (ISpriteFrame frame in spriteFrameArray)
            {
                byte[] frameData = frame.Data;

                if (frameData.Length == 0)
                {
                    MapTileFrame nullMapTileFrame = new MapTileFrame(null);
                    mapTileFrameList.Add(nullMapTileFrame);
                    continue;
                }
                Texture2D texture2D = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, frame.Size.Width, frame.Size.Height);
                int numPixels = texture2D.Width * texture2D.Height;
                Color[] texturePixelData = new Color[numPixels];

                for (int i = 0; i < numPixels; i++)
                {
                    int paletteIndex = frameData[i];
                    // TODO:  Revisit this hard coding of alpha to 255
                    Color xnaColor = new Color(paletteIndex, 0, 0, 255);
                    texturePixelData[i] = xnaColor;
                }

                texture2D.SetData(texturePixelData);
                MapTileFrame mapTileFrame = new MapTileFrame(texture2D);
                mapTileFrameList.Add(mapTileFrame);
            }

            mapTileFrameMap.Add(tmpFileName, mapTileFrameList);
        }

        public void LoadSingleTextureFromFile(string key, string fileName)
        {
            Texture2D texture2D = MikeAndConquerGame.instance.Content.Load<Texture2D>(fileName);
            individualTextureMap.Add(key, texture2D);
        }



    }
}
