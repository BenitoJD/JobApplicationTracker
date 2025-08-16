using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel; // <-- Add this using statement
using System.Windows;
using System.Windows.Input;

namespace JobApplicationTracker.ViewModels
{
    public class AddEditApplicationViewModel : ViewModelBase
    {
        public JobApplicationViewModel Application { get; }

        // Change ICommand to the concrete RelayCommand type
        public RelayCommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public Action? CloseAction { get; set; }
        public bool? DialogResult { get; private set; }

        public AddEditApplicationViewModel(JobApplicationViewModel application)
        {
            Application = application;

            // The command setup remains the same
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);

            // *** THE FIX IS HERE ***
            // Subscribe to the PropertyChanged event of the nested ViewModel.
            Application.PropertyChanged += OnApplicationPropertyChanged;
        }

        /// <summary>
        /// This event handler is called whenever a property changes on the Application object.
        /// </summary>
        private void OnApplicationPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            // We tell the SaveCommand that its CanExecute condition might have changed.
            // This will cause the UI to re-evaluate the CanSave() method.
            SaveCommand.NotifyCanExecuteChanged();
        }

        private bool CanSave()
        {
            return Application.IsValid();
        }

        private void Save()
        {
            if (!Application.IsValid())
            {
                MessageBox.Show("Please correct the validation errors before saving.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DialogResult = true;
            CloseAction?.Invoke();
        }

        private void Cancel()
        {
            DialogResult = false;
            CloseAction?.Invoke();
        }
    }
}