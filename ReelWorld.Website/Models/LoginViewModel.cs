using ReelWorld.DataAccessLibrary.Model;
using System.ComponentModel.DataAnnotations;

namespace ReelWorld.Website.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
