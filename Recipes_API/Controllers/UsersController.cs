using AuthenticationPlugin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Recipes_API.Data;
using Recipes_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Recipes_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private RecipesDbContext _dbContext;

        private IConfiguration _configuration;

        private readonly AuthService _authService;

        public UsersController(RecipesDbContext dbContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _authService = new AuthService(_configuration);
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register([FromBody] User user)
        {
            var userWithSameEmail = _dbContext.Users.Where(u => u.Email == user.Email).SingleOrDefault();
            if (userWithSameEmail != null)
            {
                return BadRequest("User with same email already exists");
            }
            var userObj = new User
            {
                Name = user.Name,
                Email = user.Email,
                // Password = SecurePasswordHasherHelper.Hash(user.Password)
                Password = user.Password,
                Role = "Users"
            };
            _dbContext.Users.Add(userObj);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] User user)
        {
            var userEmail = _dbContext.Users.FirstOrDefault(u => u.Email == user.Email);
            if (userEmail == null)
            {
                return NotFound();
            }

            // var password = SecurePasswordHasherHelper.Verify(user.Password);
            if (userEmail.Password.Equals(user.Password) == false)
            {
                return Unauthorized();
            }

            var claims = new[]
            {
               new Claim(JwtRegisteredClaimNames.Email, user.Email),
               new Claim(ClaimTypes.Email, user.Email),
               new Claim(ClaimTypes.Role,userEmail.Role)
            };

            var token = _authService.GenerateAccessToken(claims);

            return new ObjectResult(new
            {
                access_token = token.AccessToken,
                expires_in = token.ExpiresIn,
                token_type = token.TokenType,
                creation_Time = token.ValidFrom,
                expiration_Time = token.ValidTo,
                user_id = userEmail.Id,
                user_Name = userEmail.Name
            });
        }

        [Authorize]
        [HttpPut]
        public IActionResult Edit([FromBody] User user)
        {
            using (RecipesDbContext db = _dbContext)
            {
                var existingUser = db.Users.Where(u => u.Id == user.Id).FirstOrDefault();

                if (user.Email != existingUser.Email &&
                    user.Email.Trim() != "" &&
                    db.Users.Any(x => x.Email.Equals(user.Email)) == false)
                {
                    existingUser.Email = user.Email;
                }
                else if (user.Email.Trim() == "")
                {
                    existingUser.Email = existingUser.Email;
                }
                else
                {
                    return NotFound("Email already exist in database");
                }


                if (user.Name != existingUser.Name &&
                    user.Name.Trim() != "" &&
                    db.Users.Any(x => !x.Name.Equals(user.Name)))
                {
                    existingUser.Name = user.Name;
                }
                else if (user.Name.Trim() == "")
                {
                    existingUser.Name = existingUser.Name;
                }
                else
                {
                    return NotFound("Name already exist in database");
                }


                if (user.Password.Trim() != "")
                {

                    existingUser.Password = user.Password;
                }
                else if (user.Password.Trim() == "")
                {
                    existingUser.Password = existingUser.Password;
                }
                else
                {
                    return NotFound("Password");
                }

                db.SaveChanges();
                return StatusCode(StatusCodes.Status200OK);
            }

        }


    }
}
