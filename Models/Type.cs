using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AssetProject.Models
{
    public class Type
    {
        public int TypeId { set; get; }
        [Required]
        public string TypeTitle { set; get; }
        public virtual Brand Brand { get; set; }
    
        public int? BrandId { set; get; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
