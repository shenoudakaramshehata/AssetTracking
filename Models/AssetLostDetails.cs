namespace AssetProject.Models
{
    public class AssetLostDetails
    {
        public int AssetLostDetailsId { set; get; }
        public AssetLost AssetLost { set; get; }
        public int AssetLostId { set; get; }
        public Asset Asset { set; get; }
        public int AssetId { set; get; }
        public string Remarks { set; get; }
    }
}
