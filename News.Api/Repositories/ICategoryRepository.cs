using News.Api.Models.Entities;

namespace News.Api.Repositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(Guid id);
    Task<Guid> GetIdByNameAsync(string name);
}


