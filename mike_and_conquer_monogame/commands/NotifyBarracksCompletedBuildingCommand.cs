using mike_and_conquer_monogame.main;


namespace mike_and_conquer_monogame.commands
{
    public class NotifyBarracksCompletedBuildingCommand : AsyncViewCommand
    {

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.NotifyBarracksCompletedBuilding();

        }
    }
}
