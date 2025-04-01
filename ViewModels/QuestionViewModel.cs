using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation_Layer.ViewModels
{
    public class QuestionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Question text is required")]
        public string? QuestionText { get; set; }

        [Required(ErrorMessage = "Correct answer choice is required")]
        public int CorrectChoiceIndex { get; set; }

        public List<ChoiceViewModel>? Choices { get; set; } = new List<ChoiceViewModel>();
    }
}
