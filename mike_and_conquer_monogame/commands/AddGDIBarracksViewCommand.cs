
using mike_and_conquer_monogame.main;
using GDIBarracksPlacedEventData = mike_and_conquer_simulation.events.GDIBarracksPlacedEventData;



namespace mike_and_conquer_monogame.commands
{
    public class AddGDIBarracksViewCommand : AsyncViewCommand
    {


        private int unitId;
        private int x;
        private int y;

        public AddGDIBarracksViewCommand(GDIBarracksPlacedEventData eventData)
        {


            this.unitId = -1;
            this.x = eventData.X;
            this.y = eventData.Y;
        }

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.AddGDIBarracksView(unitId, x, y);
        }
    }
}
