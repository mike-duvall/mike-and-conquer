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
        private Dictionary<string , Type > eventTypeToCommandMap;
        public MasterEventHandler(MikeAndConquerGame aGame)
        {
            this.mikeAndConquerGame = aGame;
            eventTypeToCommandMap = new Dictionary<string , Type >();

        }

        public override void Update(SimulationStateUpdateEvent anEvent)
        {

            if (eventTypeToCommandMap.ContainsKey(anEvent.EventType))
            {
                Type command = eventTypeToCommandMap[anEvent.EventType];

                Object[] parameters = new Object[1];
                parameters[0] = anEvent.EventData;

                AsyncViewCommand commandInstance = (AsyncViewCommand)Activator.CreateInstance(command,parameters);
                mikeAndConquerGame.PostCommand(commandInstance);

            }
        }


        public void HandleEvent(string eventType, Type command)
        {
            eventTypeToCommandMap.Add(eventType, command);
            int x = 3;
        }

        
    }
}
