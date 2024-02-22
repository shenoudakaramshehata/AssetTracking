namespace AssetProject.Models
{
    public class AssetBrokenDetails
    {
        public int AssetBrokenDetailsId { set; get; }
        public AssetBroken AssetBroken { set; get; }
        public int AssetBrokenId { set; get; }
        public Asset Asset { set; get; }
        public int AssetId { set; get; }
        public string Remarks { set; get; }
       
    }
}
