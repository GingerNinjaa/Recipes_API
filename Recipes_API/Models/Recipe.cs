using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes_API.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(400)]
        public string Description { get; set; }
        [MaxLength(350)]
        public string ImageUrl { get; set; }
        public int PreparationTime { get; set; }
        public int CookingTime { get; set; }
        public int People { get; set; } 
        public string Preparation { get; set; }
        [MaxLength(50)]
        public string Difficulty { get; set; }
        [MaxLength(50)]
        public string Category { get; set; }
        // one Recipe
        // to many Ingredients
        public ICollection<Ingredients> Ingredients { get; set; }

    }
}
