using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation_Layer.ViewModels
{
    public class ChoiceViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Choice text is required")]
        public string ChoiceText { get; set; }
    }
}
