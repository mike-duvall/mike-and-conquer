using mike_and_conquer_monogame.main;


namespace mike_and_conquer_monogame.commands
{
    public class UpdateUnitViewHealthCommand : AsyncViewCommand
    {
        private int unitId;
        private int newHealthAmount;

        public UpdateUnitViewHealthCommand(int unitId, int newHealthAmount)
        {
            this.unitId = unitId;
            this.newHealthAmount = newHealthAmount;
        }

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.UpdateUnitViewHealth(unitId, newHealthAmount);

        }
    }
}
