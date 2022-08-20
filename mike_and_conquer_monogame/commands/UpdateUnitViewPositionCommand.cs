
using mike_and_conquer_monogame.main;

using mike_and_conquer_simulation.events;

namespace mike_and_conquer_monogame.commands
{
    public class UpdateUnitViewPositionCommand : AsyncViewCommand
    {


        private UnitPositionChangedEventData unitPositionChangedEventData;

        public UpdateUnitViewPositionCommand(UnitPositionChangedEventData data)
        {
            this.unitPositionChangedEventData = data;
        }

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.UpdateUnitViewPosition(unitPositionChangedEventData);
        }
    }
}
