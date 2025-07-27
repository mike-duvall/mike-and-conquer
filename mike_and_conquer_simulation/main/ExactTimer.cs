using System;

namespace mike_and_conquer_simulation.main
{
    /// <summary>
    /// A high-precision timer that combines Thread.Sleep() with busy-waiting 
    /// to achieve exact timing intervals with minimal CPU overhead.
    /// </summary>
    public class ExactTimer
    {
        private long previousTicks;

        /// <summary>
        /// Initializes a new instance of the ExactTimer class.
        /// </summary>
        public ExactTimer()
        {
            previousTicks = 0;
        }

        /// <summary>
        /// Waits for exactly the specified number of milliseconds since the last call to WaitForExactly.
        /// Uses a combination of Thread.Sleep() and busy-waiting for precise timing.
        /// </summary>
        /// <param name="sleepTimeInMilliseconds">The exact time to wait in milliseconds</param>
        public void WaitForExactly(int sleepTimeInMilliseconds)
        {
            // First, sleep for most of the time using TimerHelper to minimize CPU usage
            TimerHelper.SleepForNoMoreThan2(sleepTimeInMilliseconds);

            // Then use busy-waiting to achieve exact timing
            bool doneWaiting = false;
            long currentTicks = -1;

            while (!doneWaiting)
            {
                currentTicks = DateTime.Now.Ticks;
                long delta = (currentTicks - previousTicks) / TimeSpan.TicksPerMillisecond;
                if (delta >= sleepTimeInMilliseconds)
                {
                    doneWaiting = true;
                }
            }

            // Update previousTicks for the next call
            previousTicks = currentTicks;
        }

        /// <summary>
        /// Resets the timer's internal state. 
        /// The next call to WaitForExactly will start timing from when this method is called.
        /// </summary>
        public void Reset()
        {
            previousTicks = DateTime.Now.Ticks;
        }

        /// <summary>
        /// Gets the time elapsed since the last WaitForExactly call or Reset in milliseconds.
        /// </summary>
        /// <returns>Elapsed time in milliseconds</returns>
        public long GetElapsedMilliseconds()
        {
            long currentTicks = DateTime.Now.Ticks;
            return (currentTicks - previousTicks) / TimeSpan.TicksPerMillisecond;
        }

        /// <summary>
        /// Gets the time elapsed since the last WaitForExactly call or Reset in ticks.
        /// </summary>
        /// <returns>Elapsed time in ticks</returns>
        public long GetElapsedTicks()
        {
            long currentTicks = DateTime.Now.Ticks;
            return currentTicks - previousTicks;
        }
    }
}
