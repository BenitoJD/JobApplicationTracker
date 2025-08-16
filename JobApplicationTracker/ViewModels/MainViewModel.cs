using CommunityToolkit.Mvvm.Input;
using JobApplicationTracker.Data;
using JobApplicationTracker.Enums;
using JobApplicationTracker.Models;
using JobApplicationTracker.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace JobApplicationTracker.ViewModels
{
    /// <summary>
    /// The main ViewModel for the application's primary window.
    /// Manages the collection of job applications, filtering, and user actions.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly AppDbContext _context;
        private string? _searchCompanyName;
        private ApplicationStatus? _filterStatus;
        private JobApplicationViewModel? _selectedApplication;

        public ObservableCollection<JobApplicationViewModel> Applications { get; }
        public ICollectionView ApplicationsView { get; }

        public IAsyncRelayCommand LoadApplicationsCommand { get; }
        public IAsyncRelayCommand AddApplicationCommand { get; }
        public IAsyncRelayCommand EditApplicationCommand { get; }
        public IAsyncRelayCommand DeleteApplicationCommand { get; }
        public IRelayCommand ExportToCsvCommand { get; }
        public IRelayCommand ClearFiltersCommand { get; }

        public MainViewModel(AppDbContext context)
        {
            _context = context;
            Applications = new ObservableCollection<JobApplicationViewModel>();
            ApplicationsView = CollectionViewSource.GetDefaultView(Applications);
            ApplicationsView.Filter = FilterApplications;

            LoadApplicationsCommand = new AsyncRelayCommand(LoadApplicationsAsync);
            AddApplicationCommand = new AsyncRelayCommand(AddApplicationAsync);
            EditApplicationCommand = new AsyncRelayCommand(EditApplicationAsync, CanExecuteEditOrDelete);
            DeleteApplicationCommand = new AsyncRelayCommand(DeleteApplicationAsync, CanExecuteEditOrDelete);
            ExportToCsvCommand = new RelayCommand(ExportToCsv);
            ClearFiltersCommand = new RelayCommand(ClearFilters);
        }

        public string? SearchCompanyName
        {
            get => _searchCompanyName;
            set
            {
                SetProperty(ref _searchCompanyName, value);
                ApplicationsView.Refresh(); // Re-apply the filter
            }
        }

        public ApplicationStatus? FilterStatus
        {
            get => _filterStatus;
            set
            {
                SetProperty(ref _filterStatus, value);
                ApplicationsView.Refresh(); // Re-apply the filter
            }
        }

        public JobApplicationViewModel? SelectedApplication
        {
            get => _selectedApplication;
            set
            {
                SetProperty(ref _selectedApplication, value);
                // Important: Notify commands that their CanExecute status may have changed
                EditApplicationCommand.NotifyCanExecuteChanged();
                DeleteApplicationCommand.NotifyCanExecuteChanged();
            }
        }

        /// <summary>
        /// Loads applications from the database asynchronously.
        /// </summary>
        private async Task LoadApplicationsAsync()
        {
            Applications.Clear();
            var applicationsFromDb = await _context.JobApplications.ToListAsync();
            foreach (var app in applicationsFromDb)
            {
                Applications.Add(new JobApplicationViewModel(app));
            }
        }

        /// <summary>
        /// Logic for filtering the applications in the DataGrid.
        /// </summary>
        private bool FilterApplications(object item)
        {
            if (item is not JobApplicationViewModel app) return false;

            bool isCompanyMatch = string.IsNullOrWhiteSpace(SearchCompanyName) ||
                                  app.CompanyName.Contains(SearchCompanyName, StringComparison.OrdinalIgnoreCase);

            bool isStatusMatch = !FilterStatus.HasValue || app.Status == FilterStatus.Value;

            return isCompanyMatch && isStatusMatch;
        }

        private void ClearFilters()
        {
            SearchCompanyName = null;
            FilterStatus = null;
        }

        private async Task AddApplicationAsync()
        {
            // 1. Create a new model and its ViewModel
            var newAppModel = new JobApplication { DataApplied = DateTime.Now.Date };
            var newAppViewModel = new JobApplicationViewModel(newAppModel);

            // 2. Create the dialog's ViewModel, passing the new application
            var dialogViewModel = new AddEditApplicationViewModel(newAppViewModel);

            // 3. Create the dialog window and set its DataContext
            var dialogView = new AddEditApplicationView
            {
                DataContext = dialogViewModel,
                Owner = Application.Current.MainWindow // Ensures the dialog is centered on the main window
            };

            // 4. Wire up the CloseAction
            dialogViewModel.CloseAction = dialogView.Close;

            // 5. Show the dialog and wait for it to close
            dialogView.ShowDialog();

            // 6. Check the result and act accordingly
            if (dialogViewModel.DialogResult == true)
            {
                _context.JobApplications.Add(newAppModel);
                await _context.SaveChangesAsync();
                Applications.Add(newAppViewModel);
            }
        }

        private async Task EditApplicationAsync()
        {
            if (SelectedApplication == null) return;

            // 1. Create the dialog's ViewModel, passing the selected application
            var dialogViewModel = new AddEditApplicationViewModel(SelectedApplication);

            // 2. Create the dialog window and set its DataContext
            var dialogView = new AddEditApplicationView
            {
                DataContext = dialogViewModel,
                Owner = Application.Current.MainWindow
            };

            // 3. Wire up the CloseAction
            dialogViewModel.CloseAction = dialogView.Close;

            // 4. Show the dialog and wait for it to close
            dialogView.ShowDialog();

            // 5. Check the result and act accordingly
            if (dialogViewModel.DialogResult == true)
            {
                // The ViewModel is already bound to the underlying model,
                // so we just need to check for validity and save changes.
                if (!SelectedApplication.IsValid())
                {
                    MessageBox.Show("Please correct the validation errors before saving.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    // Note: In a real-world app, you might want to revert changes here if validation fails after editing.
                    // For this project, we'll rely on the dialog's validation to prevent saving invalid data.
                    return;
                }

                // EF Core tracks changes, so we just need to call SaveChangesAsync.
                await _context.SaveChangesAsync();
                MessageBox.Show("Application updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Refresh properties that depend on edited values, like the follow-up highlighting
            }
            // If cancelled, no action is needed as changes were not saved to the database.
            // However, the object in memory might still be changed. For true cancel functionality,
            // you would edit a *copy* of the object. For this project, this approach is sufficient.
        }

        private async Task DeleteApplicationAsync()
        {
            if (SelectedApplication == null) return;

            var result = MessageBox.Show($"Are you sure you want to delete the application for '{SelectedApplication.JobTitle}' at '{SelectedApplication.CompanyName}'?",
                "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                var appInDb = await _context.JobApplications.FindAsync(SelectedApplication.Id);
                if (appInDb != null)
                {
                    _context.JobApplications.Remove(appInDb);
                    await _context.SaveChangesAsync();
                    Applications.Remove(SelectedApplication);
                }
            }
        }

        /// <summary>
        /// Determines if the Edit or Delete buttons should be enabled.
        /// </summary>
        private bool CanExecuteEditOrDelete() => SelectedApplication != null;

        private void ExportToCsv()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV file (*.csv)|*.csv",
                Title = "Export Job Applications",
                FileName = $"JobApplications_{DateTime.Now:yyyyMMdd}.csv"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var sb = new StringBuilder();
                // Add header
                sb.AppendLine("CompanyName,JobTitle,Status,DateApplied,FollowUpDate,Notes");

                foreach (var app in Applications)
                {
                    sb.AppendLine($"\"{app.CompanyName}\",\"{app.JobTitle}\",{app.Status},{app.DateApplied:yyyy-MM-dd},{app.FollowUpDate:yyyy-MM-dd},\"{app.Notes?.Replace("\"", "\"\"")}\"");
                }

                File.WriteAllText(saveFileDialog.FileName, sb.ToString());
                MessageBox.Show($"Successfully exported to {saveFileDialog.FileName}", "Export Complete", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}