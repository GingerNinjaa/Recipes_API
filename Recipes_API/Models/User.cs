using System.ComponentModel.DataAnnotations;

namespace Recipes_API.Models
{
    public class User
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        public string Password { get; set; }
        [MaxLength(100)]
        public string Role { get; set; }
    }
}
