using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using mike_and_conquer_simulation.main;
using mike_and_conquer_simulation.pathfinding;
// using mike_and_conquer.gameevent;
// using mike_and_conquer.gameworld.humancontroller;
// using mike_and_conquer.main;
// using mike_and_conquer.pathfinding;
// using AsyncGameEvent = mike_and_conquer.gameevent.AsyncGameEvent;
// using CreateGDIMinigunnerGameEvent = mike_and_conquer.gameevent.CreateGDIMinigunnerGameEvent;
// using GetGDIMinigunnerByIdGameEvent = mike_and_conquer.gameevent.GetGDIMinigunnerByIdGameEvent;
// using CreateNodMinigunnerGameEvent = mike_and_conquer.gameevent.CreateNodMinigunnerGameEvent;
// using GetNodMinigunnerByIdGameEvent = mike_and_conquer.gameevent.GetNodMinigunnerByIdGameEvent;
// using ResetGameGameEvent = mike_and_conquer.gameevent.ResetGameGameEvent;
// using GetCurrentGameStateGameEvent = mike_and_conquer.gameevent.GetCurrentGameStateGameEvent;
// using CreateSandbagGameEvent = mike_and_conquer.gameevent.CreateSandbagGameEvent;


using Exception = System.Exception;

using FileStream = System.IO.FileStream;
using FileMode = System.IO.FileMode;


using Point = System.Drawing.Point;
// using Vector2 = System.Numerics.Vector2;


namespace mike_and_conquer_simulation.gameworld

{
    internal class GameWorld
    {

        public static int MAP_TILE_WIDTH = 24;
        public static int MAP_TILE_HEIGHT = 24;
        public static int MAP_TILE_WIDTH_IN_LEPTONS = 256;


        private GDIPlayer gdiPlayer;
        // private NodPlayer nodPlayer;
        //
        // public List<Sandbag> sandbagList;
        // public List<NodTurret> nodTurretList;
         public List<TerrainItem> terrainItemList;
        // public List<Projectile120mm> projectile120MmList;
        //
        // private List<GameHistoryEvent> gameHistoryEventList;
        //
        // public List<GameHistoryEvent> GameHistoryEventList
        // {
        //     get { return this.gameHistoryEventList; }
        // }

        // Tiles are 24 pixels wide and are also 256 leptons wide
        // Since MnC uses pixels as "world coordinates" or "world units"
        // WorldUnitsPerLepton is 24 / 256, which is:  0.09375
        // This is needed because Cnc code documents things like
        // movement speeds in terms of leptons
        // public static double WorldUnitsPerLepton = 24.0 / 256.0;
        public static double WorldUnitsPerLepton = (float) MAP_TILE_WIDTH / (float) MAP_TILE_WIDTH_IN_LEPTONS;

        // public List<Minigunner> GDIMinigunnerList
        // {
        //     get { return gdiPlayer.GdiMinigunnerList; }
        // }
        //
        // public List<Minigunner> NodMinigunnerList
        // {
        //     get { return nodPlayer.NodMinigunnerList; }
        // }
        //
        // public GDIBarracks GDIBarracks
        // {
        //     get { return gdiPlayer.GDIBarracks; }
        // }

        // public GDIConstructionYard GDIConstructionYard
        // {
        //     get { return gdiPlayer.GDIConstructionYard; }
        // }
        //
        // public MCV MCV
        // {
        //     get { return gdiPlayer.MCV; }
        // }
        //
        public NavigationGraph navigationGraph;

        // private List<AsyncGameEvent> gameEvents;

        public GameMap gameMap;

        // public UnitSelectionBox unitSelectionBox;

        public static GameWorld instance;

        public GameWorld()
        {

            // gdiPlayer = new GDIPlayer(new HumanPlayerController());
            // nodPlayer = new NodPlayer(new NodAIPlayerController());
            // sandbagList = new List<Sandbag>();
            // nodTurretList = new List<NodTurret>();
            terrainItemList = new List<TerrainItem>();
            // projectile120MmList = new List<Projectile120mm>();
            //
            // gameEvents = new List<AsyncGameEvent>();

            // unitSelectionBox = new UnitSelectionBox();

            // gameHistoryEventList = new List<GameHistoryEvent>();
            GameWorld.instance = this;
        }

        // public GameState HandleReset()
        // {
        //
        //     // gdiPlayer.HandleReset();
        //     // nodPlayer.HandleReset();
        //     // sandbagList.Clear();
        //     // nodTurretList.Clear();
        //     // projectile120MmList.Clear();
        //     // nodTurretList.Clear();
        //     // projectile120MmList.Clear();
        //     gameMap.Reset();
        //     // InitializeNavigationGraph();
        //     return new PlayingGameState();
        // }


        // public void PublisheGameHistoryEvent(String evenType, int unitId)
        // {
        //     // double totalMillis = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
        //     // Int32 int32InMillies = (Int32) (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
        //
        //     ulong unixTimestamp = (ulong) (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
        //     GameHistoryEvent gameHistoryEvent = new GameHistoryEvent(evenType, unitId, unixTimestamp);
        //     gameHistoryEventList.Add(gameHistoryEvent);
        // }

