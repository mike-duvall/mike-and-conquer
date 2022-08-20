using mike_and_conquer_monogame.main;


namespace mike_and_conquer_monogame.commands
{
    public class AddMinigunnerViewCommand : AsyncViewCommand
    {


        private int unitId;
        private int x;
        private int y;

        public AddMinigunnerViewCommand(int unitId, int x, int y)
        {
            this.unitId = unitId;
            this.x = x;
            this.y = y;
        }

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.AddMinigunnerView(
                unitId, x, y);

        }
    }
}
