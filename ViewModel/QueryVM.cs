using AssetProject.Models;

namespace AssetProject.ViewModel
{
    public class QueryVM
    {
        public string AssetTagId { get; set; }
        public string AssetSN { get; set; }
        public double AssetCost { get; set; }
        public string Item { get; set; }
        public string TransactionD { get; set; }
        public string EmployeeName { get; set; }
        public string DepatrmentName { get; set; }
        public string LocationName { get; set; }
        public string StoreName { get; set; }
        public int DepId { get; set; }
        public AssetMovement AssetMovement { get; set; }
    }
}
