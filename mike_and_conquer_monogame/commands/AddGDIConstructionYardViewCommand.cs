using mike_and_conquer_monogame.main;


namespace mike_and_conquer_monogame.commands
{
    public class AddGDIConstructionYardViewCommand : AsyncViewCommand
    {


        private int unitId;
        private int x;
        private int y;

        public AddGDIConstructionYardViewCommand(int unitId, int x, int y)
        {
            this.unitId = unitId;
            this.x = x;
            this.y = y;
        }

        protected override void ProcessImpl()
        {
//            MikeAndConquerGame.instance.AddMCVView(unitId, x, y);
            MikeAndConquerGame.instance.AddGDIConstructionYardView(unitId, x, y);

        }
    }
}
