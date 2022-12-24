

using SimulationStateListener = mike_and_conquer_simulation.events.SimulationStateListener;
using MikeAndConquerGame = mike_and_conquer_monogame.main.MikeAndConquerGame;
using SimulationStateUpdateEvent = mike_and_conquer_simulation.events.SimulationStateUpdateEvent;

using BuildingMinigunnerPercentCompletedEventData = mike_and_conquer_simulation.events.BuildingMinigunnerPercentCompletedEventData;
using JsonConvert = Newtonsoft.Json.JsonConvert;

using UpdateMinigunnerPercentCompletedCommand = mike_and_conquer_monogame.commands.UpdateMinigunnerPercentCompletedCommand;

namespace mike_and_conquer_monogame.eventhandler
{
    public class UpdateMinigunnerPercentBuildCompleted : SimulationStateListener
    {
        private MikeAndConquerGame mikeAndConquerGame = null;


        public UpdateMinigunnerPercentBuildCompleted(MikeAndConquerGame aGame)
        {
            this.mikeAndConquerGame = aGame;
        }


        public override void Update(SimulationStateUpdateEvent anEvent)
        {
            if (anEvent.EventType.Equals(BuildingMinigunnerPercentCompletedEventData.EventType))
            {
                BuildingMinigunnerPercentCompletedEventData eventData =
                    JsonConvert.DeserializeObject<BuildingMinigunnerPercentCompletedEventData>(anEvent.EventData);

                UpdateMinigunnerPercentCompletedCommand command = new
                    UpdateMinigunnerPercentCompletedCommand(eventData.PercentCompleted);

                mikeAndConquerGame.PostCommand(command);

            }


        }

    }
}
