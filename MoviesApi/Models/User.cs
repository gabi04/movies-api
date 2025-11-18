using System.ComponentModel.DataAnnotations;

namespace UserApi.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        /*public int Years { get; set; }

        public enum DocumentType { CC, CE, NIT, TI, PASSPORT, DNI, RUT, PEP, PTP, OTHER}
        public DocumentType Document { get; set; }
        public int DocumentNumber { get; set; }

        public int movieId { get; set; }
        [ForeignKey("movieId")]
        public Movie Movie { get; set; }*/

    }
}