        public void InitializeDefaultMap()
        {
            LoadMap();
             navigationGraph = new NavigationGraph(this.gameMap.numColumns, this.gameMap.numRows);
             InitializeNavigationGraph();
        }

        // public void InitializeTestMap(int[,] obstacleArray)
        // {
        //     gameMap = new GameMap(obstacleArray);
        //     navigationGraph = new NavigationGraph(this.gameMap.numColumns, this.gameMap.numRows);
        //     InitializeNavigationGraph();
        // }


        private void LoadMap()
        {

            // Stream inputStream = new FileStream(MikeAndConquerGame.CONTENT_DIRECTORY_PREFIX + "scg01ea.bin", FileMode.Open);
            string location = Assembly.GetEntryAssembly().Location;
            int locationOfDll = location.IndexOf("mike-and-conquer-monogame.dll");
            location = location.Remove(locationOfDll - 1);

            Assembly assembly =  Assembly.GetEntryAssembly();
            string filePath = location + "\\Content\\" + "scg01ea.bin";
            Stream inputStream = new FileStream( filePath, FileMode.Open);


            //  (Starting at 0x13CC in the file)

            // TODO Should eventually read this out of the INI file instead of hard coding
            int startX = 35;
            int startY = 39;
            int endX = 61;
            int endY = 61;

            gameMap = new GameMap(inputStream, startX, startY, endX, endY);

            // TODO:  Eventually create a data file
            // which has the tile descriptor data(specifically, the blocked tiles)
            // and read all of this from a file, rather than being hard coded
            // in the code
            Dictionary<String, TerrainItemDescriptor> terrainItemDescriptorDictionary = new Dictionary<string, TerrainItemDescriptor>();
            addT01(terrainItemDescriptorDictionary);  
            addT02(terrainItemDescriptorDictionary);  
//            addT05(terrainItemDescriptorDictionary);  // not visible
            addT06(terrainItemDescriptorDictionary);  
            addT07(terrainItemDescriptorDictionary);  
            addT16(terrainItemDescriptorDictionary);  
            addTC01(terrainItemDescriptorDictionary); 
            addTC02(terrainItemDescriptorDictionary); 
            addTC04(terrainItemDescriptorDictionary); 
            addTC05(terrainItemDescriptorDictionary);

            SortedDictionary<int, string> terrainMap = new SortedDictionary<int, string>();

            terrainMap.Add(3303, "T01.tem");
            terrainMap.Add(2988, "T01.tem");
            terrainMap.Add(2991, "T01.tem");
            terrainMap.Add(3121, "T01.tem");
            terrainMap.Add(2936, "T01.tem");
            terrainMap.Add(2861, "T02.tem");
            terrainMap.Add(3111, "T02.tem");
            terrainMap.Add(3369, "T06.tem");
            terrainMap.Add(3496, "T06.tem");
            terrainMap.Add(2860, "T07.tem");
            terrainMap.Add(3245, "T16.tem");
            terrainMap.Add(2937, "T16.tem");
            terrainMap.Add(2555, "TC01.tem");
            terrainMap.Add(3246, "TC01.tem");
            terrainMap.Add(2666, "TC01.tem");
            terrainMap.Add(3052, "TC02.tem");
            terrainMap.Add(3056, "TC02.tem");
            terrainMap.Add(2871, "TC02.tem");
            terrainMap.Add(2544, "TC02.tem");
            terrainMap.Add(2794, "TC04.tem");
            terrainMap.Add(2938, "TC04.tem");
            terrainMap.Add(2605, "TC05.tem");  
            terrainMap.Add(2680, "TC05.tem");


            float layerDepthOffset = 0.0f;
            foreach (int cellnumber in terrainMap.Keys)
            {
                Point point = ConvertCellNumberToTopLeftWorldCoordinates(cellnumber);
                if (point.X >= 0 && point.Y >= 0)
                {
                    String terrainItemType = terrainMap[cellnumber];
                    TerrainItemDescriptor terrainItemDescriptor = terrainItemDescriptorDictionary[terrainItemType];
                    TerrainItem terrainItem = new TerrainItem(point.X, point.Y, terrainItemDescriptor, layerDepthOffset);
                    layerDepthOffset = layerDepthOffset + 0.01f;
                    terrainItemList.Add(terrainItem);

                    List<MapTileInstance> blockeMapTileInstances = terrainItem.GetBlockedMapTileInstances();
                    foreach(MapTileInstance blockedMapTileInstance in blockeMapTileInstances)
                    {
                        if (blockedMapTileInstance != null)
                        {
                            blockedMapTileInstance.IsBlockingTerrain = true;
                        }

                    }

                }
            }


            inputStream.Close();


        }

        public void Update()
        {
            if (gdiPlayer != null)
            {
                gdiPlayer.Update();
            }

        }

        private static void addT01(Dictionary<string, TerrainItemDescriptor> terrainItemDescriptorDictionary)
        {
            List<Point> blockMapTileOffsets = new List<Point>();
            blockMapTileOffsets.Add(new Point(0, 1));
            TerrainItemDescriptor descriptor = new TerrainItemDescriptor(
                "T01.tem",
                48,
                48,
                blockMapTileOffsets);
            terrainItemDescriptorDictionary.Add(descriptor.TerrainItemType, descriptor);
        }

