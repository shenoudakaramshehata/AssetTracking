using System.ComponentModel.DataAnnotations;

namespace AssetProject.ViewModel
{
    public class AssetSerachVM
    {
        [Required(ErrorMessage ="Is Required")]
        public string AssetSearchItem { get; set; }
      
    }
}
