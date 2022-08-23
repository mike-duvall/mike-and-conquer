using System.Collections.Generic;
using System.Linq;
// using mike_and_conquer.gameview;
using Stream = System.IO.Stream;

using BinaryReader = System.IO.BinaryReader;

namespace mike_and_conquer_simulation.gameworld
{
    internal class GameMap
    {

        public const string CLEAR1_SHP = "clear1.tem";

        public const string D04_TEM = "d04.tem";
        public const string D09_TEM = "d09.tem";
        public const string D13_TEM = "d13.tem";
        public const string D15_TEM = "d15.tem";
        public const string D20_TEM = "d20.tem";
        public const string D21_TEM = "d21.tem";
        public const string D23_TEM = "d23.tem";

        public const string P07_TEM = "p07.tem";
        public const string P08_TEM = "p08.tem";

        public const string S09_TEM = "s09.tem";
        public const string S10_TEM = "s10.tem";
        public const string S11_TEM = "s11.tem";
        public const string S12_TEM = "s12.tem";
        public const string S14_TEM = "s14.tem";
        public const string S22_TEM = "s22.tem";
        public const string S29_TEM = "s29.tem";
        public const string S32_TEM = "s32.tem";
        public const string S34_TEM = "s34.tem";
        public const string S35_TEM = "s35.tem";

        public const string SH1_TEM = "sh1.tem";
        public const string SH2_TEM = "sh2.tem";
        public const string SH3_TEM = "sh3.tem";
        public const string SH4_TEM = "sh4.tem";
        public const string SH5_TEM = "sh5.tem";
        public const string SH6_TEM = "sh6.tem";
        public const string SH9_TEM = "sh9.tem";
        public const string SH10_TEM = "sh10.tem";
        public const string SH17_TEM = "sh17.tem";
        public const string SH18_TEM = "sh18.tem";

        public const string W1_TEM = "w1.tem";
        public const string W2_TEM = "w2.tem";


        private List<MapTileInstance> mapTileInstanceList;

        public List<MapTileInstance> MapTileInstanceList
        {
            get { return mapTileInstanceList; }
        }


        private Dictionary<byte, string> mapFileCodeToTextureStringMap = new Dictionary<byte, string>();

        public int numColumns;
        public int numRows;

        private Dictionary<string, int[]> blockingTerrainMap = new Dictionary<string, int[]>();

        private GameMap()
        {
        }

        public GameMap(Stream inputStream, int startX, int startY, int endX, int endY)
        {
            LoadCodeToTextureStringMap();

            List<byte> allBytes = ReadAllBytesFromStream(inputStream);

            numColumns = endX - startX + 1;
            numRows = endY - startY + 1;

            InitializeBlockTerrainMap();

            mapTileInstanceList = new List<MapTileInstance>();

            int x = 0;
            int y = 0;

            int i = 0;
            for (int row = startY; row <= endY; row++)
            {
                for (int column = startX; column <= endX; column++)
                {
                    int offset = CalculateOffset(column, row);
                    string textureKey = ConvertByteToTextureKey(allBytes[offset]);
                    byte imageIndex = CalculateImageIndexForTextureKey(textureKey,allBytes, column, row, offset);
                    bool isBlockingTerrain = IsBlockingTerrain(textureKey, imageIndex);

                    MapTileInstance mapTileInstance =
                        new MapTileInstance(MapTileLocation.CreateFromWorldMapTileCoordinates(x, y), textureKey, imageIndex, isBlockingTerrain);

                    this.MapTileInstanceList.Add(mapTileInstance);

                    x++;

                    bool incrementRow = ((i + 1) % numColumns) == 0;
                    if (incrementRow)
                    {
                        x = 0;
                        y++;
                    }

                    i++;

                }
            }
        }


        // Used for unit tests
        public GameMap(int[,] nodeArray)
        {
            mapTileInstanceList = new List<MapTileInstance>();

            numRows = nodeArray.GetLength(0);
            numColumns = nodeArray.GetLength(1);

            for (int y = 0; y < numRows; y++)
            {
                for (int x = 0; x < numColumns; x++)
                {
                    string dummyTexture = "";
                    byte dummyImageIndex = 0;

                    if (nodeArray[y, x] == 1)
                    {
                        MapTileInstance mapTileInstance =
                            new MapTileInstance(MapTileLocation.CreateFromWorldMapTileCoordinates(x, y), dummyTexture, dummyImageIndex, true);
                        this.MapTileInstanceList.Add(mapTileInstance);
                    }
                    else
                    {
                        MapTileInstance mapTileInstance =
                            new MapTileInstance(MapTileLocation.CreateFromWorldMapTileCoordinates(x, y), dummyTexture, dummyImageIndex, false);
                        this.MapTileInstanceList.Add(mapTileInstance);
                    }
                }
            }
        }


        private byte CalculateImageIndexForTextureKey(string textureKey, List<byte> allBytes, int column, int row, int offset)
        {

            if (textureKey == CLEAR1_SHP)
            {
                return CalculateImageIndexForClear1(column, row);
            }
            else
            {
                return allBytes[offset + 1];
            }

        }

        private List<byte> ReadAllBytesFromStream(Stream inputStream)
        {
            BinaryReader binaryReader = new BinaryReader(inputStream);
            long numBytes = binaryReader.BaseStream.Length;
            List<byte> allBytes = new List<byte>();
            for (int i = 0; i < numBytes; i++)
            {
                byte nextByte = binaryReader.ReadByte();
                allBytes.Add(nextByte);
            }

            return allBytes;
        }

