using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AssetProject.Models
{
    public class AssetDocument
    {
        public int AssetDocumentId { get; set; }
        public int AssetId { get; set; }
        public Asset Asset { set; get; }
        [Required]
        public string DocumentName { get; set; }
        public string DocumentType { get; set; }
        public string Description { get; set; }

    }
}
