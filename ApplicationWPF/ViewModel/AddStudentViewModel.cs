using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ApplicationWPF.Commands;
using ApplicationWPF.View;
using Microsoft.EntityFrameworkCore;
using SoftwareDesignQueenAnneCuriosityShopProject;
using SoftwareDesignQueenAnneCuriosityShopProject.Entities;

namespace ApplicationWPF.ViewModel
{
    public class AddStudentViewModel : NotifyPropertyChanged
    {
        // Fields
        private string _firstName;
        private string _lastName;
        private int _lrn;
        private string _phoneNumber;
        private Parent _selectedParent;
        private ClassAdviser _selectedAdviser;
        private string _sectionName;
        private string _parentNameInput;
        private string _adviserNameInput;
        private EnrollmentStatus _selectedEnrollmentStatus;

        // Events
        public event Action RequestClose;

        // Properties
        public Array RelationshipTypes => Enum.GetValues(typeof(RelationshipType));
        public Array EnrollmentTypes => Enum.GetValues(typeof(EnrollmentStatus));

        public Relationship SelectedRelationship { get; set; } = new Relationship();

        public EnrollmentStatus SelectedEnrollmentStatus
        {
            get => _selectedEnrollmentStatus;
            set
            {
                _selectedEnrollmentStatus = value;
                OnPropertyChanged(nameof(SelectedEnrollmentStatus));
            }
        }

        public string SectionName
        {
            get => _sectionName;
            set
            {
                _sectionName = value;
                OnPropertyChanged();
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
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

        // Collections for autocomplete
        public ObservableCollection<Parent> AllParents { get; } = new();
        public ObservableCollection<Parent> FilteredParents { get; } = new();
        public ObservableCollection<ClassAdviser> AllAdvisers { get; } = new();
        public ObservableCollection<ClassAdviser> FilteredAdvisers { get; } = new();

        // Commands
        public ICommand AddStudentCommand { get; set; }

        // Constructor
        public AddStudentViewModel()
        {
            AddStudentCommand = new RelayCommand(AddStudent, (s) => true);
            LoadParents();
            LoadAdvisers();
        }

        // Add Student Method
        private void AddStudent(object obj)
        {
            using var context = new Context();

            var newStudent = new Student
            {
                FirstName = FirstName,
                LastName = LastName,
                LRN = LRN,
                PhoneNumber = PhoneNumber,
                EnrollmentStatus = SelectedEnrollmentStatus
            };

            // Link Parent
            if (SelectedParent != null)
            {
                if (!context.Parents.Local.Any(p => p.ParentID == SelectedParent.ParentID))
                    context.Parents.Attach(SelectedParent);

                var relation = new Relationship
                {
                    StudentLink = newStudent,
                    ParentLink = SelectedParent,
                    TypeOfRelationship = SelectedRelationship.TypeOfRelationship
                };
                context.Relationships.Add(relation);
            }

            // Link Adviser and Advisory
            if (SelectedAdviser != null)
            {
                if (!context.ClassAdvisers.Local.Any(a => a.ClassAdviserID == SelectedAdviser.ClassAdviserID))
                    context.ClassAdvisers.Attach(SelectedAdviser);

                // Force client evaluation for case-insensitive section comparison
                var existingAdvisory = context.Advisories
                    .Include(a => a.Students)
                    .AsEnumerable()
                    .FirstOrDefault(a =>
                        a.ClassAdviserID == SelectedAdviser.ClassAdviserID &&
                        string.Equals(a.SectionName, SectionName, StringComparison.OrdinalIgnoreCase));

                if (existingAdvisory != null)
                {
                    existingAdvisory.Students.Add(newStudent);
                    newStudent.AdvisoryLink = existingAdvisory;
                }
                else
                {
                    var newAdvisory = new Advisory
                    {
                        Name = SectionName,
                        SectionName = SectionName,
                        ClassAdviserLink = SelectedAdviser,
                        Students = new List<Student> { newStudent }
                    };

                    context.Advisories.Add(newAdvisory);
                    newStudent.AdvisoryLink = newAdvisory;
                }
            }

            // Add student
            context.Students.Add(newStudent);
            context.SaveChanges();

            MessageBox.Show("Student successfully added!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Open student list window
            var studentListWindow = new StudentListWindow
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            studentListWindow.Show();

            if (obj is Window w)
                w.Close();
        }

        // Filtering Parents
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

        // Filtering Advisers
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

        // Load Parents
        private void LoadParents()
        {
            using var context = new Context();
            AllParents.Clear();
            foreach (var parent in context.Parents.ToList())
                AllParents.Add(parent);
        }

        // Load Advisers
        private void LoadAdvisers()
        {
            using var context = new Context();
            AllAdvisers.Clear();
            foreach (var adviser in context.ClassAdvisers.ToList())
                AllAdvisers.Add(adviser);
        }
    }
}
