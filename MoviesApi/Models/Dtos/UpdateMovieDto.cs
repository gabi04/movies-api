namespace MoviesApi.Models.Dtos
{
    public class UpdataMovieDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string? ImgPath { get; set; }
        public string? ImgLocalPath { get; set; }
        public IFormFile Image { get; set; }
        public enum ClasificationType { GeneralAudiences, ParentalGuidance, ParentsStronglyCautioned, Restricted, NoChildrenUnderSeventeen }
        public ClasificationType Clasification { get; set; }
        public int categoryId { get; set; }        
    }
}
