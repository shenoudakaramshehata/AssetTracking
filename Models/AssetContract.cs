using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetProject.Models
{
    public class AssetContract
    {
        public int AssetContractID { get; set; }
        public int AssetId { get; set; }
        public Asset Asset { get; set; }
        public int? ContractId { get; set; }
        public Contract Contract { get; set; }
    }
}
