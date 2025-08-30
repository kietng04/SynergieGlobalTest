using News.Api.Models.Entities;

namespace News.Api.Services;

public interface ICategoryService
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(Guid id);
    Task<Guid> GetIdByNameAsync(string name);
}


