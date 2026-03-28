using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LTWeb_DinhNgocNang_2280602045.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Age { get; set; }
    }
}
