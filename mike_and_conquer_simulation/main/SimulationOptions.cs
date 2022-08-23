

namespace mike_and_conquer_simulation.main

{
    internal class SimulationOptions
    {

        public enum GameSpeed
        {
            Slowest = 252,  // verified
            Slower = 126, // verified
            Slow = 84,  // verified
            Moderate = 63, // verified
            Normal = 42,  // verified
            Fast = 30, // verified
            // Fast = 31, // good too
            Faster = 25, // verified
            Fastest = 23  // verified
        }


        public GameSpeed CurrentGameSpeed = GameSpeed.Moderate;

    }
}

