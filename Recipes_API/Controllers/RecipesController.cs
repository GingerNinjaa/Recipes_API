using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipes_API.Data;

namespace Recipes_API.Controllers
{
    public class RecipesController : Controller
    {
        private RecipesDbContext _dbContext;
        public RecipesController(RecipesDbContext dbContext)
        {
            this._dbContext = dbContext;
        }


        [Authorize]
        [HttpGet("[action]")]
        [ResponseCache(Duration = 360, Location = ResponseCacheLocation.Any)]
        public IActionResult AllRecepies(int? pageNumber, int? pageSize)
        {
            //Paginacja
            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 5;

            var recepies = this._dbContext.Recipes.Include(x => x.Ingredients);

            return Ok(recepies.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }

        [Authorize]
        [HttpGet("[action]/{id}")]
        [ResponseCache(Duration = 360, Location = ResponseCacheLocation.Any)]
        public IActionResult RecepieDetail(int id)
        {
            var recipe = _dbContext.Recipes.Find(id);

            if (recipe == null)
            {
                return NotFound();
            }

            return Ok(recipe);
        }

        [Authorize]
        [HttpGet("[action]")]
        [ResponseCache(Duration = 360, Location = ResponseCacheLocation.Any)]
        public IActionResult FindRecipe(string recipeName)
        {
            var recepies = from recipe in _dbContext.Recipes
                where recipe.Title.StartsWith(recipeName)
                select new
                {
                    Id = recipe.Id,
                    Title = recipe.Title,
                    ImageUrl = recipe.ImageUrl
                };

            return Ok(recepies);
        }
    }
}
