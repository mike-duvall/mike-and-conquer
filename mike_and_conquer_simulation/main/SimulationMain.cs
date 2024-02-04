
using System;
using System.Collections.Generic;
using System.Threading;
using mike_and_conquer_simulation.commands;
using mike_and_conquer_simulation.commands.commandbody;
using mike_and_conquer_simulation.events;
using mike_and_conquer_simulation.gameworld;
using Newtonsoft.Json;
using Serilog;

namespace mike_and_conquer_simulation.main
{
    public class SimulationMain
    {

        private static readonly ILogger Logger = Log.ForContext<SimulationMain>();

        public static int globalId = 1;


        private SimulationOptions simulationOptions;

        private Queue<AsyncSimulationCommand> inputCommandQueue;

        private List<SimulationStateUpdateEvent> simulationStateUpdateEventsHistory;

        private List<SimulationStateListener> listeners;

        public static SimulationMain instance;

        public static ManualResetEvent condition;

        // private List<Unit> unitList;

        private GameWorld gameWorld;

        public static void StartSimulation(List<SimulationStateListener> listenerList)
        {



            Logger.Information("************************Information-Simulation Mike is cool");
            Logger.Warning("************************Warning-Simulation Mike is cool");


            new SimulationMain();
            foreach (SimulationStateListener listener in listenerList)
            {
                SimulationMain.instance.AddListener(listener);
            }


            condition = new ManualResetEvent(false);
            Thread backgroundThread = new Thread(new ThreadStart(SimulationMain.Main));
            backgroundThread.IsBackground = true;

            // Start thread  
            backgroundThread.Start();
            condition.WaitOne();

            // PublishInitializeScenarioEvent(27,23);


        }

        private void PublishInitializeScenarioEvent(
            int mapWidth,
            int mapHeight,
            MapTileInstance[,] mapTileInstanceArray,
            List<TerrainItem> terrainItemList)
        {

            List<MapTileInstanceCreateEventData> mapTileInstanceCreateEventDataList =
                new List<MapTileInstanceCreateEventData>();

            int numRows = gameWorld.gameMap.NumRows;
            int numColumns = gameWorld.gameMap.NumColumns;


            for (int row = 0; row < numRows; row++)
            {
                for (int column = 0; column < numColumns; column++)
                {
                    MapTileInstance mapTileInstance = mapTileInstanceArray[column, row];

                    MapTileInstanceCreateEventData mapTileCreateEventData = new MapTileInstanceCreateEventData(
                        mapTileInstance.MapTileInstanceId,
                        mapTileInstance.MapTileLocation.XInWorldMapTileCoordinates,
                        mapTileInstance.MapTileLocation.YInWorldMapTileCoordinates,
                        mapTileInstance.TextureKey,
                        mapTileInstance.ImageIndex,
                        mapTileInstance.IsBlockingTerrain,
                        mapTileInstance.Visibility.ToString()
                    );

                    mapTileInstanceCreateEventDataList.Add(mapTileCreateEventData);

                }

            }



            List<TerrainItemCreateEventData> terrainItemCreateEventDataList =
                new List<TerrainItemCreateEventData>();

            foreach (TerrainItem terrainItem in terrainItemList)
            {
                TerrainItemCreateEventData terrainItemCreateEventData = new TerrainItemCreateEventData(
                    terrainItem.MapTileLocation.XInWorldMapTileCoordinates,
                    terrainItem.MapTileLocation.YInWorldMapTileCoordinates,
                    terrainItem.TerrainItemType);
                terrainItemCreateEventDataList.Add(terrainItemCreateEventData);

            }

            ScenarioInitializedEventData initializedEventData = new ScenarioInitializedEventData(
                mapWidth,
                mapHeight,
                mapTileInstanceCreateEventDataList,
                terrainItemCreateEventDataList);

            string serializedEventData = JsonConvert.SerializeObject(initializedEventData);
            SimulationStateUpdateEvent simulationStateUpdateEvent =
                new SimulationStateUpdateEvent(
                        ScenarioInitializedEventData.EventType,
                        serializedEventData
                    );

            PublishEvent(simulationStateUpdateEvent);

        }



