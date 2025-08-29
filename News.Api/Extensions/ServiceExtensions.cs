using News.Api.Data;
using News.Api.Models;
using News.Api.Services;
using News.Api.Repositories;
using News.Api.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace News.Api.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<NewsDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        
        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<IArticleService, ArticleService>();

        services.AddScoped<ICollectionRepository, CollectionRepository>();
        services.AddScoped<ICollectionService, CollectionService>();
        services.AddScoped<ICollectionArticleRepository, CollectionArticleRepository>();
        services.AddScoped<ICollectionArticleService, CollectionArticleService>();

        services.AddScoped<IPasswordHashingService, PasswordHashingService>();
        
        services.Configure<JwtConfig>(configuration.GetSection("Jwt"));
        services.AddScoped<IJwtService, JWTService>();

        return services;
    }

    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "News API",
                Version = "v1",
                Description = "A simple news API with layered architecture",
                Contact = new Microsoft.OpenApi.Models.OpenApiContact
                {
                    Name = "API Support",
                    Email = "support@newsapi.com"
                }
            });
        });

        return services;
    }
}
