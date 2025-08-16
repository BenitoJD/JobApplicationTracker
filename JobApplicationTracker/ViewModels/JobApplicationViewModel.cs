using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobApplicationTracker.Enums;
using JobApplicationTracker.Models;

namespace JobApplicationTracker.ViewModels
{
    public class JobApplicationViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly JobApplication _jobApplication;

        public JobApplicationViewModel(JobApplication jobApplication)
        {
            _jobApplication = jobApplication ?? throw new ArgumentNullException(nameof(jobApplication));
        }

        public int Id => _jobApplication.Id;

        public string CompanyName
        {
            get => _jobApplication.CompanyName;
            set
            {
                if (_jobApplication.CompanyName != value)
                {
                    _jobApplication.CompanyName = value;
                    OnPropertyChanged();
                }
            }
        }
        public string JobTitle
        {
            get => _jobApplication.JobTitle;
            set
            {
                if (_jobApplication.JobTitle != value)
                {
                    _jobApplication.JobTitle = value;
                    OnPropertyChanged();
                }
            }
        }
        public ApplicationStatus Status
        {
            get => _jobApplication.Status;
            set
            {
                if (_jobApplication.Status != value)
                {
                    _jobApplication.Status = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime DateApplied
        {
            get => _jobApplication.DataApplied;
            set
            {
                if (_jobApplication.DataApplied != value)
                {
                    _jobApplication.DataApplied = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime? FollowUpDate
        {
            get => _jobApplication.FollowUpDate;
            set
            {
                if (_jobApplication.FollowUpDate != value)
                {
                    _jobApplication.FollowUpDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Notes
        {
            get => _jobApplication.Notes;
            set
            {
                if (_jobApplication.Notes != value)
                {
                    _jobApplication.Notes = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsFollowUpDueSoon =>
            FollowUpDate.HasValue && FollowUpDate.Value.Date <= DateTime.Now.AddDays(7) && FollowUpDate.Value.Date >= DateTime.Now.Date;

        public JobApplication GetModel() => _jobApplication;

        public string Error => null!;

        public string this[string columnName]
        {
            get
            {
                string? result = null;
                switch (columnName)
                {
                    case nameof(CompanyName):
                        if (string.IsNullOrWhiteSpace(CompanyName))
                            return "Company name is required.";
                        break;
                    case nameof(JobTitle):
                        if (string.IsNullOrWhiteSpace(JobTitle))
                            return "Date applied is required.";
                        break;
                    case nameof(DateApplied):
                        if (DateApplied > DateTime.Now)
                            return "Application date cannot be in the future";
                        break;
                }
                return result!;
            }
        }
        public bool IsValid()
        {
            return string.IsNullOrEmpty(this[nameof(CompanyName)]) &&
                   string.IsNullOrEmpty(this[nameof(JobTitle)]) &&
                   string.IsNullOrEmpty(this[nameof(DateApplied)]);
        }
    }
}