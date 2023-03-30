using System;
using System.Collections.Generic;
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
using Serilog;
using MemoryStream = System.IO.MemoryStream;
using Form = System.Windows.Forms.Form;
using BarracksSidebarIconView = mike_and_conquer_monogame.gameview.sidebar.BarracksSidebarIconView;
using ReadyOverlay = mike_and_conquer_monogame.gameview.sidebar.ReadyOverlay;
using MinigunnerSidebarIconView = mike_and_conquer_monogame.gameview.sidebar.MinigunnerSidebarIconView;



namespace mike_and_conquer_monogame.main
{
    public class MikeAndConquerGame : Game
    {
        private static readonly ILogger Logger = Log.ForContext<MikeAndConquerGame>();

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public List<SimulationStateListener> simulationStateListenerList = null;

        private bool hasScenarioBeenInitialized = false;
        private int mapWidth = -10;
        private int mapHeight = -10;

        private int mouseCounter = 0;

        private Queue<AsyncViewCommand> inputCommandQueue;

        public static MikeAndConquerGame instance;

        public const string CONTENT_DIRECTORY_PREFIX = "Content\\";

        public SpriteSheet SpriteSheet
        {
            get { return spriteSheet; }
        }

        private SpriteSheet spriteSheet;

        private GameWorldView gameWorldView;

        private GameState currentGameState;

        private GameStateView currentGameStateView;

        private RAISpriteFrameManager raiSpriteFrameManager;

        private bool topLevelWindowsHasBeenBroughtToForeground = false;


