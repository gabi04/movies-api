using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesApi.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string ImgPath { get; set; }
        public enum ClassificationType { GeneralAudiences, ParentalGuidance, ParentsStronglyCautioned, Restricted, NoChildrenUnderSeventeen  }
        public ClassificationType Classification {  get; set; }
        public DateTime CreationDate { get; set; }

        public int categoryId { get; set; }
        [ForeignKey("categoryId")]
        public Category Category { get; set; }
    }
}
