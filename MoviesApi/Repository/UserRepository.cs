using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UserRepository(Context db, IConfiguration config, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _db = db;
            secretKey = config.GetValue<string>("ApiSettings:Secret");
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }


        public ICollection<AppUser> GetUsers()
        {
            return _db.Users.OrderBy(u => u.Name).ToList();
        }

        public AppUser GetUser(string id)
        {
            return _db.Users.FirstOrDefault(u => u.Id == id);
        }

        public bool IsUniqueUserName(string userName)
        {
            return !_db.AppUsers.Any(u => u.UserName== userName);
        }

        public async Task<UserLoginAnswerDto> Login(UserLoginDto userLoginDto)
        {
            var user = _db.AppUsers.FirstOrDefault(
                u => u.UserName.ToLower() == userLoginDto.UserName.ToLower()
            );

            
            Console.WriteLine($"user: {user}");

            bool isValid = await _userManager.CheckPasswordAsync(user, userLoginDto.Password);

            Console.WriteLine($"isValid: {isValid}");
            if (user == null || isValid == false)
            {
                return await Task.FromResult(new UserLoginAnswerDto()
                {
                    Token = "",
                    User = null,
                    Role = ""
                });
            }

            var roles = await _userManager.GetRolesAsync(user);
            var handleToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = handleToken.CreateToken(tokenDescriptor);

            UserLoginAnswerDto userLoginAnswerDto = new UserLoginAnswerDto()
            {
                Token = handleToken.WriteToken(token),
                User = _mapper.Map<UserDataDto>(user),
            };

            return userLoginAnswerDto;
        }

        public async Task<UserDataDto> Register(UserRegisterDto userRegisterDto)
        {
            //var passwordEncrypted = getMd5(userRegisterDto.Password);

            AppUser user = new AppUser()
            {
                UserName = userRegisterDto.UserName,
                Email = userRegisterDto.UserName,
                NormalizedEmail = userRegisterDto.UserName.ToUpper(),
                Name = userRegisterDto.Name
            };

            var result = await _userManager.CreateAsync(user, userRegisterDto.Password);
            if(result.Succeeded)
            {
                if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("Registered"));
                }

                await _userManager.AddToRoleAsync(user, "Admin");
                //var userRetornado = _db.AppUsers.FirstOrDefault(u => u.UserName == userRegisterDto.UserName);

                return _mapper.Map<UserDataDto>(user);
            }

            var errors = result.Errors.Select(e => e.Description);
            throw new Exception(string.Join(" | ", errors));
            //_db.Users.Add(user);
            //await _db.SaveChangesAsync();
            //user.Password = passwordEncrypted;
            return null;
        }

        //Método para encriptar contraseña con MD5 se usa tanto en el Acceso como en el Registro
        /*public static string getMd5(string valor)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
            data = x.ComputeHash(data);
            string resp = "";
            for (int i = 0; i < data.Length; i++)
                resp += data[i].ToString("x2").ToLower();
            return resp;
        }*/

        public bool Save()
        {
            throw new NotImplementedException();
        }
    }
}
