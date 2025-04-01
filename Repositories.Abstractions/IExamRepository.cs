using DataAccess_Layer.Models;
using DataAccess_Layer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Abstractions
{
    public interface IExamRepository : IGenaricRepository<Exam>
    {
        Task<Exam> GetExamWithQuestionsAndChoicesAsync(int id);
         Task<List<Exam>> GetCourseExamsAsync(int courseid);

    }
}
