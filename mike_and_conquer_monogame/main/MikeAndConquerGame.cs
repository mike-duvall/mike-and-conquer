using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mike_and_conquer_monogame.commands;
using mike_and_conquer_monogame.commands.commandbody;
using mike_and_conquer_monogame.commands.ui;
using mike_and_conquer_monogame.eventhandler;
using mike_and_conquer_monogame.gamesprite;
using mike_and_conquer_monogame.gamestate;
using mike_and_conquer_monogame.gameview;
using mike_and_conquer_monogame.humancontroller;
using mike_and_conquer_monogame.openralocal;
using mike_and_conquer_simulation.commands;
using mike_and_conquer_simulation.events;
using mike_and_conquer_simulation.gameworld;
using mike_and_conquer_simulation.main;
using Newtonsoft.Json;

using MemoryStream = System.IO.MemoryStream;
using Form = System.Windows.Forms.Form;


namespace mike_and_conquer_monogame.main
{
    public class MikeAndConquerGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public ILogger logger;

        // public MonogameSimulationStateListener monogameSimulationStateListener = null;
        public List<SimulationStateListener> simulationStateListenerList = null;

        private bool hasScenarioBeenInitialized = false;
        private int mapWidth = -10;
        private int mapHeight = -10;

//        private List<UnitView> unitViewList;

        private Queue<AsyncViewCommand> inputCommandQueue;

        public static MikeAndConquerGame instance;

        public const string CONTENT_DIRECTORY_PREFIX = "Content\\";

        public SpriteSheet SpriteSheet
        {
            get { return spriteSheet; }
        }

        private SpriteSheet spriteSheet;

        // public GameWorld gameWorld;
        private GameWorldView gameWorldView;

        private GameState currentGameState;

        private GameStateView currentGameStateView;

        private RAISpriteFrameManager raiSpriteFrameManager;

        private bool topLevelWindowsHasBeenBroughtToForeground = false;


        public MikeAndConquerGame()
        {

            logger = MainProgram.loggerFactory.CreateLogger<MikeAndConquerGame>();
            logger.LogInformation("Game1() ctor");

            _graphics = new GraphicsDeviceManager(this);

            // unitViewList = new List<UnitView>();
            inputCommandQueue = new Queue<AsyncViewCommand>();


            new GameOptions();

            
             if (GameOptions.instance.IsFullScreen)
             {
                _graphics.IsFullScreen = true;
                _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

                // Having to set HardwareModeSwitch to false to make screenshots work for my tests
                // only on the Thinkpad laptop for some reason
                _graphics.HardwareModeSwitch = false;


                // Form myForm = (Form)Form.FromHandle(this.Window.Handle);
                // myForm.Activate();


            }
            else
            {
                _graphics.IsFullScreen = false;
                // graphics.PreferredBackBufferWidth = 1280;
                // graphics.PreferredBackBufferHeight = 1024;
                _graphics.PreferredBackBufferWidth = 1280;
                _graphics.PreferredBackBufferHeight = 768;
                 
                // graphics.PreferredBackBufferWidth = 1024;
                // graphics.PreferredBackBufferHeight = 768;


            }

             // For some reason, if HardwareModeSwitch is false
             // It doesn't always make the game window the top window
             // in the foreground
             // if (_graphics.HardwareModeSwitch == false)
             // {
             //     BringGameWindowToForeground();
             // }


            Content.RootDirectory = "Content";

//            var graphicsProfile = _graphics.GraphicsProfile;
//            var isFixedTimeStep = this.IsFixedTimeStep;
//            var synchronizeWithVerticalRetrace = _graphics.SynchronizeWithVerticalRetrace;

            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            this.IsFixedTimeStep = false;
            _graphics.SynchronizeWithVerticalRetrace = false;

            simulationStateListenerList = new List<SimulationStateListener>();
            
            simulationStateListenerList.Add(new InitializeUIWhenScenarioInitializedEventHandler(this));
            
            simulationStateListenerList.Add(new AddMinigunnerViewWhenMinigunnerCreatedEventHandler(this));
            simulationStateListenerList.Add(new AddJeepViewWhenJeepCreatedEventHandler(this));
            simulationStateListenerList.Add(new AddMCVViewWhenMCVCreatedEventHandler(this));
            
            simulationStateListenerList.Add(new UpdateUnitViewPositionWhenUnitPositionChangedEventHandler(this));
            
            simulationStateListenerList.Add(new CreatePlannedPathViewWhenUnitMovementPlanCreatedEventHandler(this));
            simulationStateListenerList.Add(new RemovePlannedStepViewWhenUnitArrivesAtPathStepEventHandler(this));
            
            simulationStateListenerList.Add( new UpdateMapTileViewVisibilityWhenMapTileVisibilityChangedEventHandler(this));

            simulationStateListenerList.Add( new RemoveUnitViewWhenUnitDeletedEventHandler(this));

            IsMouseVisible = true;
            // double currentResolution = TimerHelper.GetCurrentResolution();
            // gameWorld = new GameWorld();
            gameWorldView = new GameWorldView();
            
            raiSpriteFrameManager = new RAISpriteFrameManager();
            spriteSheet = new SpriteSheet();
            currentGameState = new PlayingGameState();


            MikeAndConquerGame.instance = this;
        }


