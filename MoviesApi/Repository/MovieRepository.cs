using Microsoft.EntityFrameworkCore;
using MoviesApi.Data;
using MoviesApi.Models;
using MoviesApi.Repository.IRepository;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MoviesApi.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly Context _db;

        public MovieRepository(Context db)
        {
            _db = db;
        }

        public ICollection<Movie> GetMovies(int pageNumber, int pageSize)
        {
            return _db.Movies.OrderBy(m => m.Name)
                .Skip((pageNumber -1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
        public int GetTotalMovies()
        {
            return _db.Movies.Count();
        }
        public ICollection<Movie> GetMoviesByCategory(int categoryId)
        {
            return _db.Movies.Include(c => c.Category).Where(c => c.categoryId == categoryId).ToList();
        }
        public IEnumerable<Movie> SearchMovie(string name)
        {
            IQueryable<Movie> query = _db.Movies;
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.Name.Contains(name) || e.Description.Contains(name));
            }
            return query.ToList();
        }
        public Movie GetMovie(int id)
        {
            return _db.Movies.FirstOrDefault(m => m.Id == id);
        }
        public bool MovieExists(int id)
        {
            return _db.Movies.Any(m => m.Id == id);
        }
        public bool MovieExists(string name)
        {
            return _db.Movies.Any(m => m.Name.ToLower().Trim() == name.ToLower().Trim());
        }
        public bool CreateMovie(Movie movie)
        {
            movie.CreationDate = DateTime.Now;
            _db.Movies.Add(movie);
            return Save();
        }
        public bool UpdateMovie(Movie movie)
        {
            movie.CreationDate = DateTime.Now;
            var existingMovie = _db.Movies.Find(movie.Id);
            if (existingMovie != null)
            {
                _db.Entry(existingMovie).CurrentValues.SetValues(movie);
            }
            else
            {
                _db.Movies.Update(movie);
            }

            return Save();
        }
        public bool DeleteMovie(Movie Movie)
        {
            _db.Movies.Remove(Movie);
            return Save();
        }
        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }





        /*public CategoryRepository(Context db)
        {
            _db = db;
        }

        public bool CategoryExists(int id)
        {
            return _db.Categories.Any(c => c.Id == id);
        }

        public bool CategoryExists(string name)
        {
            return _db.Categories.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
            
        }

        public bool CreateCategory(Category category)
        {
            category.CreationDate = DateTime.Now;
            _db.Categories.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _db.Categories.Remove(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            return _db.Categories.OrderBy(c => c.Name).ToList();
        }

        public Category GetCategory(int id)
        {
            return _db.Categories.FirstOrDefault(c => c.Id == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            category.CreationDate = DateTime.Now;
            var existingCategory = _db.Categories.Find(category.Id);
            if (existingCategory != null)
            {
                _db.Entry(existingCategory).CurrentValues.SetValues(category);
            }
            else
            {
                _db.Categories.Update(category);
            }

            return Save();
        }*/
    }
}