        public MikeAndConquerGame()
        {

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

            Content.RootDirectory = "Content";

            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            this.IsFixedTimeStep = false;
            _graphics.SynchronizeWithVerticalRetrace = false;

            simulationStateListenerList = new List<SimulationStateListener>();

            MasterEventHandler masterEventHandler = new MasterEventHandler(this);
            masterEventHandler.RegisterEventHandler(typeof(ScenarioInitializedEventData), typeof(InitializeUiCommand));
            masterEventHandler.RegisterEventHandler(typeof(GDIBarracksPlacedEventData), typeof(AddGDIBarracksViewCommand));
            masterEventHandler.RegisterEventHandler(typeof(GDIConstructionYardCreatedEventData), typeof(AddGDIConstructionYardViewCommand));
            masterEventHandler.RegisterEventHandler(typeof(MinigunnerCreateEventData), typeof(AddMinigunnerViewCommand));

            masterEventHandler.RegisterEventHandler(
                typeof(UnitBeganMovingEventData),
                typeof(UpdateUnitStateBeganMovingCommand));

            masterEventHandler.RegisterEventHandler(
                typeof(UnitBeganFiringEventData),
                typeof(UpdateUnitStateBeganFiringCommand));

            masterEventHandler.RegisterEventHandler(
                typeof(BeganMissionNoneEventData),
                typeof(UpdateUnitStateBeganIdleCommand));


            simulationStateListenerList.Add(masterEventHandler);

            simulationStateListenerList.Add(new AddJeepViewWhenJeepCreatedEventHandler(this));
            simulationStateListenerList.Add(new AddMCVViewWhenMCVCreatedEventHandler(this));

            simulationStateListenerList.Add(new UpdateUnitViewPositionWhenUnitPositionChangedEventHandler(this));
            
            simulationStateListenerList.Add(new CreatePlannedPathViewWhenUnitMovementPlanCreatedEventHandler(this));
            simulationStateListenerList.Add(new RemovePlannedStepViewWhenUnitArrivesAtPathStepEventHandler(this));
            
            simulationStateListenerList.Add( new UpdateMapTileViewVisibilityWhenMapTileVisibilityChangedEventHandler(this));

            simulationStateListenerList.Add( new RemoveUnitViewWhenUnitDeletedEventHandler(this));

            simulationStateListenerList.Add(new UpdateConstructionYardViewWhenConstructionYardStartsBuildingBarracks(this));
            simulationStateListenerList.Add(new UpdateBarracksViewWhenBarracksStartsBuildingMinigunner(this));


            simulationStateListenerList.Add(new UpdateBarracksPercentBuildCompleted(this));
            simulationStateListenerList.Add(new UpdateConstructionYardViewWhenConstructionYardCompletesBuildingBarracks (this));
            simulationStateListenerList.Add(new UpdateMinigunnerPercentBuildCompleted(this));

            

            IsMouseVisible = true;
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


        internal void ProcessUiCommandSynchronously(RawCommandUI rawCommandUi)
        {
            AsyncViewCommand command = ConvertRawCommand(rawCommandUi);
            command.Process();
            if (command.ThrownException != null)
            {
                string errorMessage = "Exception thrown processing command '" + command.ToString() + "' in SimulationMain.ProcessInputEventQueue().  Exception stacktrace follows:";
                Logger.Error(command.ThrownException, errorMessage);
            }

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
            if (rawCommand.CommandType.Equals(LeftClickSidebarCommand.CommandName))
            {
                ClickSidebarCommandBody commandBody =
                    JsonConvert.DeserializeObject<ClickSidebarCommandBody>(rawCommand.CommandData);

                LeftClickSidebarCommand command =
                    new LeftClickSidebarCommand(commandBody.SidebarIconName);
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

            Logger.Information("Game1::LoadContent()");

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadTextures();
           
            gameWorldView.LoadContent();

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
            LoadTmpFile(BarracksPlacementIndicatorView.FILE_NAME);
            MapBlackMapTileFramePixelsToToTransparent(BarracksPlacementIndicatorView.FILE_NAME);
        }

        private void MapBlackMapTileFramePixelsToToTransparent(string tmpFileName)
        {
            List<MapTileFrame> mapTileFrameList = spriteSheet.GetMapTileFrameForTmpFile(tmpFileName);
            foreach (MapTileFrame mapTileFrame in mapTileFrameList)
            {
                Texture2D theTexture = mapTileFrame.Texture;
                int numPixels = theTexture.Height * theTexture.Width;
                Color[] originalTexturePixelData = new Color[numPixels];
                Color[] changedTexturePixelData = new Color[numPixels];
                theTexture.GetData(originalTexturePixelData);

                int i = 0;
                foreach (Color color in originalTexturePixelData)
                {
                    if (color.R == 0)
                    {
                        Color newColor = new Color(0, 0, 0, 0);
                        changedTexturePixelData[i] = newColor;
                    }
                    else
                    {
                        changedTexturePixelData[i] = color;
                    }

                    i++;
                }
                theTexture.SetData(changedTexturePixelData);

            }

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
             spriteSheet.LoadSingleTextureFromFile(ReadyOverlay.SPRITE_KEY, ReadyOverlay.SPRITE_KEY);

        }


        private void LoadShpFileTextures()
        {
            raiSpriteFrameManager.LoadAllTexturesFromShpFile(GdiMinigunnerView.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                GdiMinigunnerView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(GdiMinigunnerView.SHP_FILE_NAME),
                GdiMinigunnerView.SHP_FILE_COLOR_MAPPER);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                NodMinigunnerView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(NodMinigunnerView.SHP_FILE_NAME),
                NodMinigunnerView.SHP_FILE_COLOR_MAPPER);
            
            
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
            
            
            raiSpriteFrameManager.LoadAllTexturesFromShpFile(MinigunnerSidebarIconView.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                MinigunnerSidebarIconView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(MinigunnerSidebarIconView.SHP_FILE_NAME),
                MinigunnerSidebarIconView.SHP_FILE_COLOR_MAPPER);
            
            raiSpriteFrameManager.LoadAllTexturesFromShpFile(BarracksSidebarIconView.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                BarracksSidebarIconView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(BarracksSidebarIconView.SHP_FILE_NAME),
                BarracksSidebarIconView.SHP_FILE_COLOR_MAPPER);
            
            
            
            
            raiSpriteFrameManager.LoadAllTexturesFromShpFile(GDIBarracksView.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                GDIBarracksView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(GDIBarracksView.SHP_FILE_NAME),
                GDIBarracksView.SHP_FILE_COLOR_MAPPER);
            
            raiSpriteFrameManager.LoadAllTexturesFromShpFile(GDIConstructionYardView.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                GDIConstructionYardView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(GDIConstructionYardView.SHP_FILE_NAME),
                GDIConstructionYardView.SHP_FILE_COLOR_MAPPER);
            
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
            }

            FixMousePointerProblem();


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Logger.Error("Exiting because Escape key was pressed");
                Exit();
            }
        
            // TODO: Add your update logic here
            
        
            base.Update(gameTime);
       
        
            lock (inputCommandQueue)
            {
                while (inputCommandQueue.Count > 0)
                {
                    AsyncViewCommand command = inputCommandQueue.Dequeue();
                    command.Process();
                    if (command.ThrownException != null)
                    {
                        Logger.Error(command.ThrownException.ToString());
                    }
                }
            }
        
            KeyboardState newKeyboardState = Keyboard.GetState();
            
            gameWorldView.Update(gameTime, newKeyboardState);
            
            currentGameState = this.currentGameState.Update(gameTime);
            this.currentGameStateView.Update(gameTime);
        
        }