        public void AddListener(SimulationStateListener listener)
        {
            listeners.Add(listener);
        }


        public static void Main()
        {
            SimulationMain.condition.Set();
            long previousTicks = 0;
            SimulationMain.instance.SetGameSpeed(SimulationOptions.GameSpeed.Normal);

            while (true)
            {


                int sleepTime = (int) SimulationMain.instance.simulationOptions.CurrentGameSpeed;
                //TimerHelper.SleepForNoMoreThan(sleepTime, Logger);
                TimerHelper.SleepForNoMoreThan2(sleepTime);
                // TimerHelper.SleepForNoMoreThan(sleepTime);
                //Thread.Sleep(1);



                SimulationMain.instance.Tick();

                bool doneWaiting = false;
                long delta = -1;
                long currentTicks = -1;

                while (!doneWaiting)
                {
                
                     currentTicks = DateTime.Now.Ticks;
                     delta = (currentTicks - previousTicks) / TimeSpan.TicksPerMillisecond;
                     if (delta >= sleepTime)
                     {
                         doneWaiting = true;
                     }


                }
                // Logger.LogInformation("delta=" + delta + ",  sleepTime=" + sleepTime);
                previousTicks = currentTicks;
            }

        }


        SimulationMain()
        {
            inputCommandQueue = new Queue<AsyncSimulationCommand>();
            simulationStateUpdateEventsHistory = new List<SimulationStateUpdateEvent>();
            listeners = new List<SimulationStateListener>();
            listeners.Add(new SimulationStateHistoryListener(this));

            // unitList = new List<Unit>();

            gameWorld = new GameWorld();

            simulationOptions = new SimulationOptions();

            SimulationMain.instance = this;
        }

        private void Tick()
        {
            Update();
            ProcessInputCommandQueue();
        }

        private void Update()
        {
            // foreach (Unit unit in unitList)
            // {
            //     unit.Update();
            // }

            gameWorld.Update();

        }

        private void ProcessInputCommandQueue()
        {
            lock (inputCommandQueue)
            {
                while (inputCommandQueue.Count > 0)
                {
                    AsyncSimulationCommand anEvent = inputCommandQueue.Dequeue();
                    anEvent.Process();
                    if (anEvent.ThrownException != null)
                    {
                        string errorMessage = "Exception thrown processing command '" + anEvent.ToString() + "' in SimulationMain.ProcessInputCommandQueue().  Exception stacktrace follows:";
                        // Logger.LogError(anEvent.ThrownException, errorMessage);
                    }
                }
            }
        }

        public List<SimulationStateUpdateEvent> GetCopyOfEventHistoryViaCommand()
        {
            GetCopyOfEventHistoryCommand anEvent = new GetCopyOfEventHistoryCommand();

            lock (inputCommandQueue)
            {
                inputCommandQueue.Enqueue(anEvent);
            }

            List<SimulationStateUpdateEvent> list = anEvent.GetCopyOfEventHistory();
            return list;
        }

        internal Minigunner CreateGDIMinigunner(int xInWorldCoordinates, int yInWorldCoordinates)
        {
            return gameWorld.CreateGDIMinigunner(xInWorldCoordinates, yInWorldCoordinates);
        }

        internal Minigunner CreateNodMinigunner(int xInWorldCoordinates, int yInWorldCoordinates)
        {
            return gameWorld.CreateNodMinigunner(xInWorldCoordinates, yInWorldCoordinates);
        }



        internal Minigunner CreateGDIMinigunnerAtRandomLocation()
        {
            return gameWorld.CreateGDIMinigunnerAtRandomLocation();
        }

        internal Minigunner CreateNodMinigunnerAtRandomLocation()
        {
            return gameWorld.CreateNodMinigunnerAtRandomLocation();
        }



