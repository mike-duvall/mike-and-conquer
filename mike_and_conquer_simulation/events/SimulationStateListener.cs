namespace mike_and_conquer_simulation.events
{
    public abstract class SimulationStateListener
    {
        public abstract void Update(SimulationStateUpdateEvent anEvent);
    }
}
