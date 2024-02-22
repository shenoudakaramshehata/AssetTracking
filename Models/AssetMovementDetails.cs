namespace AssetProject.Models
{
    public class AssetMovementDetails
    {
        public int AssetMovementDetailsId { set; get; }
        public AssetMovement AssetMovement { set; get; }
        public int AssetMovementId { set; get; }
        public Asset Asset { set; get; }
        public int AssetId { set; get; }
        public string Remarks { set; get; }

    }
}
