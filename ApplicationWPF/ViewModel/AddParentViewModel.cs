using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ApplicationWPF.Commands;
using System.Windows.Input;
using ApplicationWPF.View;
using Microsoft.EntityFrameworkCore;
using SoftwareDesignQueenAnneCuriosityShopProject;
using SoftwareDesignQueenAnneCuriosityShopProject.Entities;

namespace ApplicationWPF.ViewModel
{
    public class AddParentViewModel: NotifyPropertyChanged
    {

        private string _firstName;
        private string _lastName;
        private string _email;
        private string _phoneNumber;
        private string _network;
        public ICommand AddParentCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public string Network
        {
            get => _network;
            set
            {
                _network = value;
                OnPropertyChanged(nameof(Network));
            }
        }
        public string PhoneNumber
        {
            get=> _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }


        }



        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));

            }
        }

        public string LastName
        {
            get=> _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public string Email
        {
            get=> _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public AddParentViewModel()
        {
            AddParentCommand = new RelayCommand(AddParent, (s) => true);
            CancelCommand = new RelayCommand(Cancel, (s) => true);
        }


        private void Cancel(object obj)
        {

            var parentList = new ParentListWindow();
            parentList.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            parentList.Show();

            var w = obj as Window;
            w.Close();


        }


        private void AddParent(object obj)
        {



            // Validate required fields
            if (string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(PhoneNumber) ||
                string.IsNullOrWhiteSpace(Network))
            {
                MessageBox.Show(
                    "Please fill in all required fields before saving.",
                    "Missing Information",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            try
            {
                using var context = new Context();

                var newParent = new Parent
                {
                    FirstName = FirstName.Trim(),
                    LastName = LastName.Trim(),
                    Email = Email.Trim()
                };

                var newContact = new Contact
                {
                    ParentLink = newParent,
                    PhoneNumber = PhoneNumber.Trim(),
                    Network = Network.Trim()
                };

                // Add to context
                context.Parents.Add(newParent);
                context.Contacts.Add(newContact);

                // Save
                context.SaveChanges();

                // Show success
                MessageBox.Show(
                    "Parent successfully added!",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                // Reopen list window
                var parentListWindow = new ParentListWindow
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                parentListWindow.Show();

                // Close the current window
                if (obj is Window currentWindow)
                    currentWindow.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An error occurred while saving: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }



        }



    }
}