        internal void BringGameWindowToForeground()
        {
            // When
            //      _graphics.HardwareModeSwitch = false;
            // is used when setting full screen
            // it many times results in the top level window of the game 
            // not having focus
            // This method, BringGameWindowToForeground() , based on web search results,
            // Seems to fix that, but seems to only reliably work for
            // If I call it in the Update() loop

            Form myForm = (Form)Form.FromHandle(this.Window.Handle);
            myForm.Activate();
            topLevelWindowsHasBeenBroughtToForeground = true;
        }

        internal void PostCommand(RawCommandUI rawCommandUi)
        {

            AsyncViewCommand command = ConvertRawCommand(rawCommandUi);
            this.PostCommand(command);


        }

        internal AsyncViewCommand ConvertRawCommand(RawCommandUI rawCommand)
        {


            if (rawCommand.CommandType.Equals(StartScenarioUICommand.CommandName))
            {

                StartScenarioUICommand command = new StartScenarioUICommand(new HumanPlayerController());
                return command;

            }
            else if (rawCommand.CommandType.Equals(SelectUnitCommand.CommandName))
            {
                SelectUnitCommandBody commandBody =
                    JsonConvert.DeserializeObject<SelectUnitCommandBody>(rawCommand.CommandData);

                SelectUnitCommand command = new SelectUnitCommand(commandBody.UnitId);
                return command;
            }
            else if (rawCommand.CommandType.Equals(LeftClickCommand.CommandName))
            {
                ClickCommandBody commandBody =
                    JsonConvert.DeserializeObject<ClickCommandBody>(rawCommand.CommandData);

                LeftClickCommand command =
                    new LeftClickCommand(commandBody.XInWorldCoordinates, commandBody.YInWorldCoordinates);
                return command;
            }
            else if (rawCommand.CommandType.Equals(RightClickCommand.CommandName))
            {
                ClickCommandBody commandBody =
                    JsonConvert.DeserializeObject<ClickCommandBody>(rawCommand.CommandData);

                RightClickCommand command =
                    new RightClickCommand(commandBody.XInWorldCoordinates, commandBody.YInWorldCoordinates);
                return command;
            }

            else if (rawCommand.CommandType.Equals(LeftClickAndHoldCommand.CommandName))
            {
                ClickCommandBody commandBody =
                    JsonConvert.DeserializeObject<ClickCommandBody>(rawCommand.CommandData);

                LeftClickAndHoldCommand command =
                    new LeftClickAndHoldCommand(commandBody.XInWorldCoordinates, commandBody.YInWorldCoordinates);
                return command;
            }
            else if (rawCommand.CommandType.Equals(MoveMouseCommand.CommandName))
            {
                ClickCommandBody commandBody =
                    JsonConvert.DeserializeObject<ClickCommandBody>(rawCommand.CommandData);

                MoveMouseCommand command =
                    new MoveMouseCommand(commandBody.XInWorldCoordinates, commandBody.YInWorldCoordinates);
                return command;
            }
            else if (rawCommand.CommandType.Equals(ReleaseLeftMouseButtonCommand.CommandName))
            {
                ClickCommandBody commandBody =
                    JsonConvert.DeserializeObject<ClickCommandBody>(rawCommand.CommandData);

                ReleaseLeftMouseButtonCommand command = new ReleaseLeftMouseButtonCommand(commandBody.XInWorldCoordinates, commandBody.YInWorldCoordinates);
                return command;

            }
            else if (rawCommand.CommandType.Equals(SetUIOptionsCommand.CommandName))
            {
                SetUIOptionsCommandBody commandBody =
                    JsonConvert.DeserializeObject<SetUIOptionsCommandBody>(rawCommand.CommandData);

                SetUIOptionsCommand command = new SetUIOptionsCommand(commandBody.DrawShroud, commandBody.MapZoomLevel);
                return command;

            }


            else
            {
                throw new Exception("Unknown CommandType:" + rawCommand.CommandType);
            }

        }


