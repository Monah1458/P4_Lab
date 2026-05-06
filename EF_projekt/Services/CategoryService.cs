using EF_projekt.Relations;
using EF_projekt.Repositories.Interfaces;
using EF_projekt.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_projekt.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) throw new KeyNotFoundException($"Category {id} not found.");
            return category;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _unitOfWork.Categories.GetAllAsync();
        }

        public async Task<Category> CreateCategoryAsync(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                throw new ArgumentException("Category name is required.");

            var category = new Category { CategoryName = categoryName };
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.CompleteAsync();
            
            return category;
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));
            var existing = await GetCategoryByIdAsync(category.IdCategory);
            existing.CategoryName = category.CategoryName;
            _unitOfWork.Categories.Update(existing);
            await _unitOfWork.CompleteAsync();
           
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await GetCategoryByIdAsync(id); // ensure exists
            await _unitOfWork.Categories.DeleteByIdAsync(id);
            await _unitOfWork.CompleteAsync();          
        }
       
    }
}
