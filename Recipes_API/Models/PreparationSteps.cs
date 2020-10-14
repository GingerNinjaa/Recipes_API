using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes_API.Models
{
    public class PreparationSteps
    {
        public int Id { get; set; }
        public int StepNumber { get; set; }
        [MaxLength(350)]
        public string Text { get; set; }
    }
}
