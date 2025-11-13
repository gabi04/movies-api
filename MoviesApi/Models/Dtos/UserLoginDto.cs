using System.ComponentModel.DataAnnotations;

namespace UserApi.Models.Dtos
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Username is required")]
        [MaxLength(100, ErrorMessage = "The maximum number of characters is 100!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MaxLength(100, ErrorMessage = "The maximum number of characters is 100!")]
        public string Password { get; set; }

    }
}
