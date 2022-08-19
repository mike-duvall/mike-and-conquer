using System.Collections.Generic;
using System.Drawing;
using mike_and_conquer_simulation.events;
using mike_and_conquer_simulation.main;
using Newtonsoft.Json;

// using Microsoft.Xna.Framework;
// using mike_and_conquer.gameobjects;

namespace mike_and_conquer_simulation.gameworld
{
    class GDIPlayer
    {


        private PlayerController playerController;

        // public List<Minigunner> gdiMinigunnerList;
        // public List<Minigunner> GdiMinigunnerList
        // {
        //     get { return gdiMinigunnerList; }
        // }


        private List<Unit> unitList;

        public List<Unit> UnitList
        {
            get { return unitList; }
        }


        // private GDIBarracks gdiBarracks;
        // public GDIBarracks GDIBarracks
        // {
        //     get { return gdiBarracks; }
        // }
        //
        // private GDIConstructionYard gdiConstructionYard;
        // public GDIConstructionYard GDIConstructionYard
        // {
        //     get { return gdiConstructionYard; }
        // }



        // private MCV mcv;
        // public MCV MCV
        // {
        //     get { return mcv; }
        //     set { mcv = value; }
        // }

        public GDIPlayer(PlayerController playerController)
        {
            // gdiMinigunnerList = new List<Minigunner>();
            unitList = new List<Unit>();

            this.playerController = playerController;
        }

        public void HandleReset()
        {
            // gdiMinigunnerList.Clear();
            unitList.Clear();
            // mcv = null;
            // gdiConstructionYard = null;
            // gdiBarracks = null;

        }

        // public Minigunner GetMinigunner(int id)
        // {
        //     Minigunner foundMinigunner = null;
        //     foreach (Minigunner nextMinigunner in gdiMinigunnerList)
        //     {
        //         if (nextMinigunner.UnitId == id)
        //         {
        //             foundMinigunner = nextMinigunner;
        //         }
        //     }
        //
        //     return foundMinigunner;
        //
        // }

        // public void DeslectAllUnits()
        // {
        //     foreach (Minigunner nextMinigunner in gdiMinigunnerList)
        //     {
        //         nextMinigunner.selected = false;
        //     }
        //
        //     if (mcv != null)
        //     {
        //         mcv.selected = false;
        //     }
        //
        // }

        // public void Update(GameTime gameTime)
        // {
        //     playerController.Update(gameTime);
        //     UpdateGDIMinigunners(gameTime);
        //     UpdateBarracks(gameTime);
        //     UpdateConstructionYard(gameTime);
        //
        //
        //
        //     if (mcv != null)
        //     {
        //         mcv.Update(gameTime);
        //     }
        //
        // }

        public void Update()
        {
            if (playerController != null)
            {
                playerController.Update();
            }

            // UpdateGDIMinigunners();
            UpdateUnits();

        }


        // private void UpdateGDIMinigunners(GameTime gameTime)
        // {
        //     foreach (Minigunner nextMinigunner in gdiMinigunnerList)
        //     {
        //         if (nextMinigunner.Health > 0)
        //         {
        //             nextMinigunner.Update(gameTime);
        //         }
        //     }
        // }

        // private void UpdateGDIMinigunners()
        // {
        //     foreach (Minigunner nextMinigunner in gdiMinigunnerList)
        //     {
        //         // if (nextMinigunner.Health > 0)
        //         // {
        //             nextMinigunner.Update();
        //         // }
        //     }
        // }

        private void UpdateUnits()
        {
            foreach (Unit nextUnit in unitList)
            {
                nextUnit.Update();
            }
        }



        // private void UpdateBarracks(GameTime gameTime)
        // {
        //     if (gdiBarracks != null)
        //     {
        //         gdiBarracks.Update(gameTime);
        //     }
        // }
        //
        //
        // private void UpdateConstructionYard(GameTime gameTime)
        // {
        //     if (gdiConstructionYard != null)
        //     {
        //         gdiConstructionYard.Update(gameTime);
        //     }
        // }


