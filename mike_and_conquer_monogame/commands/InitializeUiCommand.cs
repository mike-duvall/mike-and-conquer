
using System.Collections.Generic;
using mike_and_conquer_monogame.main;

using mike_and_conquer_simulation.events;

namespace mike_and_conquer_monogame.commands
{
    public class InitializeUiCommand : AsyncViewCommand
    {

        private readonly ScenarioInitializedEventData eventData;

        public InitializeUiCommand(ScenarioInitializedEventData eventData)
        {
            this.eventData = eventData;
        }

        protected override void ProcessImpl()
        {

            MikeAndConquerGame.instance.InitializeUI(
                eventData.MapWidth,
                eventData.MapHeight,
                eventData.MapTileInstanceCreateEventDataList,
                eventData.TerrainItemCreateEventDataList);

        }
    }
}
