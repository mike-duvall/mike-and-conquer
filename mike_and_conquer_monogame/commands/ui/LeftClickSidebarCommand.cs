using mike_and_conquer_monogame.main;

namespace mike_and_conquer_monogame.commands.ui
{
    public class LeftClickSidebarCommand : AsyncViewCommand
    {


        private string sidebarIconName;

        public const string CommandName = "LeftClickSidebar";

        public LeftClickSidebarCommand(string sidebarIconName)
        {
            this.sidebarIconName = sidebarIconName;
        }

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.LeftClickSidebar(sidebarIconName);

        }
    }
}
