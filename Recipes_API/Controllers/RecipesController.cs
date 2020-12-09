using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipes_API.Data;
using Recipes_API.Models;

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


        [Authorize]
        [HttpGet("[action]")]
        [ResponseCache(Duration = 360, Location = ResponseCacheLocation.Any)]
        public IActionResult AllRecepies(int? pageNumber, int? pageSize)
        {
            //Paginacja
            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 5;

            var recepies = this._dbContext.Recipes
                .Include(x => x.Ingredients)
                .Include(c => c.PreparationSteps);

            return Ok(recepies.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }

        [Authorize]
        [HttpGet("[action]")]
        [ResponseCache(Duration = 360, Location = ResponseCacheLocation.Any)]
        public IActionResult AllRecepiesPartial(int? pageNumber, int? pageSize)
        {
            //Paginacja
            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 5;

            var recepies = from recipe in _dbContext.Recipes
                           select new
                           {
                               Id = recipe.Id,
                               Title = recipe.Title,
                               ImageUrl = recipe.ImageUrl,
                               PreparationTime = recipe.PreparationTime,
                               Category = recipe.Category,
                               Difficulty = recipe.Difficulty,
                               People = recipe.People
                           };

            return Ok(recepies.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }

        [Authorize]
        [HttpGet("[action]/{id}")]
        [ResponseCache(Duration = 360, Location = ResponseCacheLocation.Any)]
        public IActionResult AllRecepiesPartialByUserId(int id, int? pageNumber, int? pageSize)
        {
            if (id <= 0)
            {
                return NotFound("No record found against this Id");
            }

            //Paginacja
            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 5;

            var recepies = from recipe in _dbContext.Recipes
                           where recipe.UserId == id
                           select new
                           {
                               Id = recipe.Id,
                               Title = recipe.Title,
                               ImageUrl = recipe.ImageUrl,
                               PreparationTime = recipe.PreparationTime,
                               Category = recipe.Category,
                               Difficulty = recipe.Difficulty,
                               People = recipe.People
                           };

            return Ok(recepies.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }

        [Authorize]
        [HttpGet("[action]/{id}")]
        [ResponseCache(Duration = 360, Location = ResponseCacheLocation.Any)]
        public IActionResult RecepieDetail(int id)
        {
            //var recipe = _dbContext.Recipes.Find(id);
            var recipe = this._dbContext.Recipes
                .Include(x => x.Ingredients)
                .Include(c => c.PreparationSteps).Where(v => v.Id.Equals(id));


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

        [Authorize]
        [HttpDelete("[action]/{id}")]
        public IActionResult Delete(int id)
        {
            var recepieId = _dbContext.Recipes.Find(id);
            var IngredientsesList = _dbContext.Ingredientses.Where(x => x.RecipeId.Equals(id)).ToList();
            var PreparationStepsList = _dbContext.PreparationStepses.Where(x => x.RecipeId.Equals(id)).ToList();
            if (recepieId == null)
            {
                return NotFound("No record found against this Id");
            }
            else
            {
                _dbContext.Recipes.Remove(recepieId);
                _dbContext.Ingredientses.RemoveRange(IngredientsesList);
                _dbContext.PreparationStepses.RemoveRange(PreparationStepsList);
                _dbContext.SaveChanges();
                return Ok("Record deleted");
            }
        }

        [Authorize]
        [HttpPost("[action]")]
        public IActionResult AddRecepie([FromBody] Recipe recepie)
        {
            _dbContext.Recipes.Add(recepie);
            _dbContext.SaveChanges();

            return StatusCode(StatusCodes.Status201Created);
        }

        [Authorize]
        [HttpPut("[action]")]
        public IActionResult AddRecepieIMG([FromForm] Recipe recepie)
        {
            var test = _dbContext.Recipes.Where(x => x.Title == recepie.Title).FirstOrDefault();

            if (test != null)
            {
                var guid = Guid.NewGuid();
                var filePath = Path.Combine("wwwroot", guid + ".jpg");
                //var filePath = Path.Combine("wwwroot", guid + ".jpg");
                if (recepie.Image != null)
                {
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    recepie.Image.CopyTo(fileStream);
                }
                test.ImageUrl = filePath.Remove(0, 7);

                _dbContext.SaveChanges();
            }


            return StatusCode(StatusCodes.Status200OK);
        }

        [Authorize]
        [HttpPut("[action]/{id}")]
        public IActionResult EditRecepie(int id, [FromBody] Recipe recepie)
        {
            
            var recipes = this._dbContext.Recipes.Where(y => y.Id == id)
                .Include(x => x.Ingredients)
                .Include(c => c.PreparationSteps).FirstOrDefault();
            if (recipes == null)
            {
                return NotFound("No record found against this Id");
            }
            else
            {

                recipes.Title = recepie.Title;
                recipes.Description = recepie.Description;
                recipes.PreparationTime = recepie.PreparationTime;
                recipes.CookingTime = recepie.CookingTime;
                recipes.People = recepie.People;
                recipes.Difficulty = recepie.Difficulty;
                recipes.Category = recepie.Category;
                recipes.Ingredients = recepie.Ingredients;
                recipes.PreparationSteps = recepie.PreparationSteps;

                _dbContext.SaveChanges();
                return Ok("Record updated successfully");
            }
        }

        [Authorize]
        [HttpPut("[action]/{id}")]
        public IActionResult EditRecepieIMG(int id, [FromForm] Recipe recepie)
        {
            var recipes = _dbContext.Recipes.Where(x => x.Id == id).FirstOrDefault();
            if (recipes == null)
            {
                return NotFound("No record found against this Id");
            }
            else
            {
                var guid = Guid.NewGuid();
                var filePath = Path.Combine("wwwroot", guid + ".jpg");
                if (recepie.Image != null)
                {
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    recepie.Image.CopyTo(fileStream);
                    recipes.ImageUrl = filePath.Remove(0, 7);
                }

                _dbContext.SaveChanges();
                return Ok("Record updated successfully");
            }
        }

        [Authorize]
        [HttpDelete("[action]/{id}")]
        public IActionResult DeleteRecepie(int id)
        {
            // var recipes = _dbContext.Recipes_Recipes.Find(id);

            var recepies = this._dbContext.Recipes.Where(y => y.Id == id)
                .Include(x => x.Ingredients)
                .Include(c => c.PreparationSteps).FirstOrDefault();
            if (recepies == null)
            {
                return NotFound("No record found against this Id");
            }
            else
            {
                _dbContext.Recipes.Remove(recepies);
                _dbContext.SaveChanges();
                return Ok("Record deleted");
            }
        }




    }
}
