
using mike_and_conquer_monogame.gameview;
using mike_and_conquer_monogame.main;

namespace mike_and_conquer_monogame.commands
{
    internal class GetUnitViewCommand : AsyncViewCommand
    {

        private int unitId;



        public GetUnitViewCommand(int unitId)
        {
            this.unitId = unitId;
        }

        protected override void ProcessImpl()
        {
            result = MikeAndConquerGame.instance.GetUnitViewById(unitId);

        }

        public UnitView GetUnitView()
        {
            return (UnitView)GetResult();
        }


    }
}
