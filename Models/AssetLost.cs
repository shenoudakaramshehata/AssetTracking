using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AssetProject.Models
{
    public class AssetLost
    {
        public int AssetLostId { get; set; }
      
        [Column(TypeName = "date")]
        public DateTime DateLost { get; set; }
        public string Notes { get; set; }

        public ICollection<AssetLostDetails> AssetLostDetails { get; set; }
    }
}
