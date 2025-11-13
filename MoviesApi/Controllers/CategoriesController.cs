using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Models;
using MoviesApi.Models.Dtos;
using MoviesApi.Repository.IRepository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MoviesApi.Controllers
{
    //[Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategories()
        {
            var categoriesList = _categoryRepository.GetCategories();
            var categoriesDtoList = new List<CategoryDto>();

            foreach(var list in categoriesList)
            {
                categoriesDtoList.Add(_mapper.Map<CategoryDto>(list));
            }

            return Ok(categoriesDtoList);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategory(int id)
        {
            var itemCategory = _categoryRepository.GetCategory(id);

            if (itemCategory == null)
            {
                return NotFound();
            }

            var itemCategoryDto = _mapper.Map<CategoryDto>(itemCategory);

            return Ok(itemCategoryDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(createCategoryDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_categoryRepository.CategoryExists(createCategoryDto.Name))
            {
                ModelState.AddModelError("", "Category exists");
                return StatusCode(404, ModelState);
            }

            var category = _mapper.Map<Category>(createCategoryDto);

            if (!_categoryRepository.CreateCategory(category))
            {
                ModelState.AddModelError("", $"Something went wrong saving the register {category.Name}");
                return StatusCode(404, ModelState);
            }

            return CreatedAtRoute("GetCategory", new { id = category.Id }, category);

        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:int}", Name = "updateCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult UpdateCategory(int id, [FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (categoryDto == null || id != categoryDto.Id)
            {
                return BadRequest(ModelState);
            }

            var currentCategory = _categoryRepository.GetCategory(id);
            if (currentCategory == null)
            {
                return NotFound($"No se encontro la categoría con ID {id}");
            }

            var category = _mapper.Map<Category>(categoryDto);

            if (!_categoryRepository.UpdateCategory(category))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando el registro{category.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}", Name = "UpdatePutCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdatePutCategory(int id, [FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (categoryDto == null || id != categoryDto.Id)
            {
                return BadRequest(ModelState);
            }

            var existingCategory = _categoryRepository.GetCategory(id);
            if (existingCategory == null)
            {
                return NotFound($"No se encontro la categoría con ID {id}");
            }

            var category = _mapper.Map<Category>(categoryDto);

            if (!_categoryRepository.UpdateCategory(category))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando el registro{category.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteCategory(int id)
        {

            if (!_categoryRepository.CategoryExists(id))
            {
                return NotFound();
            }

            var categoria = _categoryRepository.GetCategory(id);

            if (!_categoryRepository.DeleteCategory(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal borrando el registro{categoria.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
