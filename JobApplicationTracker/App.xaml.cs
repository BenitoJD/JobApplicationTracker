using JobApplicationTracker.Data;
using JobApplicationTracker.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace JobApplicationTracker
{
    public partial class App : Application
    {
        private readonly AppDbContext _context;
        private readonly MainViewModel _mainViewModel;

        public App()
        {
            _context = new AppDbContext();
            _mainViewModel = new MainViewModel(_context);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // Apply any pending migrations on startup.
            // This will create the database if it doesn't exist.
            _context.Database.Migrate();

            MainWindow = new MainWindow()
            {
                DataContext = _mainViewModel
            };

            // Trigger the command to load data when the window is loaded.
            MainWindow.Loaded += async (sender, args) =>
            {
                if (_mainViewModel.LoadApplicationsCommand.CanExecute(null))
                {
                    await _mainViewModel.LoadApplicationsCommand.ExecuteAsync(null);
                }
            };

            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}