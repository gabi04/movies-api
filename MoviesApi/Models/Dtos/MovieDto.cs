namespace MoviesApi.Models.Dtos
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string ImgPath { get; set; }
        public enum ClassificationType { GeneralAudiences, ParentalGuidance, ParentsStronglyCautioned, Restricted, NoChildrenUnderSeventeen }
        public ClassificationType Classification { get; set; }
        public DateTime CreationDate { get; set; }

        public int categoryId { get; set; }
    }
}
