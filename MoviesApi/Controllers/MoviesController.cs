using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Models;
using MoviesApi.Models.Dtos;
using MoviesApi.Repository;
using MoviesApi.Repository.IRepository;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        //private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        public MoviesController(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetMovies()
        {
            var moviesList = _movieRepository.GetMovies();
            var moviesDtoList = new List<MovieDto>();

            foreach (var list in moviesList)
            {
                moviesDtoList.Add(_mapper.Map<MovieDto>(list));
            }

            return Ok(moviesDtoList);
        }

        [HttpGet("{id:int}", Name="GetMovie")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetMovie(int id)
        {
            var itemMovie = _movieRepository.GetMovie(id);
            if (itemMovie == null)
            {
                return NotFound();
            }
            var itemMovieDto = _mapper.Map<MovieDto>(itemMovie);
            return Ok(itemMovieDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(MovieDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateMovie([FromForm] CreateMovieDto createMovieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (createMovieDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_movieRepository.MovieExists(createMovieDto.Name))
            {
                ModelState.AddModelError("", "Movie exists");
                return StatusCode(404, ModelState);
            }

            var movie = _mapper.Map<Movie>(createMovieDto);

            /*if (!_movieRepository.CreateMovie(movie))
            {
                ModelState.AddModelError("", $"Something went wrong creating the movie {movie.Name}");
                return StatusCode(404, ModelState);
            }*/

            if (createMovieDto.Image != null)
            {
                string fileName = movie.Id + System.Guid.NewGuid().ToString() + Path.GetExtension(createMovieDto.Image.FileName);
                string path = @"wwwroot\ImagesMovies\" + fileName;

                var ubicacionDirectorio = Path.Combine(Directory.GetCurrentDirectory(), path);

                FileInfo file = new FileInfo(ubicacionDirectorio);

                if (file.Exists)
                {
                    file.Delete();
                }

                using (var fileStream = new FileStream(ubicacionDirectorio, FileMode.Create))
                {
                    createMovieDto.Image.CopyTo(fileStream);
                }

                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                movie.ImgPath = baseUrl + "/ImagesMovies/" + fileName;
                movie.ImgLocalPath = path;
            }
            else
            {
                movie.ImgPath = "https://placehold.co/600x400";
            }

            _movieRepository.CreateMovie(movie);

            return CreatedAtRoute("GetMovie", new { id = movie.Id }, movie);
        }

        [HttpPatch("{id:int}", Name ="UpdateMoviePatch")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult UpdateMoviePatch(int id, [FromBody] MovieDto movieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (movieDto == null || id != movieDto.Id)
            {
                return BadRequest(ModelState);
            }

            var existingMovie = _movieRepository.GetMovie(id);
            if (existingMovie == null)
            {
                return NotFound($"Movie with ID {id} not found");
            }

            var movie = _mapper.Map<Movie>(movieDto);

            if (!_movieRepository.UpdateMovie(movie))
            {
                ModelState.AddModelError("", $"Something went wrong updating the movie {movie.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateMoviePut")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateMoviePut(int id, [FromBody] MovieDto movieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (movieDto == null || id != movieDto.Id)
            {
                return BadRequest(ModelState);
            }

            var existingMovie = _movieRepository.GetMovie(id);
            if (existingMovie == null)
            {
                return NotFound($"Movie with ID {id} not found");
            }

            var category = _mapper.Map<Movie>(movieDto);

            if (!_movieRepository.UpdateMovie(category))
            {
                ModelState.AddModelError("", $"Something went wrong updating the movie {category.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteMovie")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteMovie(int id)
        {
            var existingMovie = _movieRepository.GetMovie(id);
            if (existingMovie == null)
            {
                return NotFound($"Movie with ID {id} not found");
            }

            if (!_movieRepository.DeleteMovie(existingMovie))
            {
                ModelState.AddModelError("", $"Something went wrong deleting the movie with ID {id}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpGet("GetMoviesByCategory/{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetMoviesByCategory(int categoryId)
        {
            var moviesList = _movieRepository.GetMoviesByCategory(categoryId);

            if (moviesList == null)
            {
                return NotFound();
            }

            var movieItem = new List<MovieDto>();
            foreach (var movie in moviesList)
            {
                movieItem.Add(_mapper.Map<MovieDto>(movie));
            }

            return Ok(movieItem);
        }

        [HttpGet("Search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Search(string name)
        {
            try
            {
                var result = _movieRepository.SearchMovie(name);
                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound();
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving application data");
            }
        }
    }
}