        private static void addT02(Dictionary<string, TerrainItemDescriptor> terrainItemDescriptorDictionary)
        {
            List<Point> blockMapTileOffsets = new List<Point>();
            blockMapTileOffsets.Add(new Point(0, 1));
            TerrainItemDescriptor descriptor = new TerrainItemDescriptor(
                "T02.tem",
                48,
                48,
                blockMapTileOffsets);
            terrainItemDescriptorDictionary.Add(descriptor.TerrainItemType, descriptor);
        }

        private static void addT05(Dictionary<string, TerrainItemDescriptor> terrainItemDescriptorDictionary)
        {
            List<Point> blockMapTileOffsets = new List<Point>();
            blockMapTileOffsets.Add(new Point(0, 1));
            TerrainItemDescriptor descriptor = new TerrainItemDescriptor(
                "T05.tem",
                48,
                48,
                blockMapTileOffsets);
            terrainItemDescriptorDictionary.Add(descriptor.TerrainItemType, descriptor);
        }

        private static void addT06(Dictionary<string, TerrainItemDescriptor> terrainItemDescriptorDictionary)
        {
            List<Point> blockMapTileOffsets = new List<Point>();
            blockMapTileOffsets.Add(new Point(0, 1));
            TerrainItemDescriptor descriptor = new TerrainItemDescriptor(
                "T06.tem",
                48,
                48,
                blockMapTileOffsets);
            terrainItemDescriptorDictionary.Add(descriptor.TerrainItemType, descriptor);
        }


        private static void addT07(Dictionary<string, TerrainItemDescriptor> terrainItemDescriptorDictionary)
        {
            List<Point> blockMapTileOffsets = new List<Point>();
            blockMapTileOffsets.Add(new Point(0, 1));
            TerrainItemDescriptor descriptor = new TerrainItemDescriptor(
                "T07.tem",
                48,
                48,
                blockMapTileOffsets);
            terrainItemDescriptorDictionary.Add(descriptor.TerrainItemType, descriptor);
        }

        private static void addT16(Dictionary<string, TerrainItemDescriptor> terrainItemDescriptorDictionary)
        {
            List<Point> blockMapTileOffsets = new List<Point>();
            blockMapTileOffsets.Add(new Point(0, 1));
            TerrainItemDescriptor descriptor = new TerrainItemDescriptor(
                "T16.tem",
                48,
                48,
                blockMapTileOffsets);
            terrainItemDescriptorDictionary.Add(descriptor.TerrainItemType, descriptor);
        }


        private static void addTC01(Dictionary<string, TerrainItemDescriptor> terrainItemDescriptorDictionary)
        {
            List<Point> blockMapTileOffsets = new List<Point>();
            blockMapTileOffsets.Add(new Point(0, 1));
            blockMapTileOffsets.Add(new Point(1, 1));
            TerrainItemDescriptor descriptor = new TerrainItemDescriptor(
                "TC01.tem",
                72,
                48,
                blockMapTileOffsets);
            terrainItemDescriptorDictionary.Add(descriptor.TerrainItemType, descriptor);
        }

        private static void addTC04(Dictionary<string, TerrainItemDescriptor> terrainItemDescriptorDictionary)
        {
            List<Point> blockMapTileOffsets = new List<Point>();
            blockMapTileOffsets.Add(new Point(0, 1));
            blockMapTileOffsets.Add(new Point(1, 1));
            blockMapTileOffsets.Add(new Point(2, 1));
            blockMapTileOffsets.Add(new Point(0, 2));

            TerrainItemDescriptor descriptor = new TerrainItemDescriptor(
                "TC04.tem",
                96,
                72,
                blockMapTileOffsets);
            terrainItemDescriptorDictionary.Add(descriptor.TerrainItemType, descriptor);
        }


        private static void addTC05(Dictionary<string, TerrainItemDescriptor> terrainItemDescriptorDictionary)
        {
            List<Point> blockMapTileOffsets = new List<Point>();
            blockMapTileOffsets.Add(new Point(0, 1));
            blockMapTileOffsets.Add(new Point(1, 1));
            blockMapTileOffsets.Add(new Point(2, 1));
            blockMapTileOffsets.Add(new Point(1, 2));
            blockMapTileOffsets.Add(new Point(2, 2));


            TerrainItemDescriptor descriptor = new TerrainItemDescriptor(
                "TC05.tem",
                96,
                72,
                blockMapTileOffsets);
            terrainItemDescriptorDictionary.Add(descriptor.TerrainItemType, descriptor);
        }


        private static void addTC02(Dictionary<string, TerrainItemDescriptor> terrainItemDescriptorDictionary)
        {
            List<Point> blockMapTileOffsets = new List<Point>();
            blockMapTileOffsets.Add(new Point(1, 0));
            blockMapTileOffsets.Add(new Point(0, 1));
            blockMapTileOffsets.Add(new Point(1, 1));


            TerrainItemDescriptor descriptor = new TerrainItemDescriptor(
                "TC02.tem",
                72,
                48,
                blockMapTileOffsets);
            terrainItemDescriptorDictionary.Add(descriptor.TerrainItemType, descriptor);
        }



