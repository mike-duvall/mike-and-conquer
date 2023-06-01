using System;
using ManualResetEvent = System.Threading.ManualResetEvent;


namespace mike_and_conquer_simulation.commands
{
    public abstract class AsyncSimulationCommand
    {
        protected Object result;
        private ManualResetEvent condition;
        private Exception thrownException;

        public AsyncSimulationCommand()
        {
            this.result = null;
            bool signaled = false;
            this.condition = new ManualResetEvent(signaled);
            this.thrownException = null;
        }

        protected abstract void ProcessImpl();


        public void Process()
        {
            try
            {
                ProcessImpl();
            }
            catch (Exception e)
            {
                thrownException = e;
            }

            condition.Set();
        }



        // TODO Consider making an abstract SetResult() method
        // to force people to make a conscious decision on 
        // setting a result
        public Object GetResult()
        {
            WaitUntilCompleted();
            return result;
        }

        public void WaitUntilCompleted()
        {
            condition.WaitOne();
            if (thrownException != null)
            {
                throw thrownException;
            }

        }


        public Exception ThrownException
        {
            get { return thrownException; }
        }



    }
}