        private void FixMousePointerProblem()
        {
            // This is a hack fix to fix an issue where if you change this.IsMouseVisible to false
            // while the Windows pointer is showing the mouse pointer arrow with the blue sworl "busy" icon on the side
            // it will continue to show a frozen(non moving) copy of the blue sworl "busy" icon, even after it 
            // stops showing and updating the normal Winodws mouse pointer (in favor of my manually handled one)
            // TODO:  Investigate replacing countdown timer with direct call to (possibly to native Windows API) to determine
            // native mouse pointer "busy" status, and wait until ti goes "not busy"
            if (mouseCounter < 20)
            {
                this.IsMouseVisible = true;
                mouseCounter++;
            }
            else
            {
                this.IsMouseVisible = false;
            }
        }



        public void AddMinigunnerView(int id, string player, int x, int y)
        {
            gameWorldView.AddMinigunnerView(id, player, x, y);
        }

        public void RemoveUnitView(int unitId)
        {
            gameWorldView.RemoveUnitView(unitId);
        }

        public void AddJeepView(int id, int x, int y)
        {
            gameWorldView.AddGDIJeepView(id, x, y);
        }

        public void AddMCVView(int id, int x, int y)
        {
            gameWorldView.AddGDIMCVView(id, x, y);
        }

        public void AddGDIConstructionYardView(int id, int x, int y)
        {
            gameWorldView.AddGDIConstructionYardView(id, x, y);
        }

        public void AddGDIBarracksView(int id, int x, int y)
        {
            gameWorldView.AddGDIBarracksView(id, x, y);
        }

        public void NotifyBarracksStartedBuilding()
        {
            gameWorldView.NotifyBarracksStartedBuilding();
        }

        public void NotifyMinigunnerStartedBuilding()
        {
            gameWorldView.NotifyMinigunnerStartedBuilding();
        }

        public void NotifyBarracksCompletedBuilding()
        {
            gameWorldView.NotifyBarracksCompletedBuilding();
        }

        public void UpdateBarracksPercentCompleted(int percentCompleted)
        {
            gameWorldView.UpdateBarracksPercentCompleted(percentCompleted);
        }


