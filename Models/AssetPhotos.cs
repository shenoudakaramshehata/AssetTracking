using System.ComponentModel.DataAnnotations;

namespace AssetProject.Models
{
    public class AssetPhotos
    {
       [Key]
        public int AssetPhotoId { set; get; }
        [Required]
        public string PhotoUrl { set; get; }
        public string Remarks { set; get; }
        public int AssetId { set; get; }
        public Asset Asset{ set; get; }
    }
}