        private bool IsBlockingTerrain(string textureKey, byte imageIndex)
        {
            
            if (blockingTerrainMap.ContainsKey(textureKey))
            {
                int[] blockedImageIndexes = blockingTerrainMap[textureKey];
                if (blockedImageIndexes == null)
                {
                    return true;
                }
                else
                {
                    return blockedImageIndexes.Contains(imageIndex);
                }
            }

            return false;

        }

        private void InitializeBlockTerrainMap()
        {

            blockingTerrainMap.Add(S09_TEM, new int[] {0,1,2,4,5});
            blockingTerrainMap.Add(S10_TEM, null);
            blockingTerrainMap.Add(S11_TEM, null);
            blockingTerrainMap.Add(S12_TEM, null);
            blockingTerrainMap.Add(S14_TEM, null);
            blockingTerrainMap.Add(S22_TEM, null);
            blockingTerrainMap.Add(S29_TEM, null);
            blockingTerrainMap.Add(S32_TEM, null);
            blockingTerrainMap.Add(S34_TEM, null);
            blockingTerrainMap.Add(S35_TEM, null);
            blockingTerrainMap.Add(SH1_TEM, new int[] { 6, 7, 8 });
            blockingTerrainMap.Add(SH2_TEM, new int[] { 3, 4, 5, 6, 7, 8 });
            blockingTerrainMap.Add(SH3_TEM, null);
            blockingTerrainMap.Add(SH4_TEM, null);
            blockingTerrainMap.Add(SH5_TEM, new int[] {  6, 7, 8 });
            blockingTerrainMap.Add(SH6_TEM, new int[] { 6, 7, 8 });
            blockingTerrainMap.Add(SH9_TEM, new int[] { 6 });
            blockingTerrainMap.Add(SH10_TEM, new int[] {0,2,3 });
            blockingTerrainMap.Add(SH17_TEM, null);
            blockingTerrainMap.Add(SH18_TEM, null);
            blockingTerrainMap.Add(W1_TEM, null);
            blockingTerrainMap.Add(W2_TEM, null);
        }


        private byte CalculateImageIndexForClear1(int column, int row)
        {
            return (byte)((column % 4) + ((row % 4) * 4));
        }

        private void LoadCodeToTextureStringMap()
        {
            mapFileCodeToTextureStringMap.Add(0xff, CLEAR1_SHP);

            mapFileCodeToTextureStringMap.Add(0x60, D04_TEM);
            mapFileCodeToTextureStringMap.Add(0x69, D13_TEM);
            mapFileCodeToTextureStringMap.Add(0x70, D20_TEM);
            mapFileCodeToTextureStringMap.Add(0x71, D21_TEM);
            mapFileCodeToTextureStringMap.Add(0x73, D23_TEM);

            mapFileCodeToTextureStringMap.Add(0x49, P07_TEM);
            mapFileCodeToTextureStringMap.Add(0x4A, P08_TEM);

            mapFileCodeToTextureStringMap.Add(0x15, S09_TEM);
            mapFileCodeToTextureStringMap.Add(0x16, S10_TEM);
            mapFileCodeToTextureStringMap.Add(0x17, S11_TEM);
            mapFileCodeToTextureStringMap.Add(0x18, S12_TEM);
            mapFileCodeToTextureStringMap.Add(0x1a, S14_TEM);
            mapFileCodeToTextureStringMap.Add(0x22, S22_TEM);
            mapFileCodeToTextureStringMap.Add(0x29, S29_TEM);


            mapFileCodeToTextureStringMap.Add(0x2c, S32_TEM);
            mapFileCodeToTextureStringMap.Add(0x2e, S34_TEM);
            mapFileCodeToTextureStringMap.Add(0x2f, S35_TEM);


            mapFileCodeToTextureStringMap.Add(0x03, SH1_TEM);
            mapFileCodeToTextureStringMap.Add(0x04, SH2_TEM);
            mapFileCodeToTextureStringMap.Add(0x05, SH3_TEM);

            mapFileCodeToTextureStringMap.Add(0x06, SH4_TEM);

            mapFileCodeToTextureStringMap.Add(0x07, SH5_TEM);
            mapFileCodeToTextureStringMap.Add(0x58, SH6_TEM);
            mapFileCodeToTextureStringMap.Add(0x5b, SH9_TEM);
            mapFileCodeToTextureStringMap.Add(0x5c, SH10_TEM);
            mapFileCodeToTextureStringMap.Add(0x4c, SH17_TEM);
            mapFileCodeToTextureStringMap.Add(0x4d, SH18_TEM);

            mapFileCodeToTextureStringMap.Add(0x01, W1_TEM);
            mapFileCodeToTextureStringMap.Add(0x02, W2_TEM);
        }

        private string ConvertByteToTextureKey(byte inputByte)
        {

            if(mapFileCodeToTextureStringMap.ContainsKey(inputByte))
            {
                string textureKey;
                mapFileCodeToTextureStringMap.TryGetValue(inputByte, out textureKey);
                return textureKey;
            }
            else
            {
                string textureKey;
                mapFileCodeToTextureStringMap.TryGetValue(inputByte, out textureKey);
                return textureKey;
            }

            // TODO: Change to this once we get all tile types registered
            //return mapFileCodeToTextureStringMap[inputByte];
        }

        private int CalculateOffset(int column, int row)
        {
            return (row * 64 * 2) + (column * 2);
        }


        public void Reset()
        {
            foreach (MapTileInstance mapTileInstance in MapTileInstanceList)
            {
                // mapTileInstance.ClearAllMinigunnerSlots();
                mapTileInstance.Visibility = MapTileInstance.MapTileVisibility.NotVisible;
            }

        }
    }


}
