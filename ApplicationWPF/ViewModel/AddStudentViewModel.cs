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
        public Array RelationshipTypes => Enum.GetValues(typeof(RelationshipType));
        public Array EnromentTypes => Enum.GetValues(typeof(EnrollmentStatus));

        public Relationship SelectedRelationship { get; set; } = new Relationship();
        private EnrollmentStatus _selectedEnrollmentStatus;

        public ICommand AddStudentCommand { get; set; }
        private string _sectionName;

        private string _parentNameInput;
        private string _adviserNameInput;
        private string _schoolYear;


        public EnrollmentStatus SelectedEnrollmentStatus
        {
            get => _selectedEnrollmentStatus;
            set
            {
                _selectedEnrollmentStatus = value;
                OnPropertyChanged(nameof(SelectedEnrollmentStatus));
            }
        }

        public string SchoolYear
        {
            get=>_schoolYear;
            set
            {
                _schoolYear = value;
                OnPropertyChanged(nameof(SchoolYear));
            }
        }


        public string SectionName
        {
            get=>_sectionName;
            set
            {
                _sectionName = value;
                OnPropertyChanged();
            }
        }

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

            // Create new student
            var newStudent = new Student
            {
                FirstName = FirstName,
                LastName = LastName,
                LRN = LRN,
                PhoneNumber = PhoneNumber,
                EnrollmentStatus = SelectedEnrollmentStatus
            };

            // 🔹 Link Parent if selected
            if (SelectedParent != null)
            {
                // Attach parent to context if not already tracked
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

            // 🔹 Link Adviser and Advisory if selected
            if (SelectedAdviser != null)
            {
                // Attach adviser to context if not already tracked
                if (!context.ClassAdvisers.Local.Any(a => a.ClassAdviserID == SelectedAdviser.ClassAdviserID))
                    context.ClassAdvisers.Attach(SelectedAdviser);

                // Try to find an existing advisory with same adviser and section
                var existingAdvisory = context.Advisories
                    .Include(a => a.Students)
                    .FirstOrDefault(a =>
                        a.ClassAdviserID == SelectedAdviser.ClassAdviserID &&
                        a.SectionName == SectionName && a.SchoolYear == SchoolYear);

                if (existingAdvisory != null)
                {
                    // Advisory exists -> just add the student
                    existingAdvisory.Students.Add(newStudent);
                    newStudent.AdvisoryLink = existingAdvisory;
                }
                else
                {
                    // Create a new advisory
                    var newAdvisory = new Advisory
                    {
                        Name = $"{SectionName} - {SchoolYear}",
                        SectionName = SectionName,
                        SchoolYear = SchoolYear,
                        ClassAdviserLink = SelectedAdviser, // attached entity
                        Students = new List<Student> { newStudent }
                    };

                    context.Advisories.Add(newAdvisory);
                    newStudent.AdvisoryLink = newAdvisory;
                }
            }

            // 🔹 Add student
            context.Students.Add(newStudent);

            // 🔹 Save changes
            context.SaveChanges();

            MessageBox.Show("Student successfully added!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // 🔹 Open student list window
            var s = new StudentListWindow
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            s.Show();

            if (obj is Window w)
                w.Close();
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
