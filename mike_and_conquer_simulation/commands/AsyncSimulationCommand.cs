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

        //        protected abstract GameState ProcessImpl();
        protected abstract void ProcessImpl();



        // public GameState Process()
        // {
        //     GameState newGameState = null;
        //     try
        //     {
        //         newGameState = ProcessImpl();
        //     }
        //     catch (Exception e)
        //     {
        //         thrownException = e;
        //     }
        //
        //     condition.Set();
        //     return newGameState;
        // }
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
            condition.WaitOne();
            if (thrownException != null)
            {
                throw thrownException;
            }
            else
            {
                return result;
            }
        }

        public Exception ThrownException
        {
            get { return thrownException; }
        }



    }
}
