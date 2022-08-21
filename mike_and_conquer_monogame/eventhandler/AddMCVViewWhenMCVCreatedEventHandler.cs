using mike_and_conquer_monogame.commands;
using mike_and_conquer_monogame.main;
using mike_and_conquer_simulation.events;
using Newtonsoft.Json;


namespace mike_and_conquer_monogame.eventhandler
{
    public class AddMCVViewWhenMCVCreatedEventHandler : SimulationStateListener
    {
        private MikeAndConquerGame mikeAndConquerGame = null;


        public AddMCVViewWhenMCVCreatedEventHandler(MikeAndConquerGame aGame)
        {
            this.mikeAndConquerGame = aGame;
        }

        public override void Update(SimulationStateUpdateEvent anEvent)
        {
            if (anEvent.EventType.Equals(MCVCreateEventData.EventType))
            {
                MCVCreateEventData eventData =
                    JsonConvert.DeserializeObject<MCVCreateEventData>(anEvent.EventData);

                AddMCVViewCommand viewCommand = new AddMCVViewCommand(eventData.UnitId, eventData.X, eventData.Y);

                mikeAndConquerGame.PostCommand(viewCommand);

            }


        }
    }
}
