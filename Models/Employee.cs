using System.ComponentModel.DataAnnotations;

namespace AssetProject.Models
{
    public class Employee
    {
        public int ID { get; set; }
        [Required,RegularExpression("^[0-9]+$", ErrorMessage = " Accept Number Only")]
        public string EmployeeId { get; set; }
       [Required]
        public string FullName { get; set; }
        [Required]

        public string Title { get; set; }
        [Required, RegularExpression("^[0-9]+$", ErrorMessage = " Accept Number Only")]
        public string Phone { get; set; }
        [Required, RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",ErrorMessage ="Not Valid")]
        public string Email { get; set; }
       
        public string Notes { get; set; }
        
        public string Remark { get; set; }

        public int? TenantId { get; set; }

        public virtual Tenant tenant { get; set; }
    }
}
