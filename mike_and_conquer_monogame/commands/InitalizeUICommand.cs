
using mike_and_conquer_monogame.main;

using mike_and_conquer_simulation.events;

namespace mike_and_conquer_monogame.commands
{
    public class InitalizeUICommand : AsyncViewCommand
    {


        private int id;
        private int x;
        private int y;
        private ScenarioInitializedEventData data;

        public InitalizeUICommand(ScenarioInitializedEventData data)
        {
            this.data = data;
        }

        protected override void ProcessImpl()
        {

            MikeAndConquerGame.instance.InitializeUI(data);

        }
    }
}