        public void PostCommand(AsyncViewCommand command)
        {
            lock (inputCommandQueue)
            {
                inputCommandQueue.Enqueue(command);
            }
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {

            logger.LogInformation("Game1::LoadContent()");
            logger.LogWarning("Game1::LoadContent()");

            _spriteBatch = new SpriteBatch(GraphicsDevice);


            LoadTextures();
            
            
            gameWorldView.LoadContent();




            // TODO: use this.Content to load your game content here
        }


        private void LoadTextures()
        {
            LoadMapTextures();
            LoadSingleTextures();
            LoadShpFileTextures();
            LoadTemFiles();
            LoadBarracksPlacementTexture();
        }


        private void LoadBarracksPlacementTexture()
        {
            // LoadTmpFile(BarracksPlacementIndicatorView.FILE_NAME);
            // MapBlackMapTileFramePixelsToToTransparent(BarracksPlacementIndicatorView.FILE_NAME);
        }


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



        private void LoadMapTextures()
        {
            LoadTmpFile(CLEAR1_SHP);
            LoadTmpFile(D04_TEM);
            LoadTmpFile(D09_TEM);
            LoadTmpFile(D13_TEM);
            LoadTmpFile(D15_TEM);
            LoadTmpFile(D20_TEM);
            LoadTmpFile(D21_TEM);
            LoadTmpFile(D23_TEM);

            LoadTmpFile(P07_TEM);
            LoadTmpFile(P08_TEM);

            LoadTmpFile(S09_TEM);
            LoadTmpFile(S10_TEM);
            LoadTmpFile(S11_TEM);
            LoadTmpFile(S12_TEM);
            LoadTmpFile(S14_TEM);
            LoadTmpFile(S22_TEM);
            LoadTmpFile(S29_TEM);
            LoadTmpFile(S32_TEM);
            LoadTmpFile(S34_TEM);
            LoadTmpFile(S35_TEM);

            LoadTmpFile(SH1_TEM);
            LoadTmpFile(SH2_TEM);
            LoadTmpFile(SH3_TEM);
            LoadTmpFile(SH4_TEM);
            LoadTmpFile(SH5_TEM);
            LoadTmpFile(SH6_TEM);
            LoadTmpFile(SH9_TEM);
            LoadTmpFile(SH10_TEM);
            LoadTmpFile(SH17_TEM);
            LoadTmpFile(SH18_TEM);

            LoadTmpFile(W1_TEM);
            LoadTmpFile(W2_TEM);
        }

        private void LoadSingleTextures()
        {
            // spriteSheet.LoadSingleTextureFromFile(MissionAccomplishedMessage.MISSION_SPRITE_KEY, "Mission");
            // spriteSheet.LoadSingleTextureFromFile(MissionAccomplishedMessage.ACCOMPLISHED_SPRITE_KEY, "Accomplished");
            // spriteSheet.LoadSingleTextureFromFile(MissionFailedMessage.FAILED_SPRITE_KEY, "Failed");
            // spriteSheet.LoadSingleTextureFromFile(DestinationSquare.SPRITE_KEY, DestinationSquare.SPRITE_KEY);
            // spriteSheet.LoadSingleTextureFromFile(ReadyOverlay.SPRITE_KEY, ReadyOverlay.SPRITE_KEY);

        }


        private void LoadShpFileTextures()
        {
            raiSpriteFrameManager.LoadAllTexturesFromShpFile(GdiMinigunnerView.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                GdiMinigunnerView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(GdiMinigunnerView.SHP_FILE_NAME),
                GdiMinigunnerView.SHP_FILE_COLOR_MAPPER);
            // spriteSheet.LoadUnitFramesFromSpriteFrames(
            //     NodMinigunnerView.SPRITE_KEY,
            //     raiSpriteFrameManager.GetSpriteFramesForUnit(NodMinigunnerView.SHP_FILE_NAME),
            //     NodMinigunnerView.SHP_FILE_COLOR_MAPPER);
            
            
            raiSpriteFrameManager.LoadAllTexturesFromShpFile(MCVView.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                MCVView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(MCVView.SHP_FILE_NAME),
                MCVView.SHP_FILE_COLOR_MAPPER);


            raiSpriteFrameManager.LoadAllTexturesFromShpFile(JeepView.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                JeepView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(JeepView.SHP_FILE_NAME),
                JeepView.SHP_FILE_COLOR_MAPPER);

            // raiSpriteFrameManager.LoadAllTexturesFromShpFile(SandbagView.SHP_FILE_NAME);
            // spriteSheet.LoadUnitFramesFromSpriteFrames(
            //     SandbagView.SPRITE_KEY,
            //     raiSpriteFrameManager.GetSpriteFramesForUnit(SandbagView.SHP_FILE_NAME),
            //     SandbagView.SHP_FILE_COLOR_MAPPER);
            //
            // raiSpriteFrameManager.LoadAllTexturesFromShpFile(NodTurretView.SHP_FILE_NAME);
            // spriteSheet.LoadUnitFramesFromSpriteFrames(
            //     NodTurretView.SPRITE_KEY,
            //     raiSpriteFrameManager.GetSpriteFramesForUnit(NodTurretView.SHP_FILE_NAME),
            //     NodTurretView.SHP_FILE_COLOR_MAPPER);
            //
            //
            // raiSpriteFrameManager.LoadAllTexturesFromShpFile(Projectile120mmView.SHP_FILE_NAME);
            // spriteSheet.LoadUnitFramesFromSpriteFrames(
            //     Projectile120mmView.SPRITE_KEY,
            //     raiSpriteFrameManager.GetSpriteFramesForUnit(Projectile120mmView.SHP_FILE_NAME),
            //     Projectile120mmView.SHP_FILE_COLOR_MAPPER);
            //
            //
            // raiSpriteFrameManager.LoadAllTexturesFromShpFile(MinigunnerSidebarIconView.SHP_FILE_NAME);
            // spriteSheet.LoadUnitFramesFromSpriteFrames(
            //     MinigunnerSidebarIconView.SPRITE_KEY,
            //     raiSpriteFrameManager.GetSpriteFramesForUnit(MinigunnerSidebarIconView.SHP_FILE_NAME),
            //     MinigunnerSidebarIconView.SHP_FILE_COLOR_MAPPER);
            //
            // raiSpriteFrameManager.LoadAllTexturesFromShpFile(BarracksSidebarIconView.SHP_FILE_NAME);
            // spriteSheet.LoadUnitFramesFromSpriteFrames(
            //     BarracksSidebarIconView.SPRITE_KEY,
            //     raiSpriteFrameManager.GetSpriteFramesForUnit(BarracksSidebarIconView.SHP_FILE_NAME),
            //     BarracksSidebarIconView.SHP_FILE_COLOR_MAPPER);
            //
            //
            //
            //
            // raiSpriteFrameManager.LoadAllTexturesFromShpFile(GDIBarracksView.SHP_FILE_NAME);
            // spriteSheet.LoadUnitFramesFromSpriteFrames(
            //     GDIBarracksView.SPRITE_KEY,
            //     raiSpriteFrameManager.GetSpriteFramesForUnit(GDIBarracksView.SHP_FILE_NAME),
            //     GDIBarracksView.SHP_FILE_COLOR_MAPPER);
            //
            // raiSpriteFrameManager.LoadAllTexturesFromShpFile(GDIConstructionYardView.SHP_FILE_NAME);
            // spriteSheet.LoadUnitFramesFromSpriteFrames(
            //     GDIConstructionYardView.SPRITE_KEY,
            //     raiSpriteFrameManager.GetSpriteFramesForUnit(GDIConstructionYardView.SHP_FILE_NAME),
            //     GDIConstructionYardView.SHP_FILE_COLOR_MAPPER);
            //
            raiSpriteFrameManager.LoadAllTexturesFromShpFile(PartiallyVisibileMapTileMask.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(PartiallyVisibileMapTileMask.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(PartiallyVisibileMapTileMask.SHP_FILE_NAME),
                PartiallyVisibileMapTileMask.SHP_FILE_COLOR_MAPPER);

        }

        private void LoadTemFiles()
        {
            LoadTerrainTexture("T01.tem");
            LoadTerrainTexture("T02.tem");
            LoadTerrainTexture("T05.tem");
            LoadTerrainTexture("T06.tem");
            LoadTerrainTexture("T07.tem");
            LoadTerrainTexture("T16.tem");
            LoadTerrainTexture("T17.tem");
            LoadTerrainTexture("TC01.tem");
            LoadTerrainTexture("TC02.tem");
            LoadTerrainTexture("TC04.tem");
            LoadTerrainTexture("TC05.tem");

        }


        private void LoadTerrainTexture(String filename)
        {
            raiSpriteFrameManager.LoadAllTexturesFromShpFile(filename);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                filename,
                raiSpriteFrameManager.GetSpriteFramesForUnit(filename),
                TerrainView.SHP_FILE_COLOR_MAPPER);

        }





        private void LoadTmpFile(string tmpFileName)
        {
            raiSpriteFrameManager.LoadAllTexturesFromTmpFile(tmpFileName);
            spriteSheet.LoadMapTileFramesFromSpriteFrames(
                tmpFileName,
                raiSpriteFrameManager.GetSpriteFramesForMapTile(tmpFileName));

        }


        protected override void Update(GameTime gameTime)
        {


            if (!topLevelWindowsHasBeenBroughtToForeground)
            {
                BringGameWindowToForeground();
                // Form myForm = (Form)Form.FromHandle(this.Window.Handle);
                // myForm.Activate();
                // topLevelWindowsHasBeenBroughtToForeground = true;

            }


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                MikeAndConquerGame.instance.logger.LogError("Exiting because Escape key was pressed");
                Exit();
            }
        
            // TODO: Add your update logic here
            
        
            base.Update(gameTime);
       
        
            lock (inputCommandQueue)
            {
                while (inputCommandQueue.Count > 0)
                {
                    AsyncViewCommand anEvent = inputCommandQueue.Dequeue();
                    anEvent.Process();
                }
            }
        
            KeyboardState newKeyboardState = Keyboard.GetState();
            
            gameWorldView.Update(gameTime, newKeyboardState);
            
            currentGameState = this.currentGameState.Update(gameTime);
            this.currentGameStateView.Update(gameTime);
        
        }


