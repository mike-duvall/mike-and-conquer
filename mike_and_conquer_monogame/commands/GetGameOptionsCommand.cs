
using mike_and_conquer_monogame.gameview;
using mike_and_conquer_monogame.main;

namespace mike_and_conquer_monogame.commands
{
    internal class GetGameOptionsCommand : AsyncViewCommand
    {


        public GetGameOptionsCommand()
        {

        }

        protected override void ProcessImpl()
        {
            GameOptions gameOptionsCopy = new GameOptions();
            gameOptionsCopy.DrawShroud = GameOptions.instance.DrawShroud;
            gameOptionsCopy.MapZoomLevel = GameOptions.instance.MapZoomLevel;
            // TODO: Revisit this, fill in all attributes, or OK to access via thread?

            result = gameOptionsCopy;

        }

        public GameOptions GetGameOptions()
        {
            return (GameOptions)GetResult();
        }


    }
}
