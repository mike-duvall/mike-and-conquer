using mike_and_conquer_monogame.commands;
using mike_and_conquer_monogame.main;
using mike_and_conquer_simulation.events;
using Newtonsoft.Json;


namespace mike_and_conquer_monogame.eventhandler
{
    public class AddJeepViewWhenJeepCreatedEventHandler : SimulationStateListener
    {
        private MikeAndConquerGame mikeAndConquerGame = null;


        public AddJeepViewWhenJeepCreatedEventHandler(MikeAndConquerGame aGame)
        {
            this.mikeAndConquerGame = aGame;
        }

        public override void Update(SimulationStateUpdateEvent anEvent)
        {
            if (anEvent.EventType.Equals(JeepCreateEventData.EventType))
            {
                JeepCreateEventData eventData =
                    JsonConvert.DeserializeObject<JeepCreateEventData>(anEvent.EventData);

                AddJeepViewCommand viewCommand = new AddJeepViewCommand(eventData.UnitId, eventData.X, eventData.Y);

                mikeAndConquerGame.PostCommand(viewCommand);

            }


        }
    }
}
