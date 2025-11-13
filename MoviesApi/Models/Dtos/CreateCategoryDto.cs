using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Models.Dtos
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "El número máximo de caracteres es de 100!")]
        public string Name { get; set; }
    }
}
