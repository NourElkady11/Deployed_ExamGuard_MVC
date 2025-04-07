using Data.Data;
using Data.Migrations.Data;
using Data.Models;
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
    public class StudentExamRepository : GenricRepository<StudentExam>, IStudentExamRepository
    {
        public StudentExamRepository(DataContext context) : base(context)
        {

        }
    }
}
