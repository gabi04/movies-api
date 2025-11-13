using MoviesApi.Models.Dtos;

namespace UserApi.Models.Dtos
{
    public class UserLoginAnswerDto
    {
        public UserDataDto User { get; set; }

        public string Role { get; set; }

        public string Token { get; set; }
    }
}