        internal Jeep CreateJeep(int xInWorldCoordinates, int yInWorldCoordinates)
        {

            return gameWorld.CreateJeep(xInWorldCoordinates, yInWorldCoordinates);

            // Jeep jeep = new Jeep();
            // jeep.GameWorldLocation.X = x;
            // jeep.GameWorldLocation.Y = y;
            // unitList.Add(jeep);
            //
            // SimulationStateUpdateEvent simulationStateUpdateEvent = new SimulationStateUpdateEvent();
            // simulationStateUpdateEvent.EventType = JeepCreateEventData.EventType;
            // JeepCreateEventData eventData = new JeepCreateEventData();
            // eventData.UnitId = jeep.UnitId;
            // eventData.X = x;
            // eventData.Y = y;
            //
            // simulationStateUpdateEvent.EventData = JsonConvert.SerializeObject(eventData);
            //
            // foreach (SimulationStateListener listener in listeners)
            // {
            //     listener.Update(simulationStateUpdateEvent);
            // }
            //
            // return jeep;
        }

        internal MCV CreateMCV(int xInWorldCoordinates, int yInWorldCoordinates)
        {
            return gameWorld.CreateMCV(xInWorldCoordinates, yInWorldCoordinates);
        }


        // internal GDIBarracks AddGDIBarracks(int xInWorldCoordinates, int yInWorldCoordinates)
        // {
        //     return gameWorld.CreateGDIBarracks(xInWorldCoordinates, yInWorldCoordinates);
        // }

        internal GDIConstructionYard CreateConstructionYardFromMCV()
        {
            return gameWorld.CreateConstructionYardFromMCV();
        } 


        internal void RemoveUnit(int unitId)
        {
            gameWorld.RemoveUnit(unitId);
        }


        internal void BeginBuildingBarracks()
        {
            gameWorld.BeginBuildingBarracks();
        }

        internal void BeginBuildingMinigunner()
        {
            gameWorld.BeginBuildingMinigunner();
        }

        internal GDIBarracks CreateGDIBarracksViaConstructionYard(int xInWorldCoordinates, int yInWorldCoordinates)
        {
            return gameWorld.CreateGDIBarracksViaConstructionYard(xInWorldCoordinates, yInWorldCoordinates);
        }


        public void OrderUnitToMove(int unitId, int destinationXInWorldCoordinates, int destinationYInWorldCoordinates)
        {
            Unit foundUnit = FindGDIUnitWithUnitId(unitId);
            foundUnit.OrderMoveToDestination(destinationXInWorldCoordinates, destinationYInWorldCoordinates);
        }

        public void OrderUnitToAttack(int attackerUnitId, int targetUnitId)
        {
            Unit attackingUnit = FindGDIUnitWithUnitId(attackerUnitId);
            Unit targetUnit = FindNodUnitWithUnitId(targetUnitId);
            attackingUnit.OrderToAttackEnemyUnit(targetUnit);

        }

        private Unit FindGDIUnitWithUnitId(int unitId)
        {
            return gameWorld.FindGDIUnitWithUnitId(unitId);
        }

        private Unit FindNodUnitWithUnitId(int unitId)
        {
            return gameWorld.FindNodUnitWithUnitId(unitId);
        }


        public List<SimulationStateUpdateEvent> GetCopyOfEventHistory()
        {
            List<SimulationStateUpdateEvent> copyList = new List<SimulationStateUpdateEvent>();

            foreach (SimulationStateUpdateEvent simulationStateUpdateEvent in simulationStateUpdateEventsHistory)
            {
                String copyEventData = new string(simulationStateUpdateEvent.EventData);


                SimulationStateUpdateEvent copyEvent = new SimulationStateUpdateEvent(
                    simulationStateUpdateEvent.EventType,
                    copyEventData);

                copyList.Add(copyEvent);
            }

            return copyList;
        }


        public void AddHistoryEvent(SimulationStateUpdateEvent anEvent)
        {
            simulationStateUpdateEventsHistory.Add(anEvent); 
        }

