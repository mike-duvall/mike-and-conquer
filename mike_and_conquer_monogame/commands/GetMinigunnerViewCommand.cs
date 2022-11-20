using mike_and_conquer_monogame.gameview;
using mike_and_conquer_monogame.main;


namespace mike_and_conquer_monogame.commands
{
    public class GetMinigunnerViewCommand : AsyncViewCommand
    {

        private GameWorldView gameWorldView;
        private int unitId;


        public GetMinigunnerViewCommand(GameWorldView gameWorldView, int unitId)
        {
            this.gameWorldView = gameWorldView;
            this.unitId = unitId;
        }

        protected override void ProcessImpl()
        {
            result = gameWorldView.GetUnitViewById(unitId);
        }
    }
}
