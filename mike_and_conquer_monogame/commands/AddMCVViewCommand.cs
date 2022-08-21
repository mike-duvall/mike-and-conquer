using mike_and_conquer_monogame.main;


namespace mike_and_conquer_monogame.commands
{
    public class AddMCVViewCommand : AsyncViewCommand
    {


        private int unitId;
        private int x;
        private int y;

        public AddMCVViewCommand(int unitId, int x, int y)
        {
            this.unitId = unitId;
            this.x = x;
            this.y = y;
        }

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.AddMCVView(unitId, x, y);

        }
    }
}
