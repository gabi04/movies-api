using Microsoft.IdentityModel.Tokens;
using MoviesApi.Data;
using MoviesApi.Models;
using MoviesApi.Models.Dtos;
using MoviesApi.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserApi.Models;
using UserApi.Models.Dtos;


namespace MoviesApi.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly Context _db;
        private string secretKey;

        public UserRepository(Context db, IConfiguration config)
        {
            _db = db;
            secretKey = config.GetValue<string>("ApiSettings:Secret");
        }


        public ICollection<User> GetUsers()
        {
            return _db.Users.OrderBy(u => u.Name).ToList();
        }

        public User GetUser(int id)
        {
            return _db.Users.FirstOrDefault(u => u.Id == id);
        }

        public bool IsUniqueUserName(string userName)
        {
            return !_db.Users.Any(u => u.UserName== userName);
        }

        public async Task<UserLoginAnswerDto> Login(UserLoginDto userLoginDto)
        {
            var passwordEncrypted = getMd5(userLoginDto.Password);

            var user = _db.Users.FirstOrDefault(
                u => u.UserName.ToLower() == userLoginDto.UserName.ToLower()
                && u.Password == passwordEncrypted
                );

            if (user == null)
            {
                return await Task.FromResult(new UserLoginAnswerDto()
                {
                    Token = "",
                    User = null,
                    Role = ""
                });
            }

            var handleToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = handleToken.CreateToken(tokenDescriptor);

            UserLoginAnswerDto userLoginAnswerDto = new UserLoginAnswerDto()
            {
                Token = handleToken.WriteToken(token),
                User = new UserDataDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Name = user.Name
                },
                Role = user.Role
            };

            return userLoginAnswerDto;
        }

        public async Task<User> Register(UserRegisterDto userRegisterDto)
        {
            var passwordEncrypted = getMd5(userRegisterDto.Password);

            User user = new User()
            {
                UserName = userRegisterDto.UserName,
                Password = passwordEncrypted,
                Name = userRegisterDto.UserName,
                Role = userRegisterDto.Role
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            user.Password = passwordEncrypted;
            return user;
        }

        //Método para encriptar contraseña con MD5 se usa tanto en el Acceso como en el Registro
        public static string getMd5(string valor)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
            data = x.ComputeHash(data);
            string resp = "";
            for (int i = 0; i < data.Length; i++)
                resp += data[i].ToString("x2").ToLower();
            return resp;
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }
    }
}
