using News.Api.Models.Entities;
using News.Api.Repositories;

namespace News.Api.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public Task<List<Category>> GetAllAsync()
    {
        return _categoryRepository.GetAllAsync();
    }

    public Task<Category?> GetByIdAsync(Guid id)
    {
        return _categoryRepository.GetByIdAsync(id);
    }
}


