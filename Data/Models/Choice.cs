using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Choice
    {
        public int Id { get; set; }
        public string? ChoiceText { get; set; }

        public Question? Question { get; set; }
        public int? QuestionId { get; set; }

        public ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();

    }
}
