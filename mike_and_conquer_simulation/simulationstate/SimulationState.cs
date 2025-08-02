
namespace mike_and_conquer_simulation.simulationstate
{
    internal abstract class SimulationState
    {
        private SimulationState nextState;

        public SimulationState()
        {
            nextState = this;
        }

        public SimulationState Update()
        {
            UpdateImpl();
            return nextState;
        }

        protected abstract void UpdateImpl();

        public void SetNextState(SimulationState nextState)
        {
            this.nextState = nextState;
        }



    }
}