        public void AddMinigunnerView(int id, int x, int y)
        {
            // UnitView unitView = new UnitView();
            // unitView.UnitId = id;
            // unitView.XInWorldCoordinates = x;
            // unitView.YInWorldCoordinates = y;
            // unitView.type = "Minigunner";
            // unitView.color = Color.Chocolate;
            // unitViewList.Add(unitView);

            gameWorldView.AddMinigunnerView(id, x, y);
            // MinigunnerView minigunnerView = new GdiMinigunnerView(id, x, y);
            // unitViewList.Add(minigunnerView);
        }

        public void RemoveUnitView(int unitId)
        {
            gameWorldView.RemoveUnitView(unitId);
        }

        public void AddJeepView(int id, int x, int y)
        {
            // UnitView unitView = new UnitView();
            // unitView.UnitId = id;
            // unitView.XInWorldCoordinates = x;
            // unitView.YInWorldCoordinates = y;
            // unitView.type = "Jeep";
            // unitView.color = Color.Blue;
            // unitViewList.Add(unitView);
            gameWorldView.AddJeepView(id, x, y);
        }

        public void AddMCVView(int id, int x, int y)
        {
            // // hasJeepBeenCreated = true;
            // // jeepX = x;
            // // jeepY = y;
            // UnitView unitView = new UnitView();
            // unitView.UnitId = id;
            // unitView.XInWorldCoordinates = x;
            // unitView.YInWorldCoordinates = y;
            // unitView.type = "MCV";
            // unitView.color = Color.Yellow;
            // unitViewList.Add(unitView);
            gameWorldView.AddMCVView(id, x, y);

        }


