using System;

namespace mike_and_conquer_simulation.rest.domain
{
    internal class RestSimulationOptions
    {

        public String GameSpeed { get; }

        public RestSimulationOptions(String gameSpeed)
        {
            this.GameSpeed = gameSpeed;
        }
    }
}
