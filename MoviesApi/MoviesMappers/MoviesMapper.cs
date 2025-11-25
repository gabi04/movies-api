using MoviesApi.Models;
using MoviesApi.Models.Dtos;
using AutoMapper;
using UserApi.Models.Dtos;

namespace MoviesApi.MoviesMappers
{
    public class MoviesMapper : Profile
    {
        public MoviesMapper() {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Movie, MovieDto>().ReverseMap();
            CreateMap<Movie, CreateMovieDto>().ReverseMap();
            CreateMap<Movie, UpdataMovieDto>().ReverseMap();
            CreateMap<AppUser, UserDataDto>().ReverseMap();
            CreateMap<AppUser, UserDto>().ReverseMap();
        }
    }
}