        protected override void Draw(GameTime gameTime)
        {
            Viewport originalViewport = GraphicsDevice.Viewport;
            
            GraphicsDevice.Clear(Color.Crimson);
             
            currentGameStateView.Draw(gameTime);
            //
            // DrawMap(gameTime);
            // DrawSidebar(gameTime);
            // DrawGameCursor(gameTime);
            
            // GraphicsDevice.Viewport = defaultViewport;
            GraphicsDevice.Viewport = originalViewport;
            base.Draw(gameTime);


        }


        // protected override void Draw(GameTime gameTime)
        // {
        //     GraphicsDevice.Clear(Color.CornflowerBlue);
        //     _spriteBatch.Begin();
        //
        //
        //     if (hasScenarioBeenInitialized)
        //     {
        //         DrawMap();
        //     }
        //
        //
        //
        //     foreach (UnitView unitView in unitViewList)
        //     {
        //         DrawRectangleAtCoordinate(unitView.XInWorldCoordinates, unitView.YInWorldCoordinates, unitView.color );
        //     }
        //
        //     // TODO: Add your drawing code here
        //
        //     _spriteBatch.End();
        //     base.Draw(gameTime);
        // }

        private void DrawMap()
        {

            for(int column = 0; column < this.mapWidth; column++) 
                for(int row = 0; row < this.mapHeight; row++)
                    DrawUnfilledRectangleAtCoordinate(column * 24, row * 24, 24, 24, Color.Red);

        }

