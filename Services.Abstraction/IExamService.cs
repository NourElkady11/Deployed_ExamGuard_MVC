﻿using Presentation_Layer;
using Presentation_Layer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction
{
    public interface IExamService
    {
        public Task<IEnumerable<ExamViewModel>> GetExams();
    }
}
