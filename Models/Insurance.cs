using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetProject.Models
{
    public class Insurance
    {
        [Key]
        public int InsuranceId { get; set; }
        [Required(ErrorMessage ="Is Required")]
        [ MinLength(5,ErrorMessage ="Minimum Length Is 5")]
        public string Title { get; set; }
        [ MinLength(5, ErrorMessage = "Minimum Length Is 5")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Is Required")]
        [ MinLength(5, ErrorMessage = "Minimum Length Is 5")]
        public string InsuranceCompany { get; set; }
        [Required(ErrorMessage = "Is Required")]
        [ MinLength(5, ErrorMessage = "Minimum Length Is 5")]
        public string ContactPerson { get; set; }
        [Required(ErrorMessage = "Is Required")]
        [RegularExpression("^[0-9]+$", ErrorMessage = " Accept Number Only")]
        public string PolicyNo { get; set; }
        [Required(ErrorMessage = "Is Required")]
        [RegularExpression("^[0-9]+$", ErrorMessage = " Accept Number Only")]
        public string Phone { get; set; }
        [Column(TypeName = "date"), Required(ErrorMessage = "Is Required")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "date"),Required(ErrorMessage = "Is Required")]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "Is Required")]
        [RegularExpression(@"(?!^0*$)(?!^0*\.0*$)^\d{1,5}(\.\d{1,3})?$", ErrorMessage = " Accept Number Only")]
        public decimal Deductible { get; set; }
        [Required(ErrorMessage = "Is Required")]
        [RegularExpression(@"(?!^0*$)(?!^0*\.0*$)^\d{1,5}(\.\d{1,3})?$", ErrorMessage = " Accept Number Only")]
        public decimal Permium { get; set; }
        public bool IsActive { get; set; }
        public int? TenantId { get; set; }
        public virtual Tenant tenant { get; set; }
    }
}
