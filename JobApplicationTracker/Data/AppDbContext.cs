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
                DataApplied = new DateTime(2025, 8, 5, 15, 25, 30, 183, DateTimeKind.Local).AddTicks(6486),
                FollowUpDate = null,
                Notes = "Initial application submitted."
            }, new JobApplication
            {
                Id = 2,
                CompanyName = "Innovatech",
                JobTitle = "Data Analyst",
                Status = Enums.ApplicationStatus.Interviewing,
                DataApplied = new DateTime(2025, 8, 10, 15, 25, 30, 185, DateTimeKind.Local).AddTicks(9775),
                FollowUpDate = null,
                Notes = "Interview scheduled for next week."
            }, new JobApplication
            {
                Id = 3,
                CompanyName = "Global Corp",
                JobTitle = "Project Manager",
                Status = Enums.ApplicationStatus.Offer,
                DataApplied = new DateTime(2025, 7, 31, 15, 25, 30, 185, DateTimeKind.Local).AddTicks(9781),
                FollowUpDate = null ,
                Notes = "Received offer letter, pending decision."
            }
            );
        }
    }
}
