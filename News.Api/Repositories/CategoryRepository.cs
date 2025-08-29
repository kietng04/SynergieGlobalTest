using Microsoft.EntityFrameworkCore;
using News.Api.Data;
using News.Api.Models.Entities;

namespace News.Api.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly NewsDbContext _dbContext;

    public CategoryRepository(NewsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<Category>> GetAllAsync()
    {
        return _dbContext.Categories
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public Task<Category?> GetByIdAsync(Guid id)
    {
        return _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
    }
}


