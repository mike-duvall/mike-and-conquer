using mike_and_conquer_monogame.commands;
using mike_and_conquer_monogame.main;
using mike_and_conquer_simulation.events;
using Newtonsoft.Json;

namespace mike_and_conquer_monogame.eventhandler
{
    public class RemovePlannedStepViewWhenUnitArrivesAtPathStepEventHandler : SimulationStateListener
    {
        private MikeAndConquerGame mikeAndConquerGame = null;


        public RemovePlannedStepViewWhenUnitArrivesAtPathStepEventHandler(MikeAndConquerGame aGame)
        {
            this.mikeAndConquerGame = aGame;
        }

        public override void Update(SimulationStateUpdateEvent anEvent)
        {
            if (anEvent.EventType.Equals(UnitArrivedAtPathStepEventData.EventType))
            {
                UnitArrivedAtPathStepEventData eventData =
                    JsonConvert.DeserializeObject<UnitArrivedAtPathStepEventData>(anEvent.EventData);


                RemovePlannedStepViewCommand viewCommand = new RemovePlannedStepViewCommand(eventData.UnitId, eventData.PathStep);
                mikeAndConquerGame.PostCommand(viewCommand);

            }


        }
    }
}
