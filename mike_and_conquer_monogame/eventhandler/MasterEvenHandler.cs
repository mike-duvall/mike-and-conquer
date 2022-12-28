using System;
using System.Collections.Generic;
using mike_and_conquer_monogame.commands;
using mike_and_conquer_monogame.main;
using mike_and_conquer_simulation.events;
using Newtonsoft.Json;


namespace mike_and_conquer_monogame.eventhandler
{
    public class MasterEventHandler : SimulationStateListener
    {
        private MikeAndConquerGame mikeAndConquerGame = null;
        private Dictionary<string , CommandAndEventDataTypes > eventTypeToCommandMap;
        public MasterEventHandler(MikeAndConquerGame aGame)
        {
            this.mikeAndConquerGame = aGame;
            eventTypeToCommandMap = new Dictionary<string, CommandAndEventDataTypes>();

        }

        public override void Update(SimulationStateUpdateEvent anEvent)
        {

            // if (eventTypeToCommandMap.ContainsKey(anEvent.EventType))
            // {
            //     Type command = eventTypeToCommandMap[anEvent.EventType];
            //
            //     Object[] parameters = new Object[1];
            //     parameters[0] = anEvent.EventData;
            //
            //     AsyncViewCommand commandInstance = (AsyncViewCommand)Activator.CreateInstance(command,parameters);
            //     mikeAndConquerGame.PostCommand(commandInstance);
            //
            // }

            if (eventTypeToCommandMap.ContainsKey(anEvent.EventType))
            {
                CommandAndEventDataTypes commandAndEventDataTypes = eventTypeToCommandMap[anEvent.EventType];

                // GDIBarracksPlacedEventData eventData =
                //     JsonConvert.DeserializeObject<GDIBarracksPlacedEventData>(stringEventData);

                Type eventDataType = commandAndEventDataTypes.eventDataType;

                Object eventData = JsonConvert.DeserializeObject(anEvent.EventData, eventDataType);

                Object[] parameters = new Object[1];
                parameters[0] = eventData;


                AsyncViewCommand commandInstance = (AsyncViewCommand)Activator.CreateInstance(commandAndEventDataTypes.commandType, parameters);
                mikeAndConquerGame.PostCommand(commandInstance);
            
            }

        }


        public void HandleEvent(string eventName, CommandAndEventDataTypes commandAndEventDataTypes)
        {
            eventTypeToCommandMap.Add(eventName, commandAndEventDataTypes);
            int x = 3;
        }

        
    }
}
