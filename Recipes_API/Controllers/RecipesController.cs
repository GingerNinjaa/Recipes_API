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
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : Controller
    {
        private RecipesDbContext _dbContext;
        public RecipesController(RecipesDbContext dbContext)
        {
            this._dbContext = dbContext;
        }


        //[Authorize]
        [HttpGet]
        [HttpGet("[action]")]
        [ResponseCache(Duration = 360, Location = ResponseCacheLocation.Any)]
        public IActionResult AllRecepies(int? pageNumber, int? pageSize)
        {
            //Paginacja
            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 5;

            var recepies = this._dbContext.Recipes
                .Include(x => x.Ingredients)
                .Include(c=>c.PreparationSteps);

            return Ok(recepies.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }

        //[Authorize]
        [HttpGet]
        [HttpGet("[action]")]
        [ResponseCache(Duration = 360, Location = ResponseCacheLocation.Any)]
        public IActionResult AllRecepiesPartial(int? pageNumber, int? pageSize)
        {
            //Paginacja
            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 5;

            var recepies = from recipe in _dbContext.Recipes  select new
            {
                    Id = recipe.Id,
                    Title = recipe.Title,
                    ImageUrl = recipe.ImageUrl,
                    PreparationTime = recipe.PreparationTime, 
                    Difficulty = recipe.Difficulty
            };

            return Ok(recepies.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }

        //[Authorize]
        [HttpGet("{id}")]
        [ResponseCache(Duration = 360, Location = ResponseCacheLocation.Any)]
        public IActionResult RecepieDetail(int id)
        {
            //var recipe = _dbContext.Recipes.Find(id);
            var recipe = this._dbContext.Recipes
                .Include(x => x.Ingredients)
                .Include(c => c.PreparationSteps).Where(v =>v.Id.Equals(id));

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
