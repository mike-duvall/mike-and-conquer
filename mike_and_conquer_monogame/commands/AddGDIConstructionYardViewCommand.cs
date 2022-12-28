using mike_and_conquer_monogame.main;

using GDIConstructionYardCreatedEventData = mike_and_conquer_simulation.events.GDIConstructionYardCreatedEventData;
namespace mike_and_conquer_monogame.commands
{
    public class AddGDIConstructionYardViewCommand : AsyncViewCommand
    {


        private int unitId;
        private int x;
        private int y;

        public AddGDIConstructionYardViewCommand(GDIConstructionYardCreatedEventData eventData )
        {
            this.unitId = -1;
            this.x = eventData.X;
            this.y = eventData.Y;
        }

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.AddGDIConstructionYardView(unitId, x, y);
        }
    }
}
