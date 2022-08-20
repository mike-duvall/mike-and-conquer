using mike_and_conquer_monogame.commands;

using mike_and_conquer_monogame.main;
using mike_and_conquer_simulation.events;
using Newtonsoft.Json;


namespace mike_and_conquer_monogame.eventhandler
{
    public class UpdateUnitViewPositionWhenUnitPositionChangedEventHandler : SimulationStateListener
    {
        private MikeAndConquerGame mikeAndConquerGame = null;


        public UpdateUnitViewPositionWhenUnitPositionChangedEventHandler(MikeAndConquerGame aGame)
        {
            this.mikeAndConquerGame = aGame;
        }

        public override void Update(SimulationStateUpdateEvent anEvent)
        {

            if (anEvent.EventType.Equals(UnitPositionChangedEventData.EventType))
            {
                UnitPositionChangedEventData unitPositionChangedEventData =
                    JsonConvert.DeserializeObject<UnitPositionChangedEventData>(anEvent.EventData);

                UpdateUnitViewPositionCommand command = new UpdateUnitViewPositionCommand(unitPositionChangedEventData);
                mikeAndConquerGame.PostCommand(command);

            }

        }
    }
}
