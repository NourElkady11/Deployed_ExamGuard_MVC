using Data.Data;
using DataAccess_Layer.Models;
using DataAccess_Layer.Repositories;
using Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    internal class ExamRepository : GenricRepository<Exam>, IExamRepository
    {
        public ExamRepository(DataContext context) : base(context)
        {

        }

    }
}
