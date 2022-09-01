

using System;


namespace mike_and_conquer_monogame.main

{
    public class GameOptions
    {

        public bool DrawTerrainBorder = false;
        //public bool DrawTerrainBorder = true;

        public bool DrawBlockingTerrainBorder = false;

        public bool IsFullScreen = true;
        // public bool IsFullScreen = false;

        // public bool DrawShroud = true;
        public bool DrawShroud = false;

        // public float MapZoomLevel = 1.0f;
        public float MapZoomLevel = 2.0f;

        //        public  bool PlayMusic = true;
        public bool PlayMusic = false;

        public bool DrawPaths = false;
        // public bool DrawPaths = true;

        // public enum GameSpeed
        // {
        //     Slowest = 250,
        //     Slower = 125,
        //     Slow = 85, 
        //     Moderate = 63,
        //     Normal = 40,
        //     Fast = 30,
        //     Faster = 25,
        //     Fastest = 23
        // }


        // public static float ConvertGameSpeedToDelayDivisor(GameSpeed gameSpeed)
        // {
        //     switch (gameSpeed)
        //     {
        //        // case GameSpeed.Slowest:
        //        //  return 250.0f;  4534
        //        // case GameSpeed.Slowest:
        //        //     return 251.0f;  //  4550, reload speed, // 74964 for movement test
        //         // case GameSpeed.Slowest:
        //         //    return 249.0f;  // 4514, reload speed
        //        // case GameSpeed.Slowest:
        //        //     return 248.0f;  // 4482
        //
        //        case GameSpeed.Slowest:
        //            return 248.5f;   // 4500, for reload, correct value, // 74231 for movement test, desired value 75600
        //
        //         case GameSpeed.Slower:
        //            return 125;
        //
        //
        //        // case GameSpeed.Slow:
        //         //     return 75; // reloadTime(in code) = 1.35   //  measured reloadTime(in test): 1383
        //
        //         // case GameSpeed.Slow:
        //         //     return 80;  // reloadTime(in code) = 1.44   //  measured reloadTime(in test): 1467
        //
        //        // case GameSpeed.Slow:
        //        //     return 82; // reloadTime(in code) = 1.476    //  measured reloadTime(in test): 1500 // measured movement time in test:  25035, desired value 25201
        //
        //
        //         // case GameSpeed.Slow:
        //         //     return 82.1f;   // measured movement time in test:  25083, desired value 25201
        //
        //
        //        case GameSpeed.Slow:
        //            return 82.5f;   // measured movement time in test:  , desired value 25201
        //
        //
        //         // case GameSpeed.Slow:
        //         //    return 82.5f; // reloadTime(in code) = 1.485    //  measured reloadTime(in test): 1516
        //
        //
        //         // case GameSpeed.Slow:
        //         //    return 85; // reloadTime(in code) = 1.53    //  measured reloadTime(in test): 1550
        //
        //
        //
        //         case GameSpeed.Moderate:
        //            return 63.0f;
        //         case GameSpeed.Normal:
        //             return 40.0f;  
        //
        //         case GameSpeed.Fast:
        //            return 30.0f;
        //        case GameSpeed.Faster:
        //            return 25.0f;
        //         // case GameSpeed.Fastest:
        //         //     return 23.6f; // 450
        //         // case GameSpeed.Fastest:
        //         //     return 23.0f; //434
        //         // case GameSpeed.Fastest:
        //         //     return 22.5f; // 433
        //         // case GameSpeed.Fastest:
        //         //     return 22.0f; // 415
        //         // case GameSpeed.Fastest:
        //         //     return 22.2f; // reloadTime(in code)=0.39960001373291015   //  measured reloadTime(in test): 416
        //
        //
        //
        //         // case GameSpeed.Fastest:
        //         //     return 22.22f; // reloadTime(in code)= 0.399959987640381  //  measured reloadTime(in test): 416 (but large variation, 432 to 415)
        //
        //        // case GameSpeed.Fastest:
        //        //     return 22.225f; // reloadTime(in code)= 0.400050006866455  //  measured reloadTime(in test):  433
        //
        //
        //
        //         // case GameSpeed.Fastest:
        //         //     return 22.23f; // reloadTime(in code)= 0.400139991760254  //  measured reloadTime(in test): 433
        //
        //
        //
        //         // case GameSpeed.Fastest:
        //         //    return 22.25f; //  reloadTime(in code)=0.4005 // measured reloadTime(in test): 433
        //
        //
        //         case GameSpeed.Fastest:
        //             return 22.3f;  // reloadTime(in code)=0.40139998626709 // // measured reloadTime(in test): 433
        //
        //
        //         default:
        //            throw new Exception("Unknown GameSpeed type");
        //     }
        // }
        //
        //
        // // public GameSpeed CurrentGameSpeed = GameSpeed.Moderate;
        // public GameSpeed CurrentGameSpeed = GameSpeed.Slowest;
        //
        // public float CurrentGameSpeedDivisor()
        // {
        //     return ConvertGameSpeedToDelayDivisor(this.CurrentGameSpeed);
        // }

        // public float GameSpeedDelayDivisor = GameOptions.ConvertGameSpeedToDelayDivisor(GameSpeed.Moderate);

        public static GameOptions instance;

        public GameOptions()
        {
            GameOptions.instance = this;
        }


        public  void ToggleDrawTerrainBorder()
        {
            DrawTerrainBorder = !DrawTerrainBorder;
        }

        public  void ToggleDrawBlockingTerrainBorder()
        {
            DrawBlockingTerrainBorder = !DrawBlockingTerrainBorder;
        }

        public  void ToggleDrawShroud()
        {
            DrawShroud = !DrawShroud;
        }



    }
}

