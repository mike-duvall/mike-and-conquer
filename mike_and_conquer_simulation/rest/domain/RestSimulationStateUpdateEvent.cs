using System;

namespace mike_and_conquer_simulation.rest.domain
{
    public class RestSimulationStateUpdateEvent
    {
        // public DateTime Date { get; set; }
        //
        // public int TemperatureC { get; set; }
        //
        // public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        //
        // public string Summary { get; set; }


        public string EventType { get; set; }

        public string EventData { get; set; }


        // public int X { get; set; }
        // public int Y { get; set; }
        //
        // public int UnitId { get; set; }
    }
}
