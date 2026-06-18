using System.ComponentModel.DataAnnotations;

namespace Radisson_RHG
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(15)]
        public string UserName { get; set; } = null;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null;

        [Required]
        [MaxLength(200)]
        public string PasswordHash { get; set; } = null; // store hashed password
        public DateTime CreatedOn { get; set; }

        public string Role { get; set; } = "user";

    }
}
