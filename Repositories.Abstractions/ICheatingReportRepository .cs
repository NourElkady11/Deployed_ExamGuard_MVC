using Data.Models;
using DataAccess_Layer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Abstractions
{
    public interface ICheatingReportRepository : IGenaricRepository<CheatingReport>
    {
        Task<IEnumerable<CheatingReport>> GetReportsByExamIdAsync(int examId);
        Task<IEnumerable<CheatingReport>> GetReportsByStudentIdAsync(int studentId);
        Task<IEnumerable<CheatingReport>> GetReportsByExamAndStudentAsync(int examId, int studentId);
    }
}
