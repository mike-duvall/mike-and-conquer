using mike_and_conquer_monogame.main;


namespace mike_and_conquer_monogame.commands
{
    public class SelectUnitCommand : AsyncViewCommand
    {


        private int unitId;


        public const string CommandName = "SelectUnit";

        public SelectUnitCommand(int unitId)
        {
            this.unitId = unitId;
        }

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.SelectUnit(unitId);

        }
    }
}
