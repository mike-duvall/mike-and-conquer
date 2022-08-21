using System.Collections.Generic;
using mike_and_conquer_monogame.main;

namespace mike_and_conquer_monogame.gamesprite
{
    public class ShadowMapper
    {

        private Dictionary<int, int> unitsShadowMap;
        private Dictionary<int, int> sidebarBuildShadowMap;


        private Dictionary<int, int> mapTileShadowMap13;
        private Dictionary<int, int> mapTileShadowMap14;
        private Dictionary<int, int> mapTileShadowMap15;
        private Dictionary<int, int> mapTileShadowMap16;

        public ShadowMapper()
        {
            LoadUnitsShadowMap();
            LoadSidebarMap();
            LoadMapTileShadowMaps();
        }


        private void LoadMapTileShadowMaps()
        {
            // See here:  https://forums.cncnet.org/topic/2253-mrf-creation-tool-finished/?tab=comments#comment-19465
            // "The shroud, in ?shadow.mrf, also uses this, to link its four filters (0, 1, 2, 3) to the indices 16, 15, 14 and 13 respectively. If you look at the shroud SHP file, you see it indeed uses these indices as its different shroud gradations in the fade to black."
            // So:
            // 0 maps to 16
            // 1 maps to 15
            // 2 maps to 14
            // 3 maps to 13
            // Where 0 through three is the offset of the order of how these appear in the MRF file,
            // meaning, we, the order they appear in the file is:
            // 16,15,14,13

            System.IO.FileStream mrfFileStream = System.IO.File.Open("Content\\tshadow.mrf", System.IO.FileMode.Open,
                System.IO.FileAccess.Read, System.IO.FileShare.None);
            for (int i = 0; i < 256; i++)
                mrfFileStream.ReadByte();


            mapTileShadowMap16 = new Dictionary<int, int>();
            for (int i = 0; i < 256; i++)
            {
                int byte1 = mrfFileStream.ReadByte();
                mapTileShadowMap16.Add(i, byte1);
            }

            mapTileShadowMap15 = new Dictionary<int, int>();
            for (int i = 0; i < 256; i++)
            {
                int byte1 = mrfFileStream.ReadByte();
                mapTileShadowMap15.Add(i, byte1);
            }
            mapTileShadowMap14 = new Dictionary<int, int>();
            for (int i = 0; i < 256; i++)
            {
                int byte1 = mrfFileStream.ReadByte();
                mapTileShadowMap14.Add(i, byte1);
            }

            mapTileShadowMap13 = new Dictionary<int, int>();
            for (int i = 0; i < 256; i++)
            {
                int byte1 = mrfFileStream.ReadByte();
                mapTileShadowMap13.Add(i, byte1);
            }

            mrfFileStream.Close();
        }

        private void LoadUnitsShadowMap()
        {
            unitsShadowMap = new Dictionary<int, int>();
            System.IO.FileStream mrfFileStream = System.IO.File.Open(MikeAndConquerGame.CONTENT_DIRECTORY_PREFIX + "tunits.mrf", System.IO.FileMode.Open,
                System.IO.FileAccess.Read, System.IO.FileShare.None);
            for (int i = 0; i < 256; i++)
                mrfFileStream.ReadByte();

            for (int i = 0; i < 256; i++)
            {
                int byte1 = mrfFileStream.ReadByte();
                unitsShadowMap.Add(i, byte1);
            }

            mrfFileStream.Close();
        }
         

        private void LoadSidebarMap()
        {
            sidebarBuildShadowMap = new Dictionary<int, int>();
            System.IO.FileStream mrfFileStream = System.IO.File.Open(MikeAndConquerGame.CONTENT_DIRECTORY_PREFIX + "tclock.mrf", System.IO.FileMode.Open,
                System.IO.FileAccess.Read, System.IO.FileShare.None);
            for (int i = 0; i < 256; i++)
                mrfFileStream.ReadByte();

            for (int i = 256; i < 512; i++)
            {
                int byte1 = mrfFileStream.ReadByte();
                sidebarBuildShadowMap.Add(i - 256, byte1);
            }

            mrfFileStream.Close();
        }


        internal int MapUnitsShadowPaletteIndex(int paletteIndex)
        {
            int mappedValue;
            unitsShadowMap.TryGetValue(paletteIndex, out mappedValue);
            return mappedValue;
        }


        internal int MapSidebarBuildPaletteIndex(int paletteIndex)
        {
            int mappedValue;
            sidebarBuildShadowMap.TryGetValue(paletteIndex, out mappedValue);
            return mappedValue;
        }

        internal int MapMapTile13PaletteIndex(int paletteIndex)
        {
            int mappedValue;
            mapTileShadowMap13.TryGetValue(paletteIndex, out mappedValue);
            return mappedValue;
        }

        internal int MapMapTile14PaletteIndex(int paletteIndex)
        {
            int mappedValue;
            mapTileShadowMap14.TryGetValue(paletteIndex, out mappedValue);
            return mappedValue;
        }

        internal int MapMapTile15PaletteIndex(int paletteIndex)
        {
            int mappedValue;
            mapTileShadowMap15.TryGetValue(paletteIndex, out mappedValue);
            return mappedValue;
        }

        internal int MapMapTile16PaletteIndex(int paletteIndex)
        {
            int mappedValue;
            mapTileShadowMap16.TryGetValue(paletteIndex, out mappedValue);
            return mappedValue;
        }


    }
}
