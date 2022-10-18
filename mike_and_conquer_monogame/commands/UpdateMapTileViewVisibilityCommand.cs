
using mike_and_conquer_monogame.main;


namespace mike_and_conquer_monogame.commands
{
    public class UpdateMapTileViewVisibilityCommand : AsyncViewCommand
    {

        private readonly int mapTileInstanceId;
        private readonly string visibility;

        public UpdateMapTileViewVisibilityCommand(int mapTileInstanceId, string visibility)
        {
            this.mapTileInstanceId = mapTileInstanceId;
            this.visibility = visibility;
        }

        protected override void ProcessImpl()
        {
            MikeAndConquerGame.instance.UpdateMapTileViewVisibility(mapTileInstanceId, visibility);
        }
    }
}
