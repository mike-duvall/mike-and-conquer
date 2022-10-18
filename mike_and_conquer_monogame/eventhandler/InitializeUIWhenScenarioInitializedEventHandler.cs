using mike_and_conquer_monogame.commands;

using mike_and_conquer_monogame.main;
using mike_and_conquer_simulation.events;
using Newtonsoft.Json;


namespace mike_and_conquer_monogame.eventhandler
{
    public class InitializeUIWhenScenarioInitializedEventHandler : SimulationStateListener
    {
        private MikeAndConquerGame mikeAndConquerGame = null;


        public InitializeUIWhenScenarioInitializedEventHandler(MikeAndConquerGame aGame)
        {
            this.mikeAndConquerGame = aGame;
        }

        public override void Update(SimulationStateUpdateEvent anEvent)
        {

            if (anEvent.EventType.Equals(ScenarioInitializedEventData.EventType))
            {
                ScenarioInitializedEventData eventData =
                    JsonConvert.DeserializeObject<ScenarioInitializedEventData>(anEvent.EventData);

                InitiInitalizeUiCommand command = new InitiInitalizeUiCommand(
                    eventData.MapWidth,
                    eventData.MapHeight,
                    eventData.MapTileInstanceCreateEventDataList,
                    eventData.TerrainItemCreateEventDataList);
                mikeAndConquerGame.PostCommand(command);

            }

        }
    }
}