        // public void AddMinigunner(Minigunner newMinigunner)
        // {
        //     gdiMinigunnerList.Add(newMinigunner);
        //
        // }
        //
        // public MCV AddMCV(Point positionInWorldCoordinates)
        // {
        //     mcv = new MCV(positionInWorldCoordinates.X, positionInWorldCoordinates.Y);
        //     return mcv;
        // }

        // public GDIBarracks AddGDIBarracks(MapTileLocation mapTileLocation)
        // {
        //     // TODO Might want to check if one already exists and throw error if so
        //     gdiBarracks = new GDIBarracks(mapTileLocation);
        //     return gdiBarracks;
        // }
        //
        // public GDIConstructionYard AddGDIConstructionYard(MapTileLocation mapTileLocation)
        // {
        //     // TODO Might want to check if one already exists and throw error if so
        //     gdiConstructionYard = new GDIConstructionYard(mapTileLocation);
        //     return gdiConstructionYard;
        // }

        // public bool IsPointOverMCV(Point pointInWorldCoordinates)
        // {
        //
        //     if (this.mcv != null)
        //     {
        //         if (mcv.ContainsPoint(pointInWorldCoordinates.X, pointInWorldCoordinates.Y))
        //         {
        //             return true;
        //         }
        //     }
        //
        //     return false;
        // }

        // public bool IsAMinigunnerSelected()
        // {
        //     foreach (Minigunner nextMinigunner in gdiMinigunnerList)
        //     {
        //         if (nextMinigunner.selected)
        //         {
        //             return true;
        //         }
        //     }
        //     return false;
        // }
        //
        // public bool IsAnMCVSelected()
        // {
        //     if (mcv != null)
        //     {
        //         return mcv.selected;
        //     }
        //
        //     return false;
        // }


        // public void RemoveMCV()
        // {
        //     mcv = null;
        // }


        public Minigunner CreateMinigunner(int xInWorldCoordinates, int yInWorldCoordinates)
        {
            Minigunner minigunner = new Minigunner();
            minigunner.GameWorldLocation.X = xInWorldCoordinates;
            minigunner.GameWorldLocation.Y = yInWorldCoordinates;

            unitList.Add(minigunner);

            PublishUnitCreateEvent(MinigunnerCreateEventData.EventType, minigunner.UnitId, xInWorldCoordinates, yInWorldCoordinates);

            return minigunner;
        }

        public Jeep CreateJeep(int xInWorldCoordinates, int yInWorldCoordinates)
        {
            Jeep jeep = new Jeep();
            jeep.GameWorldLocation.X = xInWorldCoordinates;
            jeep.GameWorldLocation.Y = yInWorldCoordinates;
            unitList.Add(jeep);

            PublishUnitCreateEvent(JeepCreateEventData.EventType, jeep.UnitId, xInWorldCoordinates, yInWorldCoordinates);
            return jeep;
        }

        public MCV CreateMCV(int xInWorldCoordinates, int yInWorldCoordinates)
        {
            MCV mcv = new MCV();
            mcv.GameWorldLocation.X = xInWorldCoordinates;
            mcv.GameWorldLocation.Y = yInWorldCoordinates;
            unitList.Add(mcv);

            PublishUnitCreateEvent(MCVCreateEventData.EventType, mcv.UnitId, xInWorldCoordinates, yInWorldCoordinates);
            return mcv;

        }


        private void PublishUnitCreateEvent(string eventType, int unitId, int xInWorldCoordinates, int yInWorldCoordinates)
        {
            UnitCreateEventData eventData = new UnitCreateEventData(
                unitId,
                xInWorldCoordinates,
                yInWorldCoordinates);

            string serializedEventData = JsonConvert.SerializeObject(eventData);
            SimulationStateUpdateEvent simulationStateUpdateEvent =
                new SimulationStateUpdateEvent(
                    eventType,
                    serializedEventData);


            SimulationMain.instance.PublishEvent(simulationStateUpdateEvent);

        }

        public Unit FindUnitWithUnitId(int unitId)
        {
            Unit foundUnit = null;

            foreach (Unit unit in unitList)
            {
                if (unit.UnitId == unitId)
                {
                    foundUnit = unit;
                }
            }

            return foundUnit;

        }
    }
}
