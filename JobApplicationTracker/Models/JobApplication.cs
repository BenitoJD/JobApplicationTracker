using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobApplicationTracker.Enums;

namespace JobApplicationTracker.Models
{
    public class JobApplication
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        public string JobTitle { get; set; } = string.Empty;

        public ApplicationStatus Status { get; set; }

        public DateTime DataApplied { get; set; } = DateTime.Now;

        public DateTime? FollowUpDate { get; set; }

        public string? Notes { get; set; }

    }
}
