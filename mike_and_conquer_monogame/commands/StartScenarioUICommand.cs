using mike_and_conquer_monogame.main;

using mike_and_conquer_simulation.gameworld;

namespace mike_and_conquer_monogame.commands
{
    public class StartScenarioUICommand : AsyncViewCommand
    {
        private PlayerController playerController;


        public const string CommandName = "StartScenario";

        public StartScenarioUICommand(PlayerController playerController)
        {
            this.playerController = playerController;
        }

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.StartScenario(playerController);

        }
    }
}
