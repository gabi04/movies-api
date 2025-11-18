using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Repository.IRepository;
using UserApi.Models.Dtos;

namespace MoviesApi.Controllers
{
    
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        //protected RespuestaAPI _respuestaApi;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            //this._respuestaApi = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetUsers()
        {
            var usersList = _userRepository.GetUsers();

            var usersListDto = new List<UserDto>();

            foreach (var list in usersList)
            {
                usersListDto.Add(_mapper.Map<UserDto>(list));
            }
            return Ok(usersListDto);
        }

        [HttpGet("{id:int}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUser(string id)
        {
            var itemUser = _userRepository.GetUser(id);

            if (itemUser == null)
            {
                return NotFound();
            }

            var itemUserDto = _mapper.Map<UserDto>(itemUser);

            return Ok(itemUserDto);
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool validateUniqueUsername = _userRepository.IsUniqueUserName(userRegisterDto.UserName);

            if (!validateUniqueUsername)
                return BadRequest("El nombre de usuario ya existe");

            try
            {
                var usuario = await _userRepository.Register(userRegisterDto);

                if (usuario == null)
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        "Ocurrió un error al crear el usuario");

                return Created("api/users/" + usuario.Id, usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }

        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {

            var answerLogin = await _userRepository.Login(userLoginDto);
            Console.WriteLine($"answerLogin: {answerLogin}");

            if (answerLogin.User == null || string.IsNullOrEmpty(answerLogin.Token))
            {
                return BadRequest(ModelState);
            }

            return Ok(answerLogin);
        }
    }
}
