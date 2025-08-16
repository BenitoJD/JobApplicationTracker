using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using CommunityToolkit.Mvvm.Input;
using JobApplicationTracker.Data;
using JobApplicationTracker.Enums;
using JobApplicationTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

namespace JobApplicationTracker.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly AppDbContext _context;

        private string? _searchCompanyName;

        private ApplicationStatus? _filterStatus;

        private JobApplicationViewModel? _selectedApplication;

        public ObservableCollection<JobApplicationViewModel> Applications { get; }

        public ICollectionView ApplicationView { get; }

        public IAsyncRelayCommand LoadApplicationCommand { get; }

        public IAsyncRelayCommand AddApplicationCommand { get; }

        public IAsyncRelayCommand EditApplicationCommand { get; }

        public IAsyncRelayCommand DeleteApplicationCommand { get; }

        public IRelayCommand ExportToCsvCommand { get; }

        public IRelayCommand ClearFilterCommand { get; }



        public MainViewModel(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            Applications = new ObservableCollection<JobApplicationViewModel>();
            ApplicationView = CollectionViewSource.GetDefaultView(Applications);
            ApplicationView.Filter = FilterApplications;

            
            LoadApplicationCommand = new AsyncRelayCommand(LoadApplicationAsync);
            AddApplicationCommand = new AsyncRelayCommand(AddApplicationAsync);
            EditApplicationCommand = new AsyncRelayCommand(EditApplicationAsync);
            DeleteApplicationCommand = new AsyncRelayCommand(DeleteApplicationAsync);

            ExportToCsvCommand = new RelayCommand(ExportToCsv);
            ClearFilterCommand = new RelayCommand(ClearFilter);
        }

        public string? SearchCompanyName
        {
            get => _searchCompanyName;
            set
            {
               SetProperty(ref _searchCompanyName, value);
                ApplicationView.Refresh();
            }
        }
        public ApplicationStatus? FilterStatus
        {
            get => _filterStatus;
            set
            {
                SetProperty(ref _filterStatus, value);
                ApplicationView.Refresh();
            }
        }
        public JobApplicationViewModel? SelectedApplication
        {
            get => _selectedApplication;
            set 
            {
                SetProperty(ref _selectedApplication, value);
                EditApplicationCommand.NotifyCanExecuteChanged();
                DeleteApplicationCommand.NotifyCanExecuteChanged();
            }
        }
        private async Task LoadApplicationAsync()
        {
            Applications.Clear();
            var applications = await _context.JobApplications.ToListAsync();
            foreach (var app in applications)
            {
                Applications.Add(new JobApplicationViewModel(app));
            }
        }

        private bool FilterApplications(object obj)
        {
            if (obj is not JobApplicationViewModel app) return false;
            bool matchesCompany = string.IsNullOrEmpty(SearchCompanyName) ||
                                  app.CompanyName.Contains(SearchCompanyName, StringComparison.OrdinalIgnoreCase);
            bool matchesStatus = !FilterStatus.HasValue || app.Status == FilterStatus;
            return matchesCompany && matchesStatus;
        }
        private void ClearFilter()
        {
            SearchCompanyName = null;
            FilterStatus = null;
            ApplicationView.Refresh();
        }
        private async Task AddApplicationAsync()
        {
            var newApp = new JobApplication { DataApplied = DateTime.Now };
            var vm = new JobApplicationViewModel(newApp);

            if(!vm.IsValid())
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _context.JobApplications.Add(newApp);
            await _context.SaveChangesAsync();
            Applications.Add(vm);
        }
        private async Task EditApplicationAsync()
        {
            if (SelectedApplication == null) return;
            if(!SelectedApplication.IsValid())
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var appInDb = await _context.JobApplications.FindAsync(SelectedApplication.Id);
            if(appInDb != null)
            {
                appInDb.CompanyName = SelectedApplication.CompanyName;
                appInDb.JobTitle = SelectedApplication.JobTitle;
                appInDb.Status = SelectedApplication.Status;
                appInDb.DataApplied = SelectedApplication.DateApplied;
                appInDb.FollowUpDate = SelectedApplication.FollowUpDate;
                appInDb.Notes = SelectedApplication.Notes;
                await _context.SaveChangesAsync();
                MessageBox.Show("Application updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Selected application not found in the database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

    }
        private async Task DeleteApplicationAsync()
        {
            if (SelectedApplication == null) return;
            var result = MessageBox.Show("Are you sure you want to delete this application?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes) return;
            var appInDb = await _context.JobApplications.FindAsync(SelectedApplication.Id);
            if (appInDb != null)
            {
                _context.JobApplications.Remove(appInDb);
                await _context.SaveChangesAsync();
                Applications.Remove(SelectedApplication);
                SelectedApplication = null;
                MessageBox.Show("Application deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Selected application not found in the database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool CanExecuteEditDelete() => SelectedApplication != null;

        private void ExportToCsv()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
                DefaultExt = ".csv",
                FileName = $"JobApplications_{DateTime.Now:yyyyMMdd}.csv"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var sb = new StringBuilder();
                sb.AppendLine("CompanyName,JobTitle,Status,DateApplied,FollowDate,Notes");
                foreach (var app in Applications)
                {
                    sb.AppendLine($"{app.CompanyName},{app.JobTitle},{app.Status},{app.DateApplied:yyyy-MM-dd},{app.FollowUpDate:yyyy-MM-dd},{app.Notes}");

                }
                File.WriteAllText(saveFileDialog.FileName, sb.ToString());
                MessageBox.Show("Applications exported successfully.", "Export Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }
    }
}