        private Point ConvertCellNumberToTopLeftWorldCoordinates(int cellnumber)
        {
            // TODO: Eventually update this formula to use non hard coded map size
            int quotient = cellnumber / 64;
            int remainder = cellnumber % 64;

            int mapTileX = remainder - 35;
            int mapTileY = quotient - 39;

            int mapX = mapTileX * GameWorld.MAP_TILE_WIDTH;
            int mapY = mapTileY * GameWorld.MAP_TILE_HEIGHT;

            return new Point(mapX, mapY);
        }

        // internal Minigunner GetGdiMinigunner(int id)
        // {
        //     return gdiPlayer.GetMinigunner(id);
        // }
        //
        // public NodTurret GetNodTurret(int id)
        // {
        //     foreach(NodTurret nodTurret in nodTurretList)
        //     {
        //         if (nodTurret.id == id)
        //         {
        //             return nodTurret;
        //         }
        //     }
        //
        //     return null;
        // }


        // internal Minigunner GetNodMinigunner(int id)
        // {
        //     return nodPlayer.GetNodMinigunner(id);
        // }
        //
        //
        // internal Minigunner GetGdiOrNodMinigunner(int id)
        // {
        //     Minigunner foundMinigunner = null;
        //     foundMinigunner = GetGdiMinigunner(id);
        //     if (foundMinigunner == null)
        //     {
        //         foundMinigunner = GetNodMinigunner(id);
        //     }
        //     return foundMinigunner;
        // }


        // void DeslectAllUnits()
        // {
        //     gdiPlayer.DeslectAllUnits();
        //
        // }
        //
        // internal void SelectSingleGDIUnit(Minigunner minigunner)
        // {
        //     DeslectAllUnits();
        //     minigunner.selected = true;
        // }
        //
        // internal void SelectMCV(MCV mcv)
        // {
        //     DeslectAllUnits();
        //     mcv.selected = true;
        // }



        // public void Update(GameTime gameTime)
        // {
        //     // gdiPlayer.Update(gameTime);
        //     // nodPlayer.Update(gameTime);
        //     //
        //     // foreach (NodTurret nodTurret in nodTurretList)
        //     // {
        //     //     nodTurret.Update(gameTime);
        //     // }
        //     //
        //     //
        //     //
        //     // List<Projectile120mm> projectile120mmsToRemove = new List<Projectile120mm>();
        //     // foreach (Projectile120mm projectile120mm in projectile120MmList)
        //     // {
        //     //     bool shouldRemove = projectile120mm.Update(gameTime);
        //     //     if (shouldRemove)
        //     //     {
        //     //         projectile120mmsToRemove.Add(projectile120mm);
        //     //     }
        //     // }
        //     //
        //     //
        //     // foreach (Projectile120mm projectile120mm in projectile120mmsToRemove)
        //     // {
        //     //     MikeAndConquerGame.instance.RemoveProjectile120mmWithId(projectile120mm.id);
        //     // }
        //
        //
        // }


        public class BadMinigunnerLocationException : Exception
        {
            public BadMinigunnerLocationException(Point badLocation)
                : base("Bad minigunner location.  x:" + badLocation.X + ", y:" + badLocation.Y)
            {
            }
        }


        private void AssertIsValidMinigunnerPosition(Point positionInWorldCoordinates)
        {
            foreach (MapTileInstance nexBasicMapSquare in this.gameMap.MapTileInstanceList)
            {
                if (nexBasicMapSquare.ContainsPoint(positionInWorldCoordinates) &&
                    nexBasicMapSquare.IsBlockingTerrain)
                {
                    throw new BadMinigunnerLocationException(positionInWorldCoordinates);
                }
            }
        }




        public Minigunner CreateMinigunner(int xInWorldCoordinates, int yInWorldCoordinates)
        {

            return gdiPlayer.CreateMinigunner(xInWorldCoordinates, yInWorldCoordinates);
        }

        public Jeep CreateJeep(int xInWorldCoordinates, int yInWorldCoordinates)
        {
            return gdiPlayer.CreateJeep(xInWorldCoordinates, yInWorldCoordinates);
        }

        public MCV CreateMCV(int xInWorldCoordinates, int yInWorldCoordinates)
        {
            return gdiPlayer.CreateMCV(xInWorldCoordinates, yInWorldCoordinates);
        }

        public Unit FindUnitWithUnitId(int unitId)
        {
            return gdiPlayer.FindUnitWithUnitId(unitId);
        }

        public void SetGDIPlayerController(PlayerController playerController)
        {
            gdiPlayer = new GDIPlayer(playerController);
        }




