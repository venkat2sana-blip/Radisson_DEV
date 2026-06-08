using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Radisson_RHG
{
    public class Registration
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage="Mobile number 10 digits only ")]
        public string Mobile { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Range(1,99)]
        public int Age { get; set; }
        public char Gender { get; set; }
        public DateTime CreatedOn { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var allowed = new[] { 'M', 'F', 'O' };
            if(!allowed.AsSpan().Contains(Gender))
            {
                yield return new ValidationResult("Gender must be one of 'M', 'F', or 'O'", new[] { nameof(Gender) });
            }
        }

    
    }
}
