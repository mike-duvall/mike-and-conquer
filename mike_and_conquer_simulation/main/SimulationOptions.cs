
using System.Collections;
using String = System.String;
using Exception = System.Exception;

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

        internal static GameSpeed ConvertGameSpeedStringToEnum(String gameSpeedAsString)
        {
            if (gameSpeedAsString == "Slowest") return SimulationOptions.GameSpeed.Slowest;
            if (gameSpeedAsString == "Slower") return SimulationOptions.GameSpeed.Slower;
            if (gameSpeedAsString == "Slow") return SimulationOptions.GameSpeed.Slow;
            if (gameSpeedAsString == "Moderate") return SimulationOptions.GameSpeed.Moderate;
            if (gameSpeedAsString == "Normal") return SimulationOptions.GameSpeed.Normal;
            if (gameSpeedAsString == "Fast") return SimulationOptions.GameSpeed.Fast;
            if (gameSpeedAsString == "Faster") return SimulationOptions.GameSpeed.Faster;
            if (gameSpeedAsString == "Fastest") return SimulationOptions.GameSpeed.Fastest;

            throw new Exception("Could not map game speed string of:" + gameSpeedAsString);
        }

        // internal GameSpeed AsString()
        // {
        //     switch
        //     if (gameSpeedAsString == "Slowest") return SimulationOptions.GameSpeed.Slowest;
        //     if (gameSpeedAsString == "Slower") return SimulationOptions.GameSpeed.Slower;
        //     if (gameSpeedAsString == "Slow") return SimulationOptions.GameSpeed.Slow;
        //     if (gameSpeedAsString == "Moderate") return SimulationOptions.GameSpeed.Moderate;
        //     if (gameSpeedAsString == "Normal") return SimulationOptions.GameSpeed.Normal;
        //     if (gameSpeedAsString == "Fast") return SimulationOptions.GameSpeed.Fast;
        //     if (gameSpeedAsString == "Faster") return SimulationOptions.GameSpeed.Faster;
        //     if (gameSpeedAsString == "Fastest") return SimulationOptions.GameSpeed.Fastest;
        //
        //     throw new Exception("Could not map game speed string of:" + gameSpeedAsString);
        // }



    }
}

