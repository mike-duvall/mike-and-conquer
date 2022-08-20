using mike_and_conquer_monogame.gamesprite;
//using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


using XnaPoint = Microsoft.Xna.Framework.Point;
 using XnaVector2 = Microsoft.Xna.Framework.Vector2;


namespace mike_and_conquer_monogame.gameview
{
    public class TerrainView
    {

        private TerrainSprite terrainSprite;

        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();

        private int xInWorldMapTileCoordinates;
        private int yInWorldMapTileCoordinates;


        // private TerrainItem terrainItem;
        //
        // public TerrainView(TerrainItem terrainItem)
        // {
        //     this.terrainItem = terrainItem;
        //     Point xnaPoint = new Point(terrainItem.MapTileLocation.WorldMapTileCoordinatesAsPoint.X,
        //         terrainItem.MapTileLocation.WorldMapTileCoordinatesAsPoint.Y);
        //     this.terrainSprite = new TerrainSprite(terrainItem.TerrainItemType, xnaPoint);
        // }

        public TerrainView(int xInWorldMapTileCoordinates, int yInWorldMapTileCoordinates,string terrainItemType)
        {
            this.xInWorldMapTileCoordinates = xInWorldMapTileCoordinates;
            this.yInWorldMapTileCoordinates = yInWorldMapTileCoordinates;
            XnaPoint xnaPoint = new XnaPoint(xInWorldMapTileCoordinates, yInWorldMapTileCoordinates);
            this.terrainSprite = new TerrainSprite(terrainItemType, xnaPoint);
        }


        // public void DrawFull(GameTime gameTime, SpriteBatch spriteBatch)
        // {
        //     DrawNoShadow(gameTime, spriteBatch);
        //     DrawShadowOnly(gameTime,spriteBatch);
        // }
        
        public void DrawShadowOnly(GameTime gameTime, SpriteBatch spriteBatch)
        {


            // this.terrainSprite.DrawShadowOnly(
            //     gameTime,
            //     spriteBatch,
            //     MonogameUtil.ConvertSystemNumericsVector2ToXnaVector2(terrainItem.MapTileLocation.WorldCoordinatesAsVector2)
            //     );

            XnaPoint locationAsXnaPointInMapTileCoordinates = new XnaPoint(xInWorldMapTileCoordinates,
                yInWorldMapTileCoordinates);

            XnaPoint locationAsXnaPointInWorldCoordinates = GameWorldView.ConvertMapTileCoordinatesToWorldCoordinates(locationAsXnaPointInMapTileCoordinates);

            XnaVector2 locationAsXnaVectorInWorldCoordiantes = new XnaVector2(
                locationAsXnaPointInWorldCoordinates.X,
                locationAsXnaPointInWorldCoordinates.Y);


            this.terrainSprite.DrawShadowOnly(
                gameTime,
                spriteBatch,
                locationAsXnaVectorInWorldCoordiantes);


        }



        public void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // this.terrainSprite.DrawNoShadow(
            //     gameTime,
            //     spriteBatch, 
            //     MonogameUtil.ConvertSystemNumericsVector2ToXnaVector2(terrainItem.MapTileLocation.WorldCoordinatesAsVector2),
            //     terrainItem.LayerDepthOffset);


            XnaPoint locationAsXnaPointInMapTileCoordinates = new XnaPoint(xInWorldMapTileCoordinates,
                yInWorldMapTileCoordinates);

            XnaPoint locationAsXnaPointInWorldCoordinates = GameWorldView.ConvertMapTileCoordinatesToWorldCoordinates(locationAsXnaPointInMapTileCoordinates);

            XnaVector2 locationAsXnaVectorInWorldCoordiantes = new XnaVector2(
                locationAsXnaPointInWorldCoordinates.X,
                locationAsXnaPointInWorldCoordinates.Y);

            // this.terrainSprite.DrawNoShadow(
            //     gameTime,
            //     spriteBatch,
            //     MonogameUtil.ConvertSystemNumericsVector2ToXnaVector2(terrainItem.MapTileLocation.WorldCoordinatesAsVector2),
            //     terrainItem.LayerDepthOffset);

            this.terrainSprite.DrawNoShadow(
                gameTime,
                spriteBatch,
                locationAsXnaVectorInWorldCoordiantes,
                0);



        }
    }
}
