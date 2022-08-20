
using System.Collections.Generic;
using mike_and_conquer_simulation.events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace mike_and_conquer_monogame.commands.mapper
{
    internal class HandleUnitMovementPlanCreatedCommandMapper
    {
        public static CreatePlannedPathViewCommand ConvertToCommand(string anEventEventData)
        {
            UnitMovementPlanCreatedEventData eventData =
                JsonConvert.DeserializeObject<UnitMovementPlanCreatedEventData>(anEventEventData);



            List<PathStep> pathStepList = new List<PathStep>();

            var jsonObject = JObject.Parse(anEventEventData);
            var pathSteps = jsonObject["PathSteps"];

            foreach (JObject jObject in pathSteps)
            {
                int x = jObject.GetValue("X").ToObject<int>();
                int y = jObject.GetValue("Y").ToObject<int>();
                PathStep pathStep = new PathStep(x, y);
                pathStepList.Add(pathStep);
            }

            eventData.PathSteps = pathStepList;

            CreatePlannedPathViewCommand command = new CreatePlannedPathViewCommand(eventData.UnitId, eventData.PathSteps);
            return command;

        }
    }
}
