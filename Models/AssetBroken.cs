using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AssetProject.Models
{
    public class AssetBroken
    {
        [Key]
        public int AssetBrokenId { get; set; }
        [Required(ErrorMessage = "IS Required")]
        public DateTime DateBroken { get; set; }
        [Required(ErrorMessage = "IS Required"), RegularExpression(@"^[a-zA-z]+([\s][a-zA-Z]+)*$", ErrorMessage = " Not Valid"), MinLength(5, ErrorMessage = "Minimum Length Is 5")]
        public string Notes { get; set; }
        public ICollection<AssetBrokenDetails> AssetBrokenDetails{ get; set; }
    }
}
