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
            previousTicks = DateTime.Now.Ticks;
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

    }
}
