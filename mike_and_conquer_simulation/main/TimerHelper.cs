// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace mike_and_conquer_simulation.main
{
    /// <summary>
    /// High-precision timer utility that provides accurate sleep functionality by querying
    /// Windows timer resolution and adjusting Thread.Sleep() calls accordingly.
    /// 
    /// The Windows timer has a resolution (granularity) that determines how precisely
    /// Thread.Sleep() can work. This class queries that resolution and uses it to
    /// sleep for the maximum safe duration without oversleeping.
    /// </summary>
    internal static class TimerHelper
    {
        /// <summary>
        /// Native Windows API call to query the system timer resolution.
        /// Returns resolution values in 100-nanosecond units (Windows time units).
        /// 
        /// MinimumResolution: The finest resolution the timer can achieve
        /// MaximumResolution: The coarsest resolution the timer can achieve  
        /// CurrentResolution: The currently active timer resolution
        /// </summary>
        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtQueryTimerResolution(out uint MinimumResolution, out uint MaximumResolution, out uint CurrentResolution);

        /// <summary>
        /// The threshold below which we shouldn't attempt to sleep at all.
        /// Calculated as: 1ms + (MaximumResolution / 10000.0)
        /// 
        /// If the requested sleep time is below this threshold, Thread.Sleep()
        /// would be unreliable and might sleep much longer than intended.
        /// </summary>
        private static readonly double LowestSleepThreshold;

        /// <summary>
        /// Static constructor that initializes timer resolution thresholds.
        /// 
        /// This runs once when the class is first accessed and calculates:
        /// - LowestSleepThreshold: Below this value, don't attempt Thread.Sleep()
        /// - HighestSleepThreshold: The upper bound for safe sleep calculations
        /// 
        /// The division by 10000.0 converts from Windows time units (100ns intervals)
        /// to milliseconds: 1 millisecond = 10,000 * 100 nanoseconds
        /// </summary>
        static TimerHelper()
        {
            uint min, max, current;
            NtQueryTimerResolution(out min, out max, out current);

            // LowestSleepThreshold = 1ms + maximum timer resolution
            // If we try to sleep for less than this, Thread.Sleep() becomes unreliable
            LowestSleepThreshold = 1.0 + (max / 10000.0);


            // HighestSleepThreshold represents the finest resolution available
            // This is calculated but not currently used in the implementation
            double HighestSleepThreshold = 1.0 + (min / 10000.0);

        }

        /// <summary>
        /// Returns the current active timer resolution in milliseconds.
        /// 
        /// This queries the Windows system to get the currently active timer resolution,
        /// which may change dynamically based on system load and other applications.
        /// The resolution determines the granularity of Thread.Sleep() calls.
        /// </summary>
        /// <returns>Current timer resolution in milliseconds</returns>
        public static double GetCurrentResolution()
        {
            uint min, max, current;
            NtQueryTimerResolution(out min, out max, out current);
            return current / 10000.0;
        }


        /// <summary>
        /// Returns the minimum (finest) timer resolution available on this system in milliseconds.
        /// 
        /// This represents the highest precision the Windows timer can achieve.
        /// Unlike GetCurrentResolution(), this value is constant for the system
        /// and represents the theoretical best-case timing precision available.
        /// </summary>
        /// <returns>Minimum timer resolution in milliseconds</returns>
        public static double GetMinResolution()
        {
            uint min, max, current;
            NtQueryTimerResolution(out min, out max, out current);
            return min / 10000.0;

        }



        /// <summary>
        /// Sleeps as long as possible without exceeding the specified period
        /// </summary>
        // public static void SleepForNoMoreThan(double milliseconds, ILogger logger)
        // {
        //     // Assumption is that Thread.Sleep(t) will sleep for at least (t), and at most (t + timerResolution)
        //     if (milliseconds < LowestSleepThreshold)
        //         return;
        //     // double currentResolution = GetCurrentResolution();
        //     double currentResolution = GetMinResolution();
        //
        //     var sleepTime = (int)(milliseconds - currentResolution);
        //     // logger.LogInformation("sleepTime=" + sleepTime+ ", currentResolution=" + currentResolution);
        //
        //     if (sleepTime > 0)
        //     {
        //         logger.LogInformation("sleeping for sleepTime=" + sleepTime);
        //         long startTicks = DateTime.Now.Ticks;
        //         Thread.Sleep(sleepTime);
        //         long endTicks = DateTime.Now.Ticks;
        //         long actualSleepTimeInTicks = endTicks - startTicks;
        //         long acutSleepTimeInMilliseconds = actualSleepTimeInTicks / TimeSpan.TicksPerMillisecond;
        //         logger.LogInformation("actualSleepTime=" + acutSleepTimeInMilliseconds);
        //
        //     }
        //         
        // }

        /// <summary>
        /// Sleeps for no more than the specified duration using current timer resolution.
        /// 
        /// This method attempts to sleep for close to the requested time without oversleeping.
        /// It subtracts the current timer resolution from the requested sleep time because
        /// Thread.Sleep(t) can sleep anywhere from t to (t + timer_resolution) milliseconds.
        /// 
        /// By sleeping for (requested_time - timer_resolution), we ensure we sleep for
        /// at most the requested time, leaving the remaining precision work to busy-waiting.
        /// </summary>
        /// <param name="milliseconds">Desired sleep duration in milliseconds</param>
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

        // 2025-07-27 note - Is it possible that simply using GetMinResolution() is more reliable than GetCurrentResolution() 
        // because it just results in more busy waiting?
        //
        // Another note, is that I added logging to a different method above, while debugging this issue, and I found the 
        // presence of the logging actually substantially affected the timings as well

        /// <summary>
        /// Sleeps for no more than the specified duration using minimum timer resolution.
        /// 
        /// This is similar to SleepForNoMoreThan() but uses the minimum (finest) timer
        /// resolution instead of the current resolution. This tends to be more conservative
        /// and reliable across different system configurations.
        /// 
        /// The minimum resolution represents the best-case scenario for timer precision,
        /// so using it provides more consistent behavior regardless of current system state.
        /// </summary>
        /// <param name="milliseconds">Desired sleep duration in milliseconds</param>
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
