namespace AssetProject.Models
{
    public class AssetSellDetails
    {
        public int AssetSellDetailsId { set; get; }
        public SellAsset SellAsset { set; get; }
        public int SellAssetId { set; get; }
        public Asset Asset { set; get; }
        public int AssetId { set; get; }
        public string Remarks { set; get; }
    }
}
