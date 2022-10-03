using mike_and_conquer_monogame.commands;
using mike_and_conquer_monogame.main;
using mike_and_conquer_simulation.events;
using Newtonsoft.Json;


namespace mike_and_conquer_monogame.eventhandler
{
    public class RemoveUnitViewWhenUnitDeletedEventHandler : SimulationStateListener
    {
        private MikeAndConquerGame mikeAndConquerGame = null;


        public RemoveUnitViewWhenUnitDeletedEventHandler(MikeAndConquerGame aGame)
        {
            this.mikeAndConquerGame = aGame;
        }

        public override void Update(SimulationStateUpdateEvent anEvent)
        {
            if (anEvent.EventType.Equals(UnitDeletedEventData.EventType))
            {
                // UnitDeletedEventData eventData =
                //     JsonConvert.DeserializeObject < UnitDeletedEventData eventData =
                //     > (anEvent.EventData);

                // UnitArrivedAtPathStepEventData eventData =
                //     JsonConvert.DeserializeObject<UnitArrivedAtPathStepEventData>(anEvent.EventData);

                UnitDeletedEventData eventData =
                    JsonConvert.DeserializeObject<UnitDeletedEventData>(anEvent.EventData);


                // AddMinigunnerViewCommand command = new AddMinigunnerViewCommand(eventData.UnitId, eventData.X, eventData.Y);
                RemoveUnitViewCommand command = new RemoveUnitViewCommand(eventData.UnitId);

                mikeAndConquerGame.PostCommand(command);
                
            }


        }
    }
}
