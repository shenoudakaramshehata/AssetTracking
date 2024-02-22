using System.Text.Json.Serialization;

namespace AssetProject.Models
{
    public class AssetDisposeDetails
    {
        public int AssetDisposeDetailsId { set; get; }
        [JsonIgnore]
        public DisposeAsset DisposeAsset { set; get; }
        public int DisposeAssetId { set; get; }
        [JsonIgnore]
        public Asset Asset { set; get; }
        public int AssetId { set; get; }
        public string Remarks { set; get; }
    }
}