        // public Minigunner AddGdiMinigunner(Point positionInWorldCoordinates)
        // {
        //     
        //     AssertIsValidMinigunnerPosition(positionInWorldCoordinates);
        //
        //     Minigunner newMinigunner = new Minigunner(positionInWorldCoordinates.X, positionInWorldCoordinates.Y, this );
        //     gdiPlayer.AddMinigunner(newMinigunner);
        //     // gdiMinigunnerList.Add(newMinigunner);
        //     return newMinigunner;
        // }
        //
        //
        // public MCV AddMCV(Point positionInWorldCoordinates)
        // {
        //     // mcv = new MCV(positionInWorldCoordinates.X, positionInWorldCoordinates.Y, this);
        //     // return mcv;
        //
        //     return gdiPlayer.AddMCV(positionInWorldCoordinates);
        // }


        // public Minigunner AddNodMinigunner(Point positionInWorldCoordinates, bool aiIsOn)
        // {
        //
        //     AssertIsValidMinigunnerPosition(positionInWorldCoordinates);
        //
        //
        //     Minigunner newMinigunner = new Minigunner(positionInWorldCoordinates.X, positionInWorldCoordinates.Y, this);
        //     
        //     // // TODO:  In future, don't couple Nod having to be AI controlled enemy
        //     // if (aiIsOn)
        //     // {
        //     //     MinigunnerAIController minigunnerAIController = new MinigunnerAIController(newMinigunner);
        //     //     nodMinigunnerAIControllerList.Add(minigunnerAIController);
        //     // }
        //     
        //     // return newMinigunner;
        //     return nodPlayer.AddNodMinigunner(newMinigunner, aiIsOn);
        // }
        //
        //
        // public GDIBarracks AddGDIBarracks(MapTileLocation mapTileLocation)
        // {
        //     // // TODO Might want to check if one already exists and throw error if so
        //     // gdiBarracks = new GDIBarracks(positionInWorldCoordinates.X, positionInWorldCoordinates.Y);
        //     // return gdiBarracks;
        //     return gdiPlayer.AddGDIBarracks(mapTileLocation);
        // }
        //
        // public GDIConstructionYard AddGDIConstructionYard(MapTileLocation mapTileLocation)
        // {
        //     // // TODO Might want to check if one already exists and throw error if so
        //     // gdiConstructionYard = new GDIConstructionYard(positionInWorldCoordinates.X, positionInWorldCoordinates.Y);
        //     // return gdiConstructionYard;
        //
        //     return gdiPlayer.AddGDIConstructionYard(mapTileLocation);
        // }

        // public GameState ProcessGameEvents()
        // {
        //     GameState newGameState = null;
        //
        //     lock (gameEvents)
        //     {
        //         foreach (AsyncGameEvent nextGameEvent in gameEvents)
        //         {
        //             GameState returnedGameState = nextGameEvent.Process();
        //             if (returnedGameState != null && newGameState == null)
        //             {
        //                 newGameState = returnedGameState;
        //             }
        //         }
        //         gameEvents.Clear();
        //     }
        //
        //     return newGameState;
        //
        // }


        // public Minigunner CreateGDIMinigunnerViaEvent(Point minigunnerPosition)
        // {
        //     CreateGDIMinigunnerGameEvent gameEvent = new CreateGDIMinigunnerGameEvent(minigunnerPosition);
        //     lock (gameEvents)
        //     {
        //         gameEvents.Add(gameEvent);
        //     }
        //
        //     Minigunner gdiMinigunner = gameEvent.GetMinigunner();
        //     return gdiMinigunner;
        //
        // }
        //
        // public MCV CreateMCVViaEvent(Point position)
        // {
        //     CreateMCVGameEvent gameEvent = new CreateMCVGameEvent(position);
        //     lock (gameEvents)
        //     {
        //         gameEvents.Add(gameEvent);
        //     }
        //
        //     MCV mcv = gameEvent.GetMCV();
        //     return mcv;
        //
        // }



        // public void SetGDIMinigunnerHealthViaEvent(int minigunnerId, int newHealth)
        // {
        //     SetGDIMinigunnerHealthGameEvent gameEvent = new SetGDIMinigunnerHealthGameEvent(minigunnerId, newHealth);
        //     lock (gameEvents)
        //     {
        //         gameEvents.Add(gameEvent);
        //     }
        //
        // }
        //
        //
        //
        //
        // public Sandbag CreateSandbagViaEvent(int x, int y, int index)
        // {
        //     CreateSandbagGameEvent gameEvent = new CreateSandbagGameEvent(x, y, index);
        //
        //     lock (gameEvents)
        //     {
        //         gameEvents.Add(gameEvent);
        //     }
        //
        //     Sandbag sandbag = gameEvent.GetSandbag();
        //     return sandbag;
        //
        // }
        //
        //
        // public NodTurret CreateNodTurretViaEvent(int x, int y, float direction, int type)
        // {
        //     CreateNodTurretGameEvent gameEvent = new CreateNodTurretGameEvent(x, y, direction, type);
        //
        //     lock (gameEvents)
        //     {
        //         gameEvents.Add(gameEvent);
        //     }
        //
        //     NodTurret nodTurret = gameEvent.GetNodTurret();
        //     return nodTurret;
        //
        // }
        //
        //
        //
        // public Minigunner GetGDIMinigunnerByIdViaEvent(int id)
        // {
        //     GetGDIMinigunnerByIdGameEvent gameEvent = new GetGDIMinigunnerByIdGameEvent(id);
        //
        //     lock (gameEvents)
        //     {
        //         gameEvents.Add(gameEvent);
        //     }
        //
        //     Minigunner gdiMinigunner = gameEvent.GetMinigunner();
        //     return gdiMinigunner;
        // }
        //
        //
        // public GameOptions GetGameOptionViaEvent()
        // {
        //     GameOptionsGameEvent gameEvent = new GameOptionsGameEvent();
        //
        //     lock (gameEvents)
        //     {
        //         gameEvents.Add(gameEvent);
        //     }
        //     
        //     GameOptions gameOptions = gameEvent.GetGameOptions();
        //     return gameOptions;
        // }



