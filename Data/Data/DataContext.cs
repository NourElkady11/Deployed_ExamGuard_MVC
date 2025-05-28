using Data.Models;
using DataAccess_Layer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

            modelBuilder.Entity<CourseStudent>()
                .HasKey(cs => new { cs.StudentId, cs.CourseId });

            modelBuilder.Entity<CourseStudent>()
                .HasOne(cs => cs.Student)
                .WithMany(s => s.CourseStudents)
                .HasForeignKey(cs => cs.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CourseStudent>()
                .HasOne(cs => cs.Course)
                .WithMany(c => c.CourseStudents)
                .HasForeignKey(cs => cs.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

             //////////////////////////////////////////////////////////////////////////////
             //lama tams7 el exam haytms7 3adyw hayms7 el parents lly mawgod 3andhom el forign key dh bas el 7agat lly atms7t dy zy el question haya kaman hatms7 el student anwser raw f maynfa34 3amlyten ye3mlo nafs el 7agaa

            modelBuilder.Entity<StudentAnswer>()
                .HasOne(sa => sa.Student)
                .WithMany(s => s.StudentAnswers)
                .HasForeignKey(sa => sa.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StudentAnswer>()
                .HasOne(sa => sa.Exam)
                .WithMany(e => e.StudentAnswers)
                .HasForeignKey(sa => sa.ExamId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StudentAnswer>()
                .HasOne(sa => sa.Question)
                .WithMany(q => q.StudentAnswers)
                .HasForeignKey(sa => sa.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<StudentAnswer>()
                .HasOne(sa => sa.Choice)
                .WithMany(c => c.StudentAnswers)
                .HasForeignKey(sa => sa.ChoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            //////////////////////////////////////////////////////////////////////////////////////
            // exam hayms7 quetions m3ah hal el question mawwgod f table tany 

            modelBuilder.Entity<Course>().HasMany(e => e.Exams).WithOne(c => c.Course).HasForeignKey(e => e.CourseId);

            modelBuilder.Entity<SuperVisor>().HasMany(s => s.courses).WithOne(s => s.superVisor).HasForeignKey(s => s.SuperVisorId);

            modelBuilder.Entity<Exam>().HasMany(q => q.Questions).WithOne(e => e.Exam).HasForeignKey(q => q.ExamId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Question>().HasMany(c => c.Choices).WithOne(q => q.Question).HasForeignKey(c => c.QuestionId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StudentExam>()
                .HasKey(se => new { se.StudentId, se.ExamId });

            modelBuilder.Entity<Student>().HasMany(se => se.studentExams).WithOne(s => s.student).HasForeignKey(se => se.StudentId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Exam>().HasMany(se => se.studentExams).WithOne(e => e.exam).HasForeignKey(se => se.ExamId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Exam>().HasMany(se => se.CheatingReports).WithOne(e => e.Exam).HasForeignKey(se => se.ExamId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Student>().HasMany(se => se.CheatingReports).WithOne(e => e.Student).HasForeignKey(se => se.StudentId);

        }

        public DbSet<Exam> Exams { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<SuperVisor> SuperVisors { get; set; }
        public DbSet<StudentExam> StudentExams { get; set; }
        public DbSet<StudentAnswer> studentAnswers { get; set; }
        public DbSet<CourseStudent> CourseStudents { get; set; }
        public DbSet<CheatingReport> CheatingReports { get; set; }


    }
}
