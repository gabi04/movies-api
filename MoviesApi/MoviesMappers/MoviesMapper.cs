using MoviesApi.Models;
using MoviesApi.Models.Dtos;
using AutoMapper;

namespace MoviesApi.MoviesMappers
{
    public class MoviesMapper : Profile
    {
        public MoviesMapper() {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Movie, MovieDto>().ReverseMap();
            CreateMap<Movie, CreateMovieDto>().ReverseMap();
        }
    }
}