        void DrawRectangleAtCoordinate(int x, int y, Color aColor)
        {
            int width = 10;
            int height = 10;
            Texture2D rect = new Texture2D(GraphicsDevice, width, height);

            Color[] data = new Color[width * height];
            for (int i = 0; i < data.Length; ++i) data[i] = aColor;
            rect.SetData(data);

            Vector2 coor = new Vector2(x, y);
            _spriteBatch.Draw(rect, coor, Color.White);
        }

        private Texture2D mapRect = null;
        void DrawUnfilledRectangleAtCoordinate(int x, int y, int width, int height, Color color)
        {
            if (mapRect == null)
            {
                mapRect = new Texture2D(GraphicsDevice, width, height);
                Color[] data = new Color[width * height];
                // Draw top line
                for (int i = 0; i < width; ++i) data[i] = color;

                // Draw left line
                for (int i = 0; i < width * height; i++)
                {
                    if (i % width == 0)
                    {
                        data[i] = color;
                    }
                }

                // Draw right line
                for (int i = 0; i < width * height; i++)
                {
                    if ((i + 1) % width == 0)
                    {
                        data[i] = color;
                    }
                }


                // Draw bottom line
                for (int i = width * height - width; i < width * height; i++) data[i] = color;

                // 012
                // 345        
                // 678

                mapRect.SetData(data);

            }

            Vector2 coor = new Vector2(x, y);
            _spriteBatch.Draw(mapRect, coor, Color.White);
        }




        public void UpdateUnitViewPosition(UnitPositionChangedEventData unitPositionChangedEventData)
        {
            foreach (UnitView unitView in gameWorldView.UnitViewList)
            {
                if (unitView.UnitId == unitPositionChangedEventData.UnitId)
                {
                    unitView.XInWorldCoordinates = unitPositionChangedEventData.XInWorldCoordinates;
                    unitView.YInWorldCoordinates = unitPositionChangedEventData.YInWorldCoordinates;
                }
            }

        }

        public void InitializeUI(ScenarioInitializedEventData scenarioInitializedEventData)
        {

            gameWorldView.HandleReset();
            // hasScenarioBeenInitialized = false;
            // mapWidth = -10;
            // mapHeight = -10;

            this.mapWidth = scenarioInitializedEventData.MapWidth;
            this.mapHeight = scenarioInitializedEventData.MapHeight;

            foreach (MapTileInstanceCreateEventData mapTileInstanceCreateEventData in scenarioInitializedEventData
                         .MapTileInstanceCreateEventDataList)
            {
                // public MapTileInstanceView(int imageIndex, string textureKey, bool isBlockingTerrain, MapTileVisibility mapTileVisibility)


                Enum.TryParse(mapTileInstanceCreateEventData.Visibility,
                    out MapTileInstanceView.MapTileVisibility visibilityEnumValue);

                // MapTileInstanceView mapTileInstanceView = new MapTileInstanceView(
                //     mapTileInstanceCreateEventData.ImageIndex,
                //     mapTileInstanceCreateEventData.TextureKey,
                //     mapTileInstanceCreateEventData.IsBlockingTerrain,
                //     visibilityEnumValue);

                gameWorldView.AddMapTileInstanceView(
                    mapTileInstanceCreateEventData.MapTileInstanceId,
                    mapTileInstanceCreateEventData.XInWorldMapTileCoordinates,
                    mapTileInstanceCreateEventData.YInWorldMapTileCoordinates,
                    mapTileInstanceCreateEventData.ImageIndex,
                    mapTileInstanceCreateEventData.TextureKey,
                    mapTileInstanceCreateEventData.IsBlockingTerrain,
                    visibilityEnumValue);

            }

            foreach (TerrainItemCreateEventData terrainItemCreateEventData in scenarioInitializedEventData
                         .TerrainItemCreateEventDataList)
            {
                gameWorldView.AddTerrainItemView(
                    terrainItemCreateEventData.XInWorldMapTileCoordinates,
                    terrainItemCreateEventData.YInWorldMapTileCoordinates,
                    terrainItemCreateEventData.TerrainItemType);

            }

            gameWorldView.NumColumns = this.mapWidth;
            gameWorldView.NumRows = this.mapHeight;
            hasScenarioBeenInitialized = true;
            gameWorldView.redrawBaseMapTiles = true;
        }


