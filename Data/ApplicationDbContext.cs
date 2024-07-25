    using CollegeAndCourses.Models;
    using System.Collections.Generic;
    using System.Reflection.Emit;


    using Microsoft.EntityFrameworkCore;

    namespace CollegeAndCourses.Data
    {
        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
            {
            }

            public DbSet<College> Colleges { get; set; }
            public DbSet<Course> Courses { get; set; }

       
        }
    }
