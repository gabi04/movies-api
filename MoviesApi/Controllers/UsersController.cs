using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Models.Dtos;
using MoviesApi.Repository;
using MoviesApi.Repository.IRepository;
using System.Net;
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
        public IActionResult GetUser(int id)
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
            bool validateUniqueUsername = _userRepository.IsUniqueUserName(userRegisterDto.UserName);
            if (!validateUniqueUsername)
            {
                return BadRequest(ModelState);
            }

            var usuario = await _userRepository.Register(userRegisterDto);
            if (usuario == null)
            {
                return BadRequest(ModelState);
            }

            return Ok(HttpStatusCode.OK);

        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {

            var answerLogin = await _userRepository.Login(userLoginDto);

            if (answerLogin.User == null || string.IsNullOrEmpty(answerLogin.Token))
            {
                return BadRequest(ModelState);
            }

            return Ok(answerLogin);
        }
    }
}
