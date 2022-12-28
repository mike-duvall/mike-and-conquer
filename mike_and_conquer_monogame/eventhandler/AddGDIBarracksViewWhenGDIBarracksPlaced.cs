using mike_and_conquer_monogame.commands;
using mike_and_conquer_monogame.main;
using mike_and_conquer_simulation.events;
using Newtonsoft.Json;


// namespace mike_and_conquer_monogame.eventhandler
// {
//     public class AddGDIBarracksViewWhenGDIBarracksPlaced : SimulationStateListener
//     {
//         private MikeAndConquerGame mikeAndConquerGame = null;
//
//
//         public AddGDIBarracksViewWhenGDIBarracksPlaced(MikeAndConquerGame aGame)
//         {
//             this.mikeAndConquerGame = aGame;
//         }
//
//         public override void Update(SimulationStateUpdateEvent anEvent)
//         {
//             if (anEvent.EventType.Equals(GDIBarracksPlacedEventData.EventType))
//             {
//                 GDIBarracksPlacedEventData eventData =
//                     JsonConvert.DeserializeObject<GDIBarracksPlacedEventData>(anEvent.EventData);
//
//                 AddGDIBarracksViewCommand command = new AddGDIBarracksViewCommand(
//                     -1,
//                     eventData.X,
//                     eventData.Y);
//
//                 mikeAndConquerGame.PostCommand(command);
//
//             }
//
//
//         }
//     }
// }
