namespace AssetProject.Models
{
    public class PurchaseAsset
    {
        public int PurchaseAssetId { get; set; }
        public int PurchaseId { get; set; }
        public virtual Purchase Purchase { get; set; }
        public int ItemId { get; set; }
        public virtual Item Item { get; set; }
        public double? Quantity { get; set; }
        public double? Price { get; set; }
        public double? Total { get; set; }
        public double? Discount { get; set; }
        public double? Net { get; set; }
        public string Remarks { get; set; }
    }
}
