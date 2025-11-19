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
using ApplicationWPF.View;

namespace ApplicationWPF.ViewModel
{
    public class EditAdviserViewModel: NotifyPropertyChanged
    {
        private ClassAdviser _selectedAdviser;
        private string _firstName;
        private string _lastName;
        private string _phoneNumber;
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

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

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }


        public ClassAdviser SelectedAdviser
        {
            get => _selectedAdviser;
            set
            {
                _selectedAdviser = value;
                OnPropertyChanged(nameof(SelectedAdviser));
            }
        }



        public EditAdviserViewModel(ClassAdviser adviser)
        {
            SelectedAdviser = adviser;
            SaveCommand = new RelayCommand(Save, (s) => true);
            CancelCommand = new RelayCommand(Cancel, (s) => true);
            FirstName = adviser.FirstName;
            LastName = adviser.LastName;
            PhoneNumber = adviser.PhoneNumber;

        }

        private void Cancel(object obj)
        {

            var window = new AdviserListWindow();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Show();

            var w = obj as Window;
            w.Close();

        }


        private void Save(object obj)
        {

            using var context = new Context();
            var adviserToUpdate = context.ClassAdvisers.FirstOrDefault(a => a.ClassAdviserID == SelectedAdviser.ClassAdviserID);

            if (adviserToUpdate == null) return;

            adviserToUpdate.FirstName = FirstName;
            adviserToUpdate.LastName = LastName;
            adviserToUpdate.PhoneNumber = PhoneNumber;

            context.SaveChanges();
            MessageBox.Show("Adviser successfully updated!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            var window = new AdviserListWindow();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Show();

            var w = obj as Window;
            w.Close();



        }
    }
}
