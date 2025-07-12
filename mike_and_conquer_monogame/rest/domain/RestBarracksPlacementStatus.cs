namespace mike_and_conquer_monogame.rest.domain
{
    class RestBarracksPlacementStatus
    {
        public bool IsValidPlacement { get; }

        public RestBarracksPlacementStatus(bool isValidPlacement)
        {
            this.IsValidPlacement = isValidPlacement;
        }
    }
}