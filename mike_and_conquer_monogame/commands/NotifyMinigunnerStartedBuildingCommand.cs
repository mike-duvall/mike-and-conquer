using mike_and_conquer_monogame.main;


namespace mike_and_conquer_monogame.commands
{
    public class NotifyMinigunnerStartedBuildingCommand : AsyncViewCommand
    {

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.NotifyMinigunnerStartedBuilding();

        }
    }
}
