using EF_projekt.Relations;

namespace EF_projekt.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<Category> GetCategoryByIdAsync(int id);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> CreateCategoryAsync(string categoryName);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
      
    }
}