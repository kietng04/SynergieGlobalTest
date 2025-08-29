using Microsoft.AspNetCore.Mvc;
using News.Api.Models.Dtos;
using News.Api.Models.Entities;
using News.Api.Services;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<Category>>>> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();
        return Ok(new ApiResponse<List<Category>>
        {
            Success = true,
            Data = categories,
            Message = "Categories fetched successfully",
            Timestamp = DateTime.UtcNow
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<Category>>> GetById(Guid id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        return Ok(new ApiResponse<Category>
        {
            Success = true,
            Data = category,
            Message = "Category fetched successfully",
            Timestamp = DateTime.UtcNow
        });
    }
}