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
                ScenarioInitializedEventData scenarioInitializedEventData =
                    JsonConvert.DeserializeObject<ScenarioInitializedEventData>(anEvent.EventData);

                InitalizeUICommand initializedEventHandlerCommand = new InitalizeUICommand(scenarioInitializedEventData);
                mikeAndConquerGame.PostCommand(initializedEventHandlerCommand);

            }

        }
    }
}
