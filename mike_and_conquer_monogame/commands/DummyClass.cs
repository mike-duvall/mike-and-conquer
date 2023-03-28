using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using mike_and_conquer_monogame.main;
using Serilog;
using Serilog.Core;

namespace commands
{



    public class DummyClass
    {

        private static readonly Serilog.ILogger logger = Log.ForContext<DummyClass>();

        public static void DoSomeStuff()
        {
            logger.Information("DummyClass::DoSomeStuff:   Hello, Serilog!");

        }

    }
}
