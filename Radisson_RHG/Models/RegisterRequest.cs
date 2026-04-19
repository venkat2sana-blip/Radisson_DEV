using System.ComponentModel.DataAnnotations;

namespace Radisson_RHG.Models
{
    public class RegisterRequest
    {
        [Required]
        [MinLength(5)]
        [MaxLength(15)]
        public string UserName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        [MaxLength(100)]
        public string Password { get; set; } = null!;

    }
}
