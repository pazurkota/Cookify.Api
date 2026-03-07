using Cookify.Api.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cookify.Api.Database;

public class AppDbContext(DbContextOptions<AppDbContext> contextOptions) : IdentityDbContext(contextOptions)
{
    public DbSet<Recipe> Recipes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // config one-to-many (user -> recipe)
        builder.Entity<Recipe>()
            .HasOne(r => r.Author)
            .WithMany(u => u.Recipes)
            .HasForeignKey(r => r.AuthorId);

        builder.Entity<Recipe>()
            .Property(r => r.Id)
            .ValueGeneratedOnAdd();
    }
}