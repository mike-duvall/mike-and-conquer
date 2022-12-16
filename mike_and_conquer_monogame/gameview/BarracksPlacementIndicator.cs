


using System.Collections.Generic;
using mike_and_conquer_monogame.util;
using MapTileLocation = mike_and_conquer_simulation.gameworld.MapTileLocation;
using Point = Microsoft.Xna.Framework.Point;

namespace mike_and_conquer_monogame.gameview
{
    public class BarracksPlacementIndicator
    {

        private MapTileLocation mapTileLocation;

        public MapTileLocation MapTileLocation
        {
            get {  return mapTileLocation; }
        }

        private List<BuildingPlacementIndicatorTile> buildingBuildingPlacementIndicatorTiles;

        public List<BuildingPlacementIndicatorTile> BuildingBuildingPlacementIndicatorTiles
        {
            get { return buildingBuildingPlacementIndicatorTiles; }
        }

        public BarracksPlacementIndicator(MapTileLocation mapTileLocation)
        {
            this.mapTileLocation = mapTileLocation;
            this.buildingBuildingPlacementIndicatorTiles = new List<BuildingPlacementIndicatorTile>();
            AddTileAtRelativeLocation(0, 0);
            AddTileAtRelativeLocation(1, 0);

            AddTileAtRelativeLocation(0, 1);
            AddTileAtRelativeLocation(1, 1);

            AddTileAtRelativeLocation(0, 2);
            AddTileAtRelativeLocation(1, 2);

        }

        private void AddTileAtRelativeLocation(int x, int y)
        {
            BuildingPlacementIndicatorTile newTile = new BuildingPlacementIndicatorTile(this.MapTileLocation, x, y);
            buildingBuildingPlacementIndicatorTiles.Add(newTile);
        }



        public void UpdateLocationInWorldCoordinates(Point mouseLocationWordCoordinates)
        {
            mapTileLocation
                .XInWorldCoordinates(mouseLocationWordCoordinates.X)
                .YInWorldCoordinates(mouseLocationWordCoordinates.Y);

            foreach (BuildingPlacementIndicatorTile tile in buildingBuildingPlacementIndicatorTiles)
            {
                tile.UpdateLocation(mapTileLocation);
                tile.CanPlaceBuilding = false;
            }

            bool isAnyTileTouchingExistingBase = false;

            foreach (BuildingPlacementIndicatorTile tile in buildingBuildingPlacementIndicatorTiles)
            {
                Point xnaPoint = MonogameUtil.ConvertSystemDrawingPointToXnaPoint(tile.MapTileLocation.WorldCoordinatesAsPoint);
                if (GameWorldView.instance.IsPointAdjacentToConstructionYardAndClearForBuilding(xnaPoint))
                {
                    isAnyTileTouchingExistingBase = true;
                }
            }

            if (isAnyTileTouchingExistingBase)
            {
                foreach (BuildingPlacementIndicatorTile tile in buildingBuildingPlacementIndicatorTiles)
                {

                    if (GameWorldView.instance.IsValidMoveDestination(
                            tile.MapTileLocation.WorldCoordinatesAsPoint.X,
                            tile.MapTileLocation.WorldCoordinatesAsPoint.Y))
                    {
                        tile.CanPlaceBuilding = true;
                    }
                }
            }

        }


        public bool ValidBuildingLocation()
        {
            bool isValidBuildingLocation = true;

            foreach (BuildingPlacementIndicatorTile tile in buildingBuildingPlacementIndicatorTiles)
            {
                if (!tile.CanPlaceBuilding)
                {
                    isValidBuildingLocation = false;
                }
            }

            return isValidBuildingLocation;
        }

    }
}
