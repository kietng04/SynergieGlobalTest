using Microsoft.EntityFrameworkCore;
using News.Api.Models.Entities;

namespace News.Api.Data;

public class NewsDbContext : DbContext
{
    public NewsDbContext(DbContextOptions<NewsDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Collection> Collections { get; set; }
    public DbSet<CollectionArticle> CollectionArticles { get; set; }
    public DbSet<UserSubscription> UserSubscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Password).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
        });

        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Headline).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Summary).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.Source).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Url).IsRequired().HasMaxLength(500);
            entity.HasIndex(e => e.Url).IsUnique();
            
            entity.HasOne(e => e.Category)
                  .WithMany(c => c.Articles)
                  .HasForeignKey(e => e.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Collection>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Collections)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CollectionArticle>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.HasOne(e => e.Collection)
                  .WithMany(c => c.CollectionArticles)
                  .HasForeignKey(e => e.CollectionId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(e => e.Article)
                  .WithMany(a => a.CollectionArticles)
                  .HasForeignKey(e => e.ArticleId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasIndex(e => new { e.CollectionId, e.ArticleId }).IsUnique();
        });

        modelBuilder.Entity<UserSubscription>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.EmailFrequency).IsRequired().HasMaxLength(20);
            
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Subscriptions)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(e => e.Category)
                  .WithMany(c => c.Subscriptions)
                  .HasForeignKey(e => e.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
                  
            entity.HasIndex(e => new { e.UserId, e.CategoryId }).IsUnique();
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        var techCategoryId = Guid.NewGuid();
        var sportsCategoryId = Guid.NewGuid();
        var politicsCategoryId = Guid.NewGuid();
        var healthCategoryId = Guid.NewGuid();
        var entertainmentCategoryId = Guid.NewGuid();
        
        var businessCategoryId = Guid.NewGuid();
        var generalCategoryId = Guid.NewGuid();
        var scienceCategoryId = Guid.NewGuid();

        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = businessCategoryId,
                Name = "business",
                Description = "Business news, finance, and market updates",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category
            {
                Id = entertainmentCategoryId,
                Name = "entertainment",
                Description = "Entertainment and celebrity news",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category
            {
                Id = generalCategoryId,
                Name = "general",
                Description = "General news and top stories",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category
            {
                Id = healthCategoryId,
                Name = "health",
                Description = "Health and wellness news",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category
            {
                Id = scienceCategoryId,
                Name = "science",
                Description = "Science news and discoveries",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category
            {
                Id = sportsCategoryId,
                Name = "sports",
                Description = "Sports news and updates",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Category
            {
                Id = techCategoryId,
                Name = "technology",
                Description = "Latest technology news and updates",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );

        var adminUserId = Guid.NewGuid();
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = adminUserId,
                Username = "admin",
                Email = "admin@newsapi.com",
                Password = "Admin123!",
                FirstName = "Admin",
                LastName = "User",
                Role = "Admin",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );
    }
}
