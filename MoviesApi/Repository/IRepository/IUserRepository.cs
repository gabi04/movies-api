using MoviesApi.Models;
using MoviesApi.Models.Dtos;
using UserApi.Models.Dtos;

namespace MoviesApi.Repository.IRepository
{
    public interface IUserRepository
    {
        ICollection<AppUser> GetUsers();
        AppUser GetUser(string id);
        bool IsUniqueUserName(string userName);
        Task<UserLoginAnswerDto> Login(UserLoginDto userLoginDto);
        Task<UserDataDto> Register(UserRegisterDto userRegistrationDto);
        bool Save();
    }
}
