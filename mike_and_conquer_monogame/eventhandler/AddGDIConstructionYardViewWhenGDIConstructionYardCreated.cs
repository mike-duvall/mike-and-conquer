using mike_and_conquer_monogame.commands;
using mike_and_conquer_monogame.main;
using mike_and_conquer_simulation.events;
using Newtonsoft.Json;


namespace mike_and_conquer_monogame.eventhandler
{
    public class AddGDIConstructionYardViewWhenGDIConstructionYardCreated : SimulationStateListener
    {
        private MikeAndConquerGame mikeAndConquerGame = null;


        public AddGDIConstructionYardViewWhenGDIConstructionYardCreated(MikeAndConquerGame aGame)
        {
            this.mikeAndConquerGame = aGame;
        }

        public override void Update(SimulationStateUpdateEvent anEvent)
        {
            if (anEvent.EventType.Equals(GDIConstructionYardCreatedEventData.EventType))
            {
                GDIConstructionYardCreatedEventData eventData =
                    JsonConvert.DeserializeObject<GDIConstructionYardCreatedEventData>(anEvent.EventData);

                // AddMCVViewCommand viewCommand = new AddMCVViewCommand(eventData.UnitId, eventData.X, eventData.Y);
                AddGDIConstructionYardViewCommand command = new AddGDIConstructionYardViewCommand(
                    -1,
                    eventData.X,
                    eventData.Y);

                mikeAndConquerGame.PostCommand(command);

            }


        }
    }
}