        //     foreach (MapTileInstance mapTileInstance in GameWorld.instance.gameMap.MapTileInstanceList)
        //     {
        //         AddMapTileInstanceView(mapTileInstance);
        //     }


        // public void SwitchToNewGameStateViewIfNeeded()
        // {
        //     GameState currentGameState = this.GetCurrentGameState();
        //     if (currentGameState.GetType().Equals(typeof(PlayingGameState)))
        //     {
        //         HandleSwitchToPlayingGameStateView();
        //     }
        //     else if (currentGameState.GetType().Equals(typeof(MissionAccomplishedGameState)))
        //     {
        //         HandleSwitchToMissionAccomplishedGameStateView();
        //     }
        //     else if (currentGameState.GetType().Equals(typeof(MissionFailedGameState)))
        //     {
        //         HandleSwitchToMissionFailedGameStateView();
        //     }
        // }

        public void SwitchToNewGameStateViewIfNeeded()
        {
            HandleSwitchToPlayingGameStateView();
        }

        private void HandleSwitchToPlayingGameStateView()
        {
            if (currentGameStateView == null || !currentGameStateView.GetType().Equals(typeof(PlayingGameStateView)))
            {
                currentGameStateView = new PlayingGameStateView();
            }
        }

        // private void HandleSwitchToMissionAccomplishedGameStateView()
        // {
        //     if (currentGameStateView == null || !currentGameStateView.GetType().Equals(typeof(MissionAccomplishedGameStateView)))
        //     {
        //         currentGameStateView = new MissionAccomplishedGameStateView();
        //     }
        // }
        //
        // private void HandleSwitchToMissionFailedGameStateView()
        // {
        //     if (currentGameStateView == null || !currentGameStateView.GetType().Equals(typeof(MissionFailedGameStateView)))
        //     {
        //         currentGameStateView = new MissionFailedGameStateView();
        //     }
        // }

        public void StartScenario(PlayerController playerController)
        {
            StartScenarioCommand command = new StartScenarioCommand();
            command.GDIPlayerController = playerController;
            SimulationMain.instance.PostCommand(command);

        }

