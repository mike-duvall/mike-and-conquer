using mike_and_conquer_monogame.main;


namespace mike_and_conquer_monogame.commands
{
    public class UpdateBarracksPercentCompletedCommand : AsyncViewCommand
    {
        private int percentCompleted;
        public UpdateBarracksPercentCompletedCommand(int percentCompleted)
        {
            this.percentCompleted = percentCompleted;
        }

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.UpdateBarracksPercentCompleted(percentCompleted);

        }
    }
}
