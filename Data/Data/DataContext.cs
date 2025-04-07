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

            modelBuilder.Entity<Course>().HasMany(e => e.Exams).WithOne(c => c.Course).HasForeignKey(e => e.CourseId);

            modelBuilder.Entity<SuperVisor>().HasMany(s => s.courses).WithOne(s => s.superVisor).HasForeignKey(s => s.SuperVisorId);

            modelBuilder.Entity<Exam>().HasMany(q => q.Questions).WithOne(e => e.Exam).HasForeignKey(q => q.ExamId);
            modelBuilder.Entity<Question>().HasMany(c => c.Choices).WithOne(q => q.Question).HasForeignKey(c => c.QuestionId);
            //Create the relation

            modelBuilder.Entity<StudentExam>()
                .HasKey(se => new { se.StudentId, se.ExamId });

            modelBuilder.Entity<Student>().HasMany(se => se.studentExams).WithOne(s => s.student).HasForeignKey(se => se.StudentId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Exam>().HasMany(se => se.studentExams).WithOne(e => e.exam).HasForeignKey(se => se.ExamId).OnDelete(DeleteBehavior.Cascade);


        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<SuperVisor> SuperVisors { get; set; }
        public DbSet<StudentExam> StudentExams { get; set; }
        public DbSet<CourseStudent> CourseStudents { get; set; }
        public DbSet<Exam> Exams { get; set; }


    }
}
