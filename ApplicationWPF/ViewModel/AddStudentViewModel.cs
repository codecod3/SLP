using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class AddStudentViewModel: NotifyPropertyChanged 
    {
       private string _firstName;
       private string _lastName;
       private int _lrn;
       private string _phoneNumber;
        private Parent _selectedParent;
        private ClassAdviser _selectedAdviser;
        public event Action RequestClose;
        public ICommand AddStudentCommand { get; set; }

        private string _parentNameInput;
        private string _adviserNameInput;

        public string FirstName
        {
            get => _firstName;
            set { _firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        public int LRN
        {
            get => _lrn;
            set
            {
                _lrn = value;
                OnPropertyChanged();
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }


        public Parent SelectedParent
        {
            get => _selectedParent;
            set
            {
                if (_selectedParent != value)
                {
                    _selectedParent = value;
                    OnPropertyChanged(nameof(SelectedParent));
                    if (_selectedParent != null)
                        ParentNameInput = $"{_selectedParent.FirstName} {_selectedParent.LastName}";
                }
            }
        }

        public ClassAdviser SelectedAdviser
        {
            get => _selectedAdviser;
            set
            {
                if (_selectedAdviser != value)
                {
                    _selectedAdviser = value;
                    OnPropertyChanged(nameof(SelectedAdviser));
                    if (_selectedAdviser != null)
                        AdviserNameInput = $"{_selectedAdviser.FirstName} {_selectedAdviser.LastName}";
                }
            }
        }


        public string ParentNameInput
        {
            get => _parentNameInput;
            set
            {
                if (_parentNameInput != value)
                {
                    _parentNameInput = value;
                    OnPropertyChanged(nameof(ParentNameInput));
                    UpdateFilteredParents();
                }
            }
        }

        public string AdviserNameInput
        {
            get => _adviserNameInput;
            set
            {
                if (_adviserNameInput != value)
                {
                    _adviserNameInput = value;
                    OnPropertyChanged(nameof(AdviserNameInput));
                    UpdateFilteredAdvisers();
                }
            }
        }


        public AddStudentViewModel()
        {
            AddStudentCommand = new RelayCommand(AddStudent, (s) => true);
            LoadAdvisers();
            LoadParents();
        }


        private void AddStudent(object obj)
        {
            using var context = new Context();
            var newStudent = new Student
            {
                FirstName = FirstName,
                LastName = LastName,
                LRN = LRN,
                PhoneNumber = PhoneNumber,
                EnrollmentStatus = true
            };
           

            if(SelectedParent != null)
            {

                var relation = new Relationship
                {
                    StudentLink = newStudent,
                    ParentLink = SelectedParent,
                    StudentID = newStudent.StudentID,
                    ParentID = SelectedParent.ParentID

                };
                context.Relationships.Add(relation);

            }
           
            if(SelectedAdviser != null)
            {
                var advisory = new Advisory
                {
                    ClassAdviserLink = SelectedAdviser,
                    ClassAdviserID = SelectedAdviser.ClassAdviserID
                };
                advisory.Students.Add(newStudent);
                context.Advisories.Add(advisory);
                newStudent.AdvisoryLink = advisory;
                newStudent.AdvisoryID = advisory.AdvisoryID;


            }

           

            context.Students.Add(newStudent);
            
            context.SaveChanges();

            var w = obj as Window;
            w.Close();
            var s = new StudentListWindow();
            s.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            s.Show();
        }



        public ObservableCollection<Parent> AllParents { get; } = new();
        public ObservableCollection<Parent> FilteredParents { get; } = new();

        public ObservableCollection<ClassAdviser> AllAdvisers { get; } = new();
        public ObservableCollection<ClassAdviser> FilteredAdvisers { get; } = new();


        public void UpdateFilteredParents()
        {
            FilteredParents.Clear();
            if (string.IsNullOrWhiteSpace(ParentNameInput))
                return;

            var matches = AllParents.Where(p =>
                (!string.IsNullOrEmpty(p.FirstName) && p.FirstName.Contains(ParentNameInput, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(p.LastName) && p.LastName.Contains(ParentNameInput, StringComparison.OrdinalIgnoreCase)));

            foreach (var parent in matches)
                FilteredParents.Add(parent);
        }

        public void UpdateFilteredAdvisers()
        {
            FilteredAdvisers.Clear();
            if (string.IsNullOrWhiteSpace(AdviserNameInput))
                return;

            var matches = AllAdvisers.Where(a =>
                (!string.IsNullOrEmpty(a.FirstName) && a.FirstName.Contains(AdviserNameInput, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(a.LastName) && a.LastName.Contains(AdviserNameInput, StringComparison.OrdinalIgnoreCase)));

            foreach (var adviser in matches)
                FilteredAdvisers.Add(adviser);
        }


        private void LoadParents()
        {
            using var context = new Context();
            var parentsFromDb = context.Parents.ToList();
            AllParents.Clear();
            foreach (var parent in parentsFromDb)
                AllParents.Add(parent);
        }

        private void LoadAdvisers()
        {
            using var context = new Context();
            var advisersFromDb = context.ClassAdvisers.ToList();
            AllAdvisers.Clear();
            foreach (var adviser in advisersFromDb)
                AllAdvisers.Add(adviser);
        }


    }
}
