
using mike_and_conquer_monogame.main;

using mike_and_conquer_simulation.events;

namespace mike_and_conquer_monogame.commands
{
    public class InitializeScenarioCommand : AsyncViewCommand
    {


        private int id;
        private int x;
        private int y;
        private InitializeScenarioEventData data;

        public InitializeScenarioCommand(InitializeScenarioEventData data)
        {
            this.data = data;
        }

        protected override void ProcessImpl()

        {

            MikeAndConquerGame.instance.InitializeScenario(data);

        }
    }
}
