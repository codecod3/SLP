using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationWPF.Commands;
using SoftwareDesignQueenAnneCuriosityShopProject.Entities;
using SoftwareDesignQueenAnneCuriosityShopProject;
using System.Windows.Input;
using System.Windows;

namespace ApplicationWPF.ViewModel
{
    public class EditParentViewModel: NotifyPropertyChanged
    {
        private Parent _selectedParent;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _phoneNumber;
        private string _network;
        public ICommand SaveCommand { get; set; }

        public Parent SelectedParent
        {
            get => _selectedParent;
            set
            {
                _selectedParent = value;
                OnPropertyChanged(nameof(SelectedParent));
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
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        public string Network
        {
            get => _network;
            set
            {
                _network = value;
                OnPropertyChanged(nameof(Network));
            }
        }

        public EditParentViewModel(Parent parent)
        {
            SelectedParent = parent;
            FirstName = parent.FirstName;
            LastName = parent.LastName;
            Email = parent.Email;

            using var contex = new Context();

            var contact = contex.Contacts.FirstOrDefault(c => c.ParentID == parent.ParentID);

            if (contact != null)
            {
                PhoneNumber = contact.PhoneNumber;
                Network = contact.Network;
            }

            SaveCommand = new RelayCommand(Save, (s) => true);

        }

        private void Save(object obj)
        {
            using var context = new Context();
            var parentToUpdate = context.Parents.FirstOrDefault(p => p.ParentID == SelectedParent.ParentID);
            if (parentToUpdate == null) return;
            var contactToUpdate = context.Contacts.FirstOrDefault(c => c.ParentID == SelectedParent.ParentID);
            if (contactToUpdate == null) return;

            parentToUpdate.FirstName = FirstName;
            parentToUpdate.LastName = LastName;
            parentToUpdate.Email = Email;
            contactToUpdate.PhoneNumber = PhoneNumber;
            contactToUpdate.Network = Network;

            context.SaveChanges();
            MessageBox.Show("Parent successfully updated!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            var w = obj as Window;
            w.Close();

        }

    }
}
