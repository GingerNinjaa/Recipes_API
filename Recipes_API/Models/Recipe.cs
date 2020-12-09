using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

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
        [MaxLength(50)]
        public string Difficulty { get; set; }
        [MaxLength(50)]
        public string Category { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }// one Recipe
        // to many Ingredients
        public ICollection<Ingredients> Ingredients { get; set; }
        // one Recipe
        // to many PreparationStep
        public ICollection<PreparationSteps> PreparationSteps { get; set; }
        public int UserId { get; set; }
    }
}
