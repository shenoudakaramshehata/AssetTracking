using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AssetProject.Models
{
    public class DisposeAsset
    {
        public int DisposeAssetId { get; set; }
       
        [Column(TypeName = "date")]
        public DateTime DateDisposed { get; set; }
        [Required]
        public string DisposeTo { get; set; }
        public string Notes { get; set; }
        [JsonIgnore]
        public ICollection<AssetDisposeDetails> AssetDisposeDetails { get; set; }

    }
}
