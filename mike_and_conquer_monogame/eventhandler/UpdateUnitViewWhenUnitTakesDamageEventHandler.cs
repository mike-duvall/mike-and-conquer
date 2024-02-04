using mike_and_conquer_monogame.commands;
using mike_and_conquer_monogame.main;
using mike_and_conquer_simulation.events;
using Newtonsoft.Json;


namespace mike_and_conquer_monogame.eventhandler
{
    public class UpdateUnitViewWhenUnitTakesDamageEventHandler : SimulationStateListener
    {
        private MikeAndConquerGame mikeAndConquerGame = null;


        public UpdateUnitViewWhenUnitTakesDamageEventHandler(MikeAndConquerGame aGame)
        {
            this.mikeAndConquerGame = aGame;
        }

        public override void Update(SimulationStateUpdateEvent anEvent)
        {
            if (anEvent.EventType.Equals(UnitTookDamageEventData.EventType))
            {
                UnitTookDamageEventData eventData =
                    JsonConvert.DeserializeObject<UnitTookDamageEventData>(anEvent.EventData);

                UpdateUnitViewHealthCommand command = new UpdateUnitViewHealthCommand(eventData.UnitId, eventData.NewHealthAmount);

                mikeAndConquerGame.PostCommand(command);
            }


        }
    }
}
