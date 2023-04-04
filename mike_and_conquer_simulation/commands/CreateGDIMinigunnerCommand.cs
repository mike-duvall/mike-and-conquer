using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.VisualBasic.CompilerServices;
using mike_and_conquer_simulation.main;

namespace mike_and_conquer_simulation.commands
{
    internal class CreateGDIMinigunnerCommand : AsyncSimulationCommand
    {

        public const string CommandName = "CreateGDIMinigunner";


        public int X { get; set; }
        public int Y { get; set; }

        public Nullable<int> health;


        protected override void ProcessImpl()
        {
            if(health == null)
            {
                result = SimulationMain.instance.CreateGDIMinigunner(X, Y);
            }
            else
            {
                result = SimulationMain.instance.CreateGDIMinigunner(X, Y, health.Value);
            }
            
        }

        public Minigunner GetMinigunner()
        {
            return (Minigunner) GetResult();
        }
    }
}