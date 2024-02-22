using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AssetProject.Models
{
    public class AssetsInsurance
    {
        public int AssetsInsuranceId { get; set; }
        public int AssetId { get; set; }
        public Asset Asset { get; set; }
        public int? InsuranceId { get; set; }
        public Insurance Insurance { get; set; }

    }
}
