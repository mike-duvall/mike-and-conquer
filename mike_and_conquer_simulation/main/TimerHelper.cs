// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace mike_and_conquer_simulation.main
{
    internal static class TimerHelper
    {
        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtQueryTimerResolution(out uint MinimumResolution, out uint MaximumResolution, out uint CurrentResolution);

        private static readonly double LowestSleepThreshold;

        static TimerHelper()
        {
            uint min, max, current;
            NtQueryTimerResolution(out min, out max, out current);
            LowestSleepThreshold = 1.0 + (max / 10000.0);


            double HighestSleepThreshold = 1.0 + (min / 10000.0);
            // int x = 3;

        }

        /// <summary>
        /// Returns the current timer resolution in milliseconds
        /// </summary>
        public static double GetCurrentResolution()
        {
            uint min, max, current;
            NtQueryTimerResolution(out min, out max, out current);
            return current / 10000.0;
        }


        public static double GetMinResolution()
        {
            uint min, max, current;
            NtQueryTimerResolution(out min, out max, out current);
            return min / 10000.0;

        }



        /// <summary>
        /// Sleeps as long as possible without exceeding the specified period
        /// </summary>
        public static void SleepForNoMoreThan(double milliseconds, ILogger logger)
        {
            // Assumption is that Thread.Sleep(t) will sleep for at least (t), and at most (t + timerResolution)
            if (milliseconds < LowestSleepThreshold)
                return;
            // double currentResolution = GetCurrentResolution();
            double currentResolution = GetMinResolution();

            var sleepTime = (int)(milliseconds - currentResolution);
            // logger.LogInformation("sleepTime=" + sleepTime+ ", currentResolution=" + currentResolution);

            if (sleepTime > 0)
            {
                logger.LogInformation("sleeping for sleepTime=" + sleepTime);
                long startTicks = DateTime.Now.Ticks;
                Thread.Sleep(sleepTime);
                long endTicks = DateTime.Now.Ticks;
                long actualSleepTimeInTicks = endTicks - startTicks;
                long acutSleepTimeInMilliseconds = actualSleepTimeInTicks / TimeSpan.TicksPerMillisecond;
                logger.LogInformation("actualSleepTime=" + acutSleepTimeInMilliseconds);

            }
                
        }

        public static void SleepForNoMoreThan(double milliseconds)
        {
            // Assumption is that Thread.Sleep(t) will sleep for at least (t), and at most (t + timerResolution)
            if (milliseconds < LowestSleepThreshold)
                return;
            var sleepTime = (int)(milliseconds - GetCurrentResolution());
            if (sleepTime > 0)
                Thread.Sleep(sleepTime);
        }

        // This is same as SleepForNoMoreThan() above, but uses GetMinResolution() instead of GetCurrentResolution()
        // On my HP Omen laptop, the timing tests failed unless I did this.  
        // On Lenova laptop, NtQueryTimerResolution reported these values:
        //      min	156250	uint
        //      max	5000	uint
        //      current	156239	uint
        //      HighestSleepThreshold	16.625	double
        //      LowestSleepThreshold = 1.5
        // whereas on HP Omen laptop, NtQueryTimerResolution reported these values:
        //      min	156250	uint
        //      max	5000	uint
        //      current	9973	uint
        //      HighestSleepThreshold	16.625	double
        //      LowestSleepThreshold = 1.5
        // For the Lenovo, where the tests always passed, "current" was always very close to "min",
        // whereas on the HP Omen, "current" was always different than "min"
        // But it makes me wonder if the actual "current", or what was being used, was actually in fact closer to "min"
        // Hence the tweak to this version of the method to use GetMinResolution() rather than GetCurrentResolution()
        // Once I switched to GetMinResolution(), everything passed just fine on HP Omen
        //
        // Another note, is that I added logging to a different method above, while debugging this issue, and I found the 
        // presence of the logging actually substantially affected the timings as well
        public static void SleepForNoMoreThan2(double milliseconds)
        {
            // Assumption is that Thread.Sleep(t) will sleep for at least (t), and at most (t + timerResolution)
            if (milliseconds < LowestSleepThreshold)
                return;
            var sleepTime = (int)(milliseconds - GetMinResolution());
            if (sleepTime > 0)
                Thread.Sleep(sleepTime);
        }

    }
}
