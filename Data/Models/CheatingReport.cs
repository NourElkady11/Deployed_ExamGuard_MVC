using DataAccess_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
	public class CheatingReport
	{
		public int Id { get; set; }
		public int? StudentId { get; set; }
		public int? ExamId { get; set; }
		public string StudentName { get; set; }
		public string StudentEmail { get; set; }
		public DateTime DetectionTime { get; set; }
		public string DetectionType { get; set; }
		public string? ImagePath { get; set; } // Path to the captured snapshot
		public int? TabSwitchCount { get; set; } // New property for tab switching count

		public Student? Student { get; set; }
		public Exam? Exam { get; set; }
	}
}