        public void UpdateMinigunnerPercentCompleted(int percentCompleted)
        {
            gameWorldView.UpdateMinigunnerPercentCompleted(percentCompleted);
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



        public void UpdateUnitViewPosition(int unitId, int xInWorldCoordinates, int yInWorldCoordinates)
        {
            foreach (UnitView unitView in gameWorldView.GDIUnitViewList)
            {
                if (unitView.UnitId == unitId)
                {
                    unitView.XInWorldCoordinates = xInWorldCoordinates;
                    unitView.YInWorldCoordinates = yInWorldCoordinates;
                }
            }

            foreach (UnitView unitView in gameWorldView.NodUnitViewList)
            {
                if (unitView.UnitId == unitId)
                {
                    unitView.XInWorldCoordinates = xInWorldCoordinates;
                    unitView.YInWorldCoordinates = yInWorldCoordinates;
                }
            }


        }

        public void InitializeUI(
            int theMapWidth,
            int theMapHeight,
            List<MapTileInstanceCreateEventData> mapTileInstanceCreateEventDataList,
            List<TerrainItemCreateEventData> terrainItemCreateEventDataList)
        {

            gameWorldView.HandleReset();

            this.mapWidth = theMapWidth;
            this.mapHeight = theMapHeight;

            foreach (MapTileInstanceCreateEventData mapTileInstanceCreateEventData in mapTileInstanceCreateEventDataList)
            {

                Enum.TryParse(mapTileInstanceCreateEventData.Visibility,
                    out MapTileInstanceView.MapTileVisibility visibilityEnumValue);

                gameWorldView.AddMapTileInstanceView(
                    mapTileInstanceCreateEventData.MapTileInstanceId,
                    mapTileInstanceCreateEventData.XInWorldMapTileCoordinates,
                    mapTileInstanceCreateEventData.YInWorldMapTileCoordinates,
                    mapTileInstanceCreateEventData.ImageIndex,
                    mapTileInstanceCreateEventData.TextureKey,
                    mapTileInstanceCreateEventData.IsBlockingTerrain,
                    visibilityEnumValue);

            }

            foreach (TerrainItemCreateEventData terrainItemCreateEventData in terrainItemCreateEventDataList)
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
            command.WaitUntilCompleted();

        }

        private UnitView GetUnitViewByIdViaCommand(int unitId)
        {
            GetMinigunnerViewCommand command = new GetMinigunnerViewCommand(GameWorldView.instance, unitId);
            PostCommand(command);
            UnitView unitView = (UnitView) command.GetResult();
            return unitView;
        }

        public void SelectUnit(int unitId)
        {

            UnitView unitView = GetUnitViewByIdViaCommand(unitId);

            Vector2 unitViewLocationAsWorldCoordinates = new Vector2();
            unitViewLocationAsWorldCoordinates.X = unitView.XInWorldCoordinates;
            unitViewLocationAsWorldCoordinates.Y = unitView.YInWorldCoordinates - 5;

            Vector2 transformedLocation =
                GameWorldView.instance.ConvertWorldCoordinatesToScreenCoordinates(unitViewLocationAsWorldCoordinates);

            int screenWidth = GameWorldView.instance.ScreenWidth;
            int screenHeight = GameWorldView.instance.ScreenHeight;

            MouseInputHandler.DoLeftMouseClick((uint)transformedLocation.X, (uint)transformedLocation.Y, screenWidth, screenHeight);

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

            MouseInputHandler.DoLeftMouseClick(
                (uint)transformedLocation.X,
                (uint)transformedLocation.Y,
                GameWorldView.instance.ScreenWidth,
                GameWorldView.instance.ScreenHeight);

        }



        public void LeftClickSidebar(string sidebarIconName)
        {
            Point position = new Point();

            if (sidebarIconName == "Barracks")
            {
                position = GameWorldView.instance.BarracksSidebarIconView.GetPosition();
            }
            else if (sidebarIconName == "Minigunner")
            {
                position = GameWorldView.instance.MinigunnerSidebarIconView.GetPosition();
            }
            else
            {
                throw new Exception("Unknown sidebarIconName:" + sidebarIconName);
            }

            Vector2 positionInWorldCoordinates = new Vector2(position.X, position.Y);

            Vector2 transformedLocation =
                GameWorldView.instance.ConvertWorldCoordinatesToScreenCoordinatesForSidebar(positionInWorldCoordinates);


            int screenWidth = GameWorldView.instance.ScreenWidth;
            int screenHeight = GameWorldView.instance.ScreenHeight;


            MouseInputHandler.MoveMouseToCoordinates((uint)transformedLocation.X, (uint)transformedLocation.Y, screenWidth, screenHeight);

            MouseInputHandler.DoLeftMouseClick((uint)transformedLocation.X, (uint)transformedLocation.Y, screenWidth, screenHeight);



        }

        public void RightClick(int xInWorldCoordinates, int yInWorldCoordinates)
        {
            Vector2 transformedLocation =
                ConvertWorldCoordinatesToScreenCoordaintes(xInWorldCoordinates, yInWorldCoordinates);

            MouseInputHandler.DoRightMouseClick(
                (uint)transformedLocation.X,
                (uint)transformedLocation.Y,
                GameWorldView.instance.ScreenWidth,
                GameWorldView.instance.ScreenHeight);

        }




        public void MoveMouse(int xInWorldCoordinates, int yInWorldCoordinates)
        {

            Vector2 transformedLocation =
                ConvertWorldCoordinatesToScreenCoordaintes(xInWorldCoordinates, yInWorldCoordinates);

            MouseInputHandler.MoveMouseToCoordinates(
                (uint)transformedLocation.X,
                (uint)transformedLocation.Y,
                GameWorldView.instance.ScreenWidth,
                GameWorldView.instance.ScreenHeight);

        }


        public void LeftClickAndHold(int xInWorldCoordinates, int yInWorldCoordinates)
        {
            Vector2 transformedLocation =
                ConvertWorldCoordinatesToScreenCoordaintes(xInWorldCoordinates, yInWorldCoordinates);

            MouseInputHandler.DoLeftMouseClickAndHold(
                (uint)transformedLocation.X,
                (uint)transformedLocation.Y,
                GameWorldView.instance.ScreenWidth,
                GameWorldView.instance.ScreenHeight);

        }

        public void ReleaseLeftMouseButton(int xInWorldCoordinates, int yInWorldCoordinates)
        {

            Vector2 transformedLocation =
                ConvertWorldCoordinatesToScreenCoordaintes(xInWorldCoordinates, yInWorldCoordinates);


            MouseInputHandler.DoReleaseLeftMouseClick(
                (uint)transformedLocation.X,
                (uint)transformedLocation.Y,
                GameWorldView.instance.ScreenWidth,
                GameWorldView.instance.ScreenHeight);

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


        public void UpdateMapTileViewVisibility(int mapTileInstanceId, string visibility)
        {
            GameWorldView.instance.UpdateMapTileViewVisibility(mapTileInstanceId, visibility);
        }

        public void SetUIOptions(bool drawShroud, float mapZoomLevel)
        {
            GameOptions.instance.DrawShroud = drawShroud;
            GameOptions.instance.MapZoomLevel = mapZoomLevel;
        }
    }
}