        // public NodTurret GetNodTurretByIdViaEvent(int id)
        // {
        //     GetNodTurretByIdGameEvent gameEvent = new GetNodTurretByIdGameEvent(id);
        //
        //     lock (gameEvents)
        //     {
        //         gameEvents.Add(gameEvent);
        //     }
        //
        //     NodTurret nodTurret = gameEvent.GetNodTurret();
        //     return nodTurret;
        // }
        //
        //
        //
        // public Minigunner CreateNodMinigunnerViaEvent(Point positionInWorldCoordinates, bool aiIsOn)
        // {
        //     CreateNodMinigunnerGameEvent gameEvent = new CreateNodMinigunnerGameEvent(positionInWorldCoordinates, aiIsOn);
        //     lock (gameEvents)
        //     {
        //         gameEvents.Add(gameEvent);
        //     }
        //
        //     Minigunner minigunner = gameEvent.GetMinigunner();
        //     return minigunner;
        //
        // }
        //
        //
        // public Minigunner GetNodMinigunnerByIdViaEvent(int id)
        // {
        //     GetNodMinigunnerByIdGameEvent gameEvent = new GetNodMinigunnerByIdGameEvent(id);
        //
        //     lock (gameEvents)
        //     {
        //         gameEvents.Add(gameEvent);
        //     }
        //
        //     Minigunner gdiMinigunner = gameEvent.GetMinigunner();
        //     return gdiMinigunner;
        // }
        //
        //
        // public void ResetGameViaEvent(bool drawShroud, float initialMapZoom, GameOptions.GameSpeed gameSpeed)
        // {
        //     ResetGameGameEvent gameEvent = new ResetGameGameEvent(drawShroud, initialMapZoom, gameSpeed);
        //
        //     lock (gameEvents)
        //     {
        //         gameEvents.Add(gameEvent);
        //     }
        //
        // }
        //
        // public MemoryStream GetScreenshotViaEvent()
        // {
        //     GetScreenshotGameEvent gameEvent = new GetScreenshotGameEvent();
        //
        //     lock (gameEvents)
        //     {
        //         gameEvents.Add(gameEvent);
        //     }
        //
        //     MemoryStream memoryStream = gameEvent.GetMemoryStream();
        //     return memoryStream;
        // }
        //
        //
        // public GameState GetCurrentGameStateViaEvent()
        // {
        //     GetCurrentGameStateGameEvent gameEvent = new GetCurrentGameStateGameEvent();
        //
        //     lock (gameEvents)
        //     {
        //         gameEvents.Add(gameEvent);
        //     }
        //
        //     return gameEvent.GetGameState();
        // }

        internal MapTileInstance FindMapTileInstance(MapTileLocation mapTileLocation)
        {

            foreach (MapTileInstance nextBasicMapSquare in this.gameMap.MapTileInstanceList)
            {
                if (nextBasicMapSquare.ContainsPoint(mapTileLocation.WorldCoordinatesAsPoint))
                {
                    return nextBasicMapSquare;
                }
            }
            throw new Exception("Unable to find MapTileInstance at coordinates, x:" + mapTileLocation.WorldCoordinatesAsPoint.X  + ", y:" +  mapTileLocation.WorldCoordinatesAsPoint.Y);

        }

        public  MapTileInstance FindMapTileInstanceAllowNull(MapTileLocation mapTileLocation)
        {

            foreach (MapTileInstance nextBasicMapSquare in this.gameMap.MapTileInstanceList)
            {
                if (nextBasicMapSquare.ContainsPoint(mapTileLocation.WorldCoordinatesAsPoint))
                {
                    return nextBasicMapSquare;
                }
            }

            return null;

        }



        public void InitializeNavigationGraph()
        {

            navigationGraph.Reset();

            // foreach (Sandbag nextSandbag in sandbagList)
            // {
            //     MapTileLocation sandbagMapTileLocation = nextSandbag.MapTileLocation;
            //     navigationGraph.MakeNodeBlockingNode(
            //         sandbagMapTileLocation.WorldMapTileCoordinatesAsPoint.Y,
            //         sandbagMapTileLocation.WorldMapTileCoordinatesAsPoint.Y);
            // }


            foreach (MapTileInstance nextMapTileInstance in this.gameMap.MapTileInstanceList)
            {
                if (nextMapTileInstance.IsBlockingTerrain)
                {

                    MapTileLocation mapTileLocation = nextMapTileInstance.MapTileLocation;
                    
                    navigationGraph.MakeNodeBlockingNode(
                        mapTileLocation.WorldMapTileCoordinatesAsPoint.X,
                        mapTileLocation.WorldMapTileCoordinatesAsPoint.Y);
                }
            }

            navigationGraph.RebuildAdajencyGraph();

        }

