using mike_and_conquer_monogame.main;


namespace mike_and_conquer_monogame.commands
{
    public class UpdateMinigunnerPercentCompletedCommand : AsyncViewCommand
    {
        private int percentCompleted;
        public UpdateMinigunnerPercentCompletedCommand(int percentCompleted)
        {
            this.percentCompleted = percentCompleted;
        }

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.UpdateMinigunnerPercentCompleted(percentCompleted);

        }
    }
}
