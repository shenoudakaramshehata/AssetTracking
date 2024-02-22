namespace AssetProject.Models
{
    public class AssetRepairDetails
    {
        public int AssetRepairDetailsId { set; get; }
        public AssetRepair AssetRepair { set; get; }
        public int AssetRepairId { set; get; }
        public Asset Asset { set; get; }
        public int AssetId { set; get; }
        public string Remarks { set; get; }

    }
}
