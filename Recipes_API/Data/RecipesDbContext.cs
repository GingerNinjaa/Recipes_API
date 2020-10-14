using Microsoft.EntityFrameworkCore;
using Recipes_API.Models;

namespace Recipes_API.Data
{
    public class RecipesDbContext : DbContext
    {
        public RecipesDbContext(DbContextOptions<RecipesDbContext> options) : base(options)
        {
                
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredients> Ingredientses { get; set; }
        public DbSet<PreparationSteps> PreparationStepses { get; set; }
        public DbSet<User> Users { get; set; }
        
    }
}