        public Point ConvertMapSquareIndexToWorldCoordinate(int index)
        {
            int numColumns = navigationGraph.width;
            Point point = new Point();
            int row = index / numColumns;
            int column = index - (row * numColumns);
        
            point.X = (column * GameWorld.MAP_TILE_WIDTH) + (GameWorld.MAP_TILE_WIDTH / 2);
            point.Y = (row * GameWorld.MAP_TILE_HEIGHT) + (GameWorld.MAP_TILE_HEIGHT / 2);
            return point;
        }




        public bool IsPointOnMap(Point pointInWorldCoordinates)
        {
            int largestXValue = (this.gameMap.numColumns * GameWorld.MAP_TILE_WIDTH) -1;
            int largestYValue = (this.gameMap.numRows * GameWorld.MAP_TILE_HEIGHT) -1;

            return (pointInWorldCoordinates.X >= 0 &&
                    pointInWorldCoordinates.X < largestXValue &&
                    pointInWorldCoordinates.Y >= 0 &&
                    pointInWorldCoordinates.Y < largestYValue
                );

        }

//        public void MakeMapSquareVisible(Point positionInWorldCoordinates, MapTileInstance.MapTileVisibility visibility)
//        {
//            MapTileInstance mapTileInstance = this.FindMapTileInstance(positionInWorldCoordinates.X, positionInWorldCoordinates.Y);
//            mapTileInstance.Visibility = visibility;
//        }


//         public bool IsPointAdjacentToConstructionYardAndClearForBuilding(Point pointInWordlCoordinates)
//         {
//             MapTileLocation mapTileLocation = MapTileLocation.CreateFromWorldCoordinates(pointInWordlCoordinates.X, pointInWordlCoordinates.Y);
// //            MapTileInstance mapTileInstance = this.FindMapTileInstanceAllowNull(pointInWordlCoordinates.X, pointInWordlCoordinates.Y);
//             MapTileInstance mapTileInstance = this.FindMapTileInstanceAllowNull(mapTileLocation);
//             if (mapTileInstance == null)
//             {
//                 return false;
//             }
//
//             return IsMapTileInstanceAdjacentToConstructionYard(mapTileInstance) &&
//                    IsMapTileInstanceClearForBuilding(mapTileInstance);
//
//         }

        // private bool IsMapTileInstanceClearForBuilding(MapTileInstance mapTileInstance)
        // {
        //     return !mapTileInstance.IsBlockingTerrain &&
        //            !GDIConstructionYard.ContainsPoint(mapTileInstance.MapTileLocation.WorldCoordinatesAsPoint);
        //
        // }
        //
        //
        // private bool IsRelativeMapTileInstanceAdjacentToConstructionsYard(MapTileInstance mapTileInstance,
        //     TILE_LOCATION tileLocation)
        // {
        //     MapTileInstance adjacentTile = FindAdjacentMapTileInstance(mapTileInstance, tileLocation);
        //     if (adjacentTile != null && GDIConstructionYard.ContainsPoint(adjacentTile.MapTileLocation.WorldCoordinatesAsPoint))
        //     {
        //         return true;
        //     }
        //
        //     return false;
        //
        // }


        public enum TILE_LOCATION
        {
            WEST,
            NORTH_WEST,
            NORTH,
            NORTH_EAST,
            EAST,
            SOUTH_EAST,
            SOUTH,
            SOUTH_WEST
        }



        // private bool IsMapTileInstanceAdjacentToConstructionYard(MapTileInstance mapTileInstance)
        // {
        //
        //     if (IsRelativeMapTileInstanceAdjacentToConstructionsYard(mapTileInstance, TILE_LOCATION.WEST))
        //     {
        //         return true;
        //     }
        //
        //     if (IsRelativeMapTileInstanceAdjacentToConstructionsYard(mapTileInstance, TILE_LOCATION.NORTH_WEST))
        //     {
        //         return true;
        //     }
        //
        //     if (IsRelativeMapTileInstanceAdjacentToConstructionsYard(mapTileInstance, TILE_LOCATION.NORTH))
        //     {
        //         return true;
        //     }
        //
        //     if (IsRelativeMapTileInstanceAdjacentToConstructionsYard(mapTileInstance, TILE_LOCATION.NORTH_EAST))
        //     {
        //         return true;
        //     }
        //
        //     if (IsRelativeMapTileInstanceAdjacentToConstructionsYard(mapTileInstance, TILE_LOCATION.EAST))
        //     {
        //         return true;
        //     }
        //
        //     if (IsRelativeMapTileInstanceAdjacentToConstructionsYard(mapTileInstance, TILE_LOCATION.SOUTH_EAST))
        //     {
        //         return true;
        //     }
        //
        //     if (IsRelativeMapTileInstanceAdjacentToConstructionsYard(mapTileInstance, TILE_LOCATION.SOUTH))
        //     {
        //         return true;
        //     }
        //
        //     if (IsRelativeMapTileInstanceAdjacentToConstructionsYard(mapTileInstance, TILE_LOCATION.SOUTH_WEST))
        //     {
        //         return true;
        //     }
        //
        //     return false;
        //
        // }



