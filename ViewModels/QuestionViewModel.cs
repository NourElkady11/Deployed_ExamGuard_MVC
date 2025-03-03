using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation_Layer.ViewModels
{
    public class QuestionViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public List<ChoiceViewModel> Choices { get; set; } = new List<ChoiceViewModel>();
        public int CorrectChoiceId { get; set; }
    }
}
