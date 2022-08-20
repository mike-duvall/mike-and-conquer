using mike_and_conquer_monogame.commands;

using mike_and_conquer_monogame.main;
using mike_and_conquer_simulation.events;
using Newtonsoft.Json;


namespace mike_and_conquer_monogame.eventhandler
{
    public class InitializeScenarioEventHandler : SimulationStateListener
    {
        private MikeAndConquerGame mikeAndConquerGame = null;


        public InitializeScenarioEventHandler(MikeAndConquerGame aGame)
        {
            this.mikeAndConquerGame = aGame;
        }

        public override void Update(SimulationStateUpdateEvent anEvent)
        {

            if (anEvent.EventType.Equals(InitializeScenarioEventData.EventType))
            {
                InitializeScenarioEventData initializeScenarioEventData =
                    JsonConvert.DeserializeObject<InitializeScenarioEventData>(anEvent.EventData);

                InitializeScenarioCommand command = new InitializeScenarioCommand(initializeScenarioEventData);
                mikeAndConquerGame.PostCommand(command);

            }

        }
    }
}
