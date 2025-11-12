
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ApplicationWPF.Commands;
using ApplicationWPF.View;
using Microsoft.EntityFrameworkCore;
using SoftwareDesignQueenAnneCuriosityShopProject;
using SoftwareDesignQueenAnneCuriosityShopProject.Entities;

namespace ApplicationWPF.ViewModel
{
    public class AddAdviserViewModel: NotifyPropertyChanged
    {

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


        public AddAdviserViewModel()
        {
            SaveCommand = new RelayCommand(AddAdviser, (s) => true);
            CancelCommand = new RelayCommand(Cancel, (s)=> true);
        }



        private void Cancel(object obj)
        {



        }

        private void AddAdviser(object obj)
        {

            using var context = new Context();
            var newAdviser = new ClassAdviser()
            {
                FirstName = FirstName,
                LastName = LastName,
                PhoneNumber = PhoneNumber,

            };
            context.ClassAdvisers.Add(newAdviser);
            context.SaveChanges();

            var w = obj as Window;
            w.Close();

        }

    }
}
