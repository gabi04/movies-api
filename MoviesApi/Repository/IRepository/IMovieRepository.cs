using MoviesApi.Models;

namespace MoviesApi.Repository.IRepository
{
    public interface IMovieRepository
    {
        ICollection<Movie> GetMovies(int pageNumber, int pageSize);
        int GetTotalMovies();
        ICollection<Movie> GetMoviesByCategory(int categoryId);
        IEnumerable<Movie> SearchMovie(string name);
        Movie GetMovie(int id);
        bool MovieExists(int id);
        bool MovieExists(string name);
        bool CreateMovie(Movie movie);
        bool UpdateMovie(Movie Movie);
        bool DeleteMovie(Movie Movie);
        bool Save();
    }
}
