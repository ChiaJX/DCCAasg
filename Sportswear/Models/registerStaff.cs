using System.ComponentModel.DataAnnotations;

namespace Sportswear.Models
{
    public class registerStaff
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }


        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }



    }
}
