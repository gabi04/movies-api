using MoviesApi.Data;
using MoviesApi.Models;
using MoviesApi.Repository.IRepository;

namespace MoviesApi.Repository
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly Context _db;

        public CategoryRepository(Context db)
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
        }
    }
}