        private MapTileInstance FindAdjacentMapTileInstance(MapTileInstance mapTileInstance,TILE_LOCATION tileLocation)
        {

            MapTileLocation adjacentMapTileLocation =
                mapTileInstance.MapTileLocation.CreateAdjacentMapTileLocation(tileLocation);



            MapTileInstance fouMapTileInstance =
                this.FindMapTileInstanceAllowNull(adjacentMapTileLocation);


            return fouMapTileInstance;

        }


        public void StartScenario(PlayerController playerController)
        {

            terrainItemList.Clear();

            gdiPlayer = new GDIPlayer(playerController);
            InitializeDefaultMap();
        }

        //         public  bool IsValidMoveDestination(Point pointInWorldCoordinates)
        //         {
        //             bool isValidMoveDestination = true;
        // //            MapTileInstance clickedMapTileInstance =
        // //                FindMapTileInstanceAllowNull(pointInWorldCoordinates.X, pointInWorldCoordinates.Y);
        //
        //             MapTileInstance clickedMapTileInstance =
        //                 FindMapTileInstanceAllowNull(
        //                     MapTileLocation.CreateFromWorldCoordinates(pointInWorldCoordinates.X, pointInWorldCoordinates.Y));
        //
        //
        //             if (clickedMapTileInstance == null)
        //             {
        //                 isValidMoveDestination = false;
        //             }
        //             else if (clickedMapTileInstance.IsBlockingTerrain)
        //             {
        //                 isValidMoveDestination = false;
        //             }
        //
        //
        //             foreach (Sandbag nextSandbag in MikeAndConquerGame.instance.gameWorld.sandbagList)
        //             {
        //
        //                 if (nextSandbag.ContainsPoint(pointInWorldCoordinates))
        //                 {
        //                     isValidMoveDestination = false;
        //                 }
        //             }
        //
        //             if (GDIConstructionYard != null)
        //             {
        //                 if (GDIConstructionYard.ContainsPoint(pointInWorldCoordinates))
        //                 {
        //                     isValidMoveDestination = false;
        //                 }
        //             }
        //
        //             return isValidMoveDestination;
        //         }
        //
        //         public bool IsPointOverEnemy(Point pointInWorldCoordinates)
        //         {
        //             // foreach (Minigunner nextNodMinigunner in nodMinigunnerList)
        //             // {
        //             //     if (nextNodMinigunner.ContainsPoint(pointInWorldCoordinates.X, pointInWorldCoordinates.Y))
        //             //     {
        //             //         return true;
        //             //     }
        //             // }
        //             //
        //             // return false;
        //             //
        //             return nodPlayer.IsPointOverMinigunner(pointInWorldCoordinates);
        //         }
        //
        //         public bool IsPointOverMCV(Point pointInWorldCoordinates)
        //         {
        //
        //             // if (this.mcv != null)
        //             // {
        //             //     if (mcv.ContainsPoint(pointInWorldCoordinates.X, pointInWorldCoordinates.Y))
        //             //     {
        //             //         return true;
        //             //     }
        //             // }
        //             //
        //             // return false;
        //
        //             return gdiPlayer.IsPointOverMCV(pointInWorldCoordinates);
        //         }
        //
        //
        //         public bool IsAMinigunnerSelected()
        //         {
        //             // foreach (Minigunner nextMinigunner in gdiMinigunnerList)
        //             // {
        //             //     if (nextMinigunner.selected)
        //             //     {
        //             //         return true;
        //             //     }
        //             // }
        //             // return false;
        //             return gdiPlayer.IsAMinigunnerSelected();
        //         }
        //
        //         public bool IsAnMCVSelected()
        //         {
        //             // if (mcv != null)
        //             // {
        //             //     return mcv.selected;
        //             // }
        //             //
        //             // return false;
        //             return gdiPlayer.IsAnMCVSelected();
        //         }
        //
        //         public bool IsAnyUnitSelected()
        //         {
        //             return IsAMinigunnerSelected() || IsAnMCVSelected();
        //         }


        // public void RemoveMCV()
        // {
        //     gdiPlayer.RemoveMCV();
        // }
        //
        // public Projectile120mm AddProjectile120mm(GameWorldLocation gameWorldLocation, Minigunner target)
        // {
        //     Projectile120mm projectile120Mm = new Projectile120mm(gameWorldLocation, target);
        //     projectile120MmList.Add(projectile120Mm);
        //     return projectile120Mm;
        // }
        //
        // public void RemoveProjectile120mm(int id)
        // {
        //     foreach (Projectile120mm projectile120Mm in projectile120MmList)
        //     {
        //         if (projectile120Mm.id == id)
        //         {
        //             projectile120MmList.Remove(projectile120Mm);
        //             return;
        //         }
        //     }
        // }
    }
}
