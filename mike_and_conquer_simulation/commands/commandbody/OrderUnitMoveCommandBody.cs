namespace mike_and_conquer_simulation.commands.commandbody
{
    internal class OrderUnitMoveCommandBody
    {
        public int DestinationLocationXInWorldCoordinates { get; set; }

        public int DestinationLocationYInWorldCoordinates { get; set; }

        public int UnitId { get; set; }

        public const string CommandName = "CreateMinigunner";
    }
}
