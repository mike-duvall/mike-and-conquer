using System;
using System.Collections.Generic;
using mike_and_conquer_monogame.main;
using mike_and_conquer_monogame.openra;

namespace mike_and_conquer_monogame.openralocal
{
    class RAISpriteFrameManager
    {

        private Dictionary<string, openra.IReadOnlyList<ISpriteFrame>> unitSpriteFrameMap;
        private Dictionary<string, ISpriteFrame[]> mapTileSpriteFrameMap;


        public RAISpriteFrameManager()
        {
            unitSpriteFrameMap = new Dictionary<string, openra.IReadOnlyList<ISpriteFrame>>();
            mapTileSpriteFrameMap = new Dictionary<string, ISpriteFrame[]>();

        }

        public void LoadAllTexturesFromShpFile(string shpFileName)
        {
            try
            {
                string filePath = MikeAndConquerGame.CONTENT_DIRECTORY_PREFIX  + shpFileName;
                System.IO.FileStream shpStream = System.IO.File.Open(filePath, System.IO.FileMode.Open,
                    System.IO.FileAccess.Read, System.IO.FileShare.None);
                ShpTDSprite shpTDSprite = new ShpTDSprite(shpStream);
                unitSpriteFrameMap.Add(shpFileName, shpTDSprite.Frames);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                throw e;

            }
        }



        

        public void LoadAllTexturesFromTmpFile(string fileName)
        {
            TmpTDLoader tmpTDLoader = new TmpTDLoader();
            String filePath = MikeAndConquerGame.CONTENT_DIRECTORY_PREFIX + fileName;
            System.IO.FileStream tmpStream = System.IO.File.Open(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None);

            ISpriteFrame[] frames;
            tmpTDLoader.TryParseSprite(tmpStream, out frames);
            mapTileSpriteFrameMap.Add(fileName,frames);
        }

        public openra.IReadOnlyList<ISpriteFrame> GetSpriteFramesForUnit(string shpFileName)
        {
            return unitSpriteFrameMap[shpFileName];
        }

        public ISpriteFrame[] GetSpriteFramesForMapTile(string tmpFileName)
        {
            return mapTileSpriteFrameMap[tmpFileName];
        }

    }
}
