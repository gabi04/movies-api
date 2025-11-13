using MoviesApi.Models.Dtos;
using UserApi.Models;
using UserApi.Models.Dtos;

namespace MoviesApi.Repository.IRepository
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(int id);
        bool IsUniqueUserName(string userName);
        Task<UserLoginAnswerDto> Login(UserLoginDto userLoginDto);
        Task<User> Register(UserRegisterDto userRegistrationDto);
        bool Save();
    }
}
