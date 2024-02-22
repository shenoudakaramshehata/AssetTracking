using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AssetProject.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId  { get; set; }
        [Required(ErrorMessage = "Is Required"), MinLength(5, ErrorMessage = "Minimum Length Is 5")]
        public string FullName  { get; set; }
        [Required(ErrorMessage = "Is Required")]
        public string CompanyName  { get; set; }
        [Required(ErrorMessage = "Is Required"), MinLength(5, ErrorMessage = "Minimum Length Is 5")]
        public string Address1  { get; set; }
        [ MinLength(5, ErrorMessage = "Minimum Length Is 5")]
        public string Address2  { get; set; }
        [ MinLength(5, ErrorMessage = "Minimum Length Is 5")]
        public string City  { get; set; }
        [Required(ErrorMessage = "Is Required"), MinLength(5, ErrorMessage = "Minimum Length Is 5")]
        public string State  { get; set; }
        [Required(ErrorMessage = "Is Required"), RegularExpression("^[0-9]+$", ErrorMessage = " Accept Number Only")]
        public string PostalCode  { get; set; }
        [ MinLength(5, ErrorMessage = "Minimum Length Is 5")]
        public string Country  { get; set; }
        [Required(ErrorMessage = "Is Required"), RegularExpression("^[0-9]+$", ErrorMessage = " Accept Number Only")]
        public string Phone  { get; set; }
        [Required(ErrorMessage = "Is Required"), RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Not Valid")]
        public string Email  { get; set; }
        [MinLength(5, ErrorMessage = "Minimum Length Is 5")]
        public string Notes  { get; set; }
        public virtual ICollection<AssetLeasing> AssetLeasings { get; set; }
        public int? TenantId { get; set; }
        public virtual Tenant tenant { get; set; }
    }
}
