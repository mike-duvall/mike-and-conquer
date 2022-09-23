using mike_and_conquer_monogame.main;


namespace mike_and_conquer_monogame.commands
{
    public class RemoveUnitViewCommand : AsyncViewCommand
    {


        private int unitId;

        public RemoveUnitViewCommand(int unitId)
        {
            this.unitId = unitId;
        }

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.RemoveUnitView(unitId);

        }
    }
}