        public void PublishEvent(SimulationStateUpdateEvent simulationStateUpdateEvent)
        {
            foreach (SimulationStateListener listener in listeners)
            {
                listener.Update(simulationStateUpdateEvent);
            }
        }

        internal void SetGameSpeed(SimulationOptions.GameSpeed aGameSpeed)
        {
            this.simulationOptions.CurrentGameSpeed = aGameSpeed;
        }

        internal SimulationOptions GetSimulationOptions()
        {
            return simulationOptions;
        }


        public void PostCommand(AsyncSimulationCommand command)
        {
            lock (inputCommandQueue)
            {
                inputCommandQueue.Enqueue(command);
            }

        }



        internal void PostCommand(JsonAsyncSimulationCommand incomingAdminCommand)
        {
            AsyncSimulationCommand asyncSimulationCommand = ParseAsyncSimulationCommand(incomingAdminCommand);
            PostCommand(asyncSimulationCommand);
        }

        AsyncSimulationCommand ParseAsyncSimulationCommand(JsonAsyncSimulationCommand jsonAsyncSimulationCommand)
        {
            if (jsonAsyncSimulationCommand.CommandType.Equals(CreateGDIMinigunnerCommand.CommandName))
            {

                CreateMinigunnerCommandBody commandBody =
                    JsonConvert.DeserializeObject<CreateMinigunnerCommandBody>(jsonAsyncSimulationCommand.JsonCommandData);

                CreateGDIMinigunnerCommand createGdiUnit = new CreateGDIMinigunnerCommand();
                createGdiUnit.X = commandBody.StartLocationXInWorldCoordinates;
                createGdiUnit.Y = commandBody.StartLocationYInWorldCoordinates;

                return createGdiUnit;

            }
            if (jsonAsyncSimulationCommand.CommandType.Equals(CreateNodMinigunnerCommand.CommandName))
            {

                CreateMinigunnerCommandBody commandBody =
                    JsonConvert.DeserializeObject<CreateMinigunnerCommandBody>(jsonAsyncSimulationCommand.JsonCommandData);

                CreateNodMinigunnerCommand command = new CreateNodMinigunnerCommand();
                command.X = commandBody.StartLocationXInWorldCoordinates;
                command.Y = commandBody.StartLocationYInWorldCoordinates;

                return command;

            }


            if (jsonAsyncSimulationCommand.CommandType.Equals(CreateGDIMinigunnerAtRandomLocationCommand.CommandName)) 
            {
                CreateGDIMinigunnerAtRandomLocationCommand command = new CreateGDIMinigunnerAtRandomLocationCommand();
                return command;
            }
            if (jsonAsyncSimulationCommand.CommandType.Equals(CreateNodMinigunnerAtRandomLocationCommand.CommandName))
            {
                CreateNodMinigunnerAtRandomLocationCommand command = new CreateNodMinigunnerAtRandomLocationCommand();
                return command;
            }


            else if (jsonAsyncSimulationCommand.CommandType.Equals(CreateJeepCommand.CommandName))
            {

                CreateJeepCommandBody commandBody =
                    JsonConvert.DeserializeObject<CreateJeepCommandBody>(jsonAsyncSimulationCommand.JsonCommandData);

                CreateJeepCommand createdUnit = new CreateJeepCommand();
                createdUnit.X = commandBody.StartLocationXInWorldCoordinates;
                createdUnit.Y = commandBody.StartLocationYInWorldCoordinates;

                return createdUnit;

            }
            else if (jsonAsyncSimulationCommand.CommandType.Equals(CreateMCVCommand.CommandName))
            {

                CreateMCVCommandBody commandBody =
                    JsonConvert.DeserializeObject<CreateMCVCommandBody>(jsonAsyncSimulationCommand.JsonCommandData);

                CreateMCVCommand createdUnit = new CreateMCVCommand();
                createdUnit.X = commandBody.StartLocationXInWorldCoordinates;
                createdUnit.Y = commandBody.StartLocationYInWorldCoordinates;

                return createdUnit;

            }

            else if (jsonAsyncSimulationCommand.CommandType.Equals(StartScenarioCommand.CommandName))
            {

                return new StartScenarioCommand();

            }

            else if (jsonAsyncSimulationCommand.CommandType.Equals(OrderUnitToAttackCommand.CommandName))
            {

                OrderUnitAttackCommandBody commandBody =
                    JsonConvert.DeserializeObject<OrderUnitAttackCommandBody>(jsonAsyncSimulationCommand.JsonCommandData);

                OrderUnitToAttackCommand aCommand = new OrderUnitToAttackCommand();
                aCommand.AttackerUnitId = commandBody.AttackerUnitId;
                aCommand.TargetUnitId = commandBody.TargetUnitId;

                return aCommand;

            }
            else if (jsonAsyncSimulationCommand.CommandType.Equals(OrderUnitToMoveCommand.CommandName))
            {

                OrderUnitMoveCommandBody commandBody =
                    JsonConvert.DeserializeObject<OrderUnitMoveCommandBody>(jsonAsyncSimulationCommand.JsonCommandData);

                OrderUnitToMoveCommand anEvent = new OrderUnitToMoveCommand();
                anEvent.UnitId = commandBody.UnitId;
                anEvent.DestinationXInWorldCoordinates = commandBody.DestinationLocationXInWorldCoordinates;
                anEvent.DestinationYInWorldCoordinates = commandBody.DestinationLocationYInWorldCoordinates;

                return anEvent;

            }
            else if (jsonAsyncSimulationCommand.CommandType.Equals(SetGameSpeedCommand.CommandName))
            {
                SetSimulationOptionsCommandBody commandBody =
                    JsonConvert.DeserializeObject<SetSimulationOptionsCommandBody>(jsonAsyncSimulationCommand.JsonCommandData);

                SimulationOptions.GameSpeed inputGameSpeed = SimulationOptions.ConvertGameSpeedStringToEnum(commandBody.GameSpeed);
                SetGameSpeedCommand aCommand = new SetGameSpeedCommand();
                aCommand.GameSpeed = inputGameSpeed;

                return aCommand;

            }
            else if (jsonAsyncSimulationCommand.CommandType.Equals(RemoveUnitCommand.CommandName))
            {

                RemoveUnitCommandBody commandBody =
                    JsonConvert.DeserializeObject<RemoveUnitCommandBody>(jsonAsyncSimulationCommand.JsonCommandData);

                RemoveUnitCommand command = new RemoveUnitCommand();
                command.UnitId = commandBody.UnitId;
                return command;
            }
            else if (jsonAsyncSimulationCommand.CommandType.Equals(ApplyDamageToUnitCommand.CommandName))
            {
                ApplyDamageToUnitCommandBody commandBody =
                    JsonConvert.DeserializeObject<ApplyDamageToUnitCommandBody>(jsonAsyncSimulationCommand.JsonCommandData);

                ApplyDamageToUnitCommand command = new ApplyDamageToUnitCommand();
                command.UnitId = commandBody.UnitId;
                command.DamageAmount = commandBody.DamageAmount;
                return command;
            }

            else
            {
                throw new Exception("Unknown CommandType:" + jsonAsyncSimulationCommand.CommandType);
            }


        }


        public void StartScenario(PlayerController playerController)
        {

            lock (simulationStateUpdateEventsHistory)
            {
                simulationStateUpdateEventsHistory.Clear();
            }

            SimulationMain.globalId = 1;

            gameWorld.StartScenario(playerController);
            
            PublishInitializeScenarioEvent(27, 23, gameWorld.gameMap.MapTileInstanceArray, gameWorld.terrainItemList);
        }


        internal Unit ApplyDamageToUnit(int unitId, int damageAmount)
        {
            return gameWorld.ApplyDamageToUnit(unitId, damageAmount);
        }
    }
}
