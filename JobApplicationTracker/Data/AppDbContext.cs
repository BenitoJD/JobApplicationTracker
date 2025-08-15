using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobApplicationTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Data
{
    public class AppDbContext: DbContext
    {
        public DbSet<JobApplication> JobApplications { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite("Data Source=JobApplicationTracker.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobApplication>().HasData(new JobApplication
            {
                Id = 1,
                CompanyName = "Tech Solutions",
                JobTitle = "Software Engineer",
                Status = Enums.ApplicationStatus.Applied,
                DataApplied = DateTime.Now.AddDays(-10),
                FollowUpDate = DateTime.Now.AddDays(5),
                Notes = "Initial application submitted."
            }, new JobApplication
            {
                Id = 2,
                CompanyName = "Innovatech",
                JobTitle = "Data Analyst",
                Status = Enums.ApplicationStatus.Interviewing,
                DataApplied = DateTime.Now.AddDays(-5),
                FollowUpDate = DateTime.Now.AddDays(3),
                Notes = "Interview scheduled for next week."
            }, new JobApplication
            {
                Id = 3,
                CompanyName = "Global Corp",
                JobTitle = "Project Manager",
                Status = Enums.ApplicationStatus.Offer,
                DataApplied = DateTime.Now.AddDays(-15),
                FollowUpDate = DateTime.Now.AddDays(10),
                Notes = "Received offer letter, pending decision."
            }
            );
        }
    }
}
