using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes_API.Models
{
    public class Ingredients
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public double Amount { get; set; }
        [MaxLength(100)]
        public string AmountDesc { get; set; }

        public int RecipeId { get; set; }
    }
}