        public void SelectUnit(int unitId)
        {

            UnitView unitView = this.gameWorldView.GetUnitViewById(unitId);

            Vector2 unitViewLocationAsWorldCoordinates = new Vector2();
            unitViewLocationAsWorldCoordinates.X = unitView.XInWorldCoordinates;
            unitViewLocationAsWorldCoordinates.Y = unitView.YInWorldCoordinates - 10;

            Vector2 transformedLocation =
                GameWorldView.instance.ConvertWorldCoordinatesToScreenCoordinates(unitViewLocationAsWorldCoordinates);

            int screenWidth = GameWorldView.instance.ScreenWidth;
            int screenHeight = GameWorldView.instance.ScreenHeight;

            // It appears that this mouse clicking code needs to run in a thread other than the main game processing thread
            // maybe because it has sleeps in it?
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                // Console.WriteLine("Hello, world");
                MouseInputHandler.DoLeftMouseClick((uint)transformedLocation.X, (uint)transformedLocation.Y, screenWidth, screenHeight);

            }).Start();

        }


        private Vector2 ConvertWorldCoordinatesToScreenCoordaintes(int xInWorldCoordinates, int yInWorldCoordinates)
        {
            Vector2 unitViewLocationAsWorldCoordinates = new Vector2();
            unitViewLocationAsWorldCoordinates.X = xInWorldCoordinates;
            unitViewLocationAsWorldCoordinates.Y = yInWorldCoordinates - 10;

            Vector2 transformedLocation =
                GameWorldView.instance.ConvertWorldCoordinatesToScreenCoordinates(unitViewLocationAsWorldCoordinates);

            return transformedLocation;
        }

        public void LeftClick(int xInWorldCoordinates, int yInWorldCoordinates)
        {

            Vector2 transformedLocation =
                ConvertWorldCoordinatesToScreenCoordaintes(xInWorldCoordinates, yInWorldCoordinates);

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                // Console.WriteLine("Hello, world");
                MouseInputHandler.DoLeftMouseClick(
                    (uint)transformedLocation.X,
                    (uint)transformedLocation.Y,
                    GameWorldView.instance.ScreenWidth,
                    GameWorldView.instance.ScreenHeight);

            }).Start();

        }

        public void RightClick(int xInWorldCoordinates, int yInWorldCoordinates)
        {
            Vector2 transformedLocation =
                ConvertWorldCoordinatesToScreenCoordaintes(xInWorldCoordinates, yInWorldCoordinates);

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                MouseInputHandler.DoRightMouseClick(
                    (uint)transformedLocation.X,
                    (uint)transformedLocation.Y,
                    GameWorldView.instance.ScreenWidth,
                    GameWorldView.instance.ScreenHeight);

            }).Start();

        }



        public void MoveMouse(int xInWorldCoordinates, int yInWorldCoordinates)
        {

            Vector2 transformedLocation =
                ConvertWorldCoordinatesToScreenCoordaintes(xInWorldCoordinates, yInWorldCoordinates);


            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                MouseInputHandler.MoveMouseToCoordinates(
                    (uint)transformedLocation.X,
                    (uint)transformedLocation.Y,
                    GameWorldView.instance.ScreenWidth,
                    GameWorldView.instance.ScreenHeight);

            }).Start();

        }


        public void LeftClickAndHold(int xInWorldCoordinates, int yInWorldCoordinates)
        {
            Vector2 transformedLocation =
                ConvertWorldCoordinatesToScreenCoordaintes(xInWorldCoordinates, yInWorldCoordinates);


            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                MouseInputHandler.DoLeftMouseClickAndHold(
                    (uint)transformedLocation.X,
                    (uint)transformedLocation.Y,
                    GameWorldView.instance.ScreenWidth,
                    GameWorldView.instance.ScreenHeight);

            }).Start();

        }

        public void ReleaseLeftMouseButton(int xInWorldCoordinates, int yInWorldCoordinates)
        {

            Vector2 transformedLocation =
                ConvertWorldCoordinatesToScreenCoordaintes(xInWorldCoordinates, yInWorldCoordinates);


            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                MouseInputHandler.DoReleaseLeftMouseClick(
                    (uint)transformedLocation.X,
                    (uint)transformedLocation.Y,
                    GameWorldView.instance.ScreenWidth,
                    GameWorldView.instance.ScreenHeight);

            }).Start();

        }


        public UnitView GetUnitViewByIdByEvent(int unitId)
        {

            GetUnitViewCommand command = new GetUnitViewCommand(unitId);

            lock (inputCommandQueue)
            {
                inputCommandQueue.Enqueue(command);
            }

            UnitView unitView = command.GetUnitView();
            return unitView;
        }

        public MemoryStream GetScreenshotViaEvent()
        {
            GetScreenshotCommand command = new GetScreenshotCommand();

            lock (inputCommandQueue)
            {
                inputCommandQueue.Enqueue(command);
            }

            MemoryStream memoryStream = command.GetMemoryStream();
            return memoryStream;

        }



        public UnitView GetUnitViewById(int unitId)
        {
            return gameWorldView.GetUnitViewById(unitId);
        }

        public void CreatePlannedPathView(int unitId, List<PathStep> pathStepList)
        {
            gameWorldView.CreatePlannedPathView(unitId, pathStepList);
        }

        public void RemovePlannedStepView(int unitId, PathStep pathStep)
        {
            gameWorldView.RemovePlannedStepView(unitId, pathStep);
        }


        public void UpdateMapTileViewVisibility(MapTileVisibilityUpdatedEventData eventData)
        {
            GameWorldView.instance.UpdateMapTileViewVisibility(eventData);
        }

        public void SetUIOptions(bool drawShroud, float mapZoomLevel)
        {
            GameOptions.instance.DrawShroud = drawShroud;
            GameOptions.instance.MapZoomLevel = mapZoomLevel;

            int x = 3;

            bool drawShroud2 = GameOptions.instance.DrawShroud;
            float mapZoomLevel2 = GameOptions.instance.MapZoomLevel;
            x = 4;
        }
    }
}
