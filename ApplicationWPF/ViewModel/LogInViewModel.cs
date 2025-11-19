using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using SoftwareDesignQueenAnneCuriosityShopProject;
using SoftwareDesignQueenAnneCuriosityShopProject.Entities;
using ApplicationWPF.Commands;
using ApplicationWPF.View;

namespace ApplicationWPF.ViewModel
{
    public class LogInViewModel : NotifyPropertyChanged
    {
        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        private string _id;
        public string ID
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        public ICommand LogInCommand { get; set; }

        public LogInViewModel()
        {
            LogInCommand = new RelayCommand(LogIn, (s)=>true);
        }

        private bool CanLogIn(object obj)
        {
            // Only enable login if all fields are filled
            return !string.IsNullOrWhiteSpace(UserName)
                   && !string.IsNullOrWhiteSpace(LastName)
                   && !string.IsNullOrWhiteSpace(ID);
        }

        private void LogIn(object obj)
        {
            if (!int.TryParse(ID, out int userId))
            {
                MessageBox.Show("ID must be a number.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var db = new Context();

            // Convert both DB fields and input to lowercase and trim spaces for comparison
            string firstNameInput = UserName?.Trim().ToLower() ?? string.Empty;
            string lastNameInput = LastName?.Trim().ToLower() ?? string.Empty;

            var user = db.Users
                .AsEnumerable() // switch to client evaluation to allow complex comparisons
                .FirstOrDefault(u =>
                    u.UserID == userId &&
                    (u.FirstName?.Trim().ToLower() ?? string.Empty) == firstNameInput &&
                    (u.LastName?.Trim().ToLower() ?? string.Empty) == lastNameInput
                );

            if (user == null)
            {
                MessageBox.Show("User not found. Please check your credentials.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            

            // Open main window
            var mainWindow = new HomePageView();
            mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            mainWindow.Show();

            MessageBox.Show($"Welcome {user.FirstName}!", "Login Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Close login window
            if (obj is Window w)
                w.Close();
        }

    }
}
