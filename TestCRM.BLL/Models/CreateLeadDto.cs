using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestCRM.BLL.Models
{
    public class CreateLeadDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Phone number must be in E.164 format.")]
        public string Phone { get; set; } = string.Empty;
    }
}
