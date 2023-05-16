using System.ComponentModel.DataAnnotations;

namespace BezpieczenstwoProjekt.Models.Dto
{
    public class Login
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
