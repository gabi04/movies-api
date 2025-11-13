namespace MoviesApi.Models.Dtos
{
    public class CreateMovieDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string ImgPath { get; set; }
        public enum ClasificationType { GeneralAudiences, ParentalGuidance, ParentsStronglyCautioned, Restricted, NoChildrenUnderSeventeen }
        public ClasificationType Clasification { get; set; }
        public int categoryId { get; set; }
    }
}
