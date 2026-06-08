using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Radisson_RHG.Models
{
    public class RegistrationCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage ="mobile number must be 10 degits")]
        public string Mobile { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Range(1,99)]
        public int Age { get; set; }

        [Required]
        [RegularExpression(@"^[MFO]$", ErrorMessage = "Gender must be 'm','f' or 'o'.")]
        public string Gender { get; set; } = null!;

        public DateTime? CreatedOn { get; set; }



    }
}
