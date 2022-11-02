using mike_and_conquer_monogame.main;

namespace mike_and_conquer_monogame.commands.ui
{
    public class LeftClickMCVCommand : AsyncViewCommand
    {


        private int unitId;


        public const string CommandName = "LeftClickMCV";

        public LeftClickMCVCommand(int unitId)
        {
            this.unitId = unitId;

        }

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.LeftClickMCV(unitId);

        }
    }
}
