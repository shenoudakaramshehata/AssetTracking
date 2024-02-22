namespace AssetProject.Models
{
    public class AssetLeasingDetails
    {
        public int AssetLeasingDetailsId { set; get; }
        public AssetLeasing AssetLeasing { set; get; }
        public int AssetLeasingId { set; get; }
        public Asset Asset { set; get; }
        public int AssetId { set; get; }
        public string Remarks { set; get; }
    }
}
