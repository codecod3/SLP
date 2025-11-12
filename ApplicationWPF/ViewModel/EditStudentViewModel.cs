using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using ApplicationWPF.Commands;
using SoftwareDesignQueenAnneCuriosityShopProject.Entities;
using SoftwareDesignQueenAnneCuriosityShopProject;
using ApplicationWPF.View;

namespace ApplicationWPF.ViewModel
{
    public class EditStudentViewModel : NotifyPropertyChanged
    {
        private Student _selectedStudent;
        private Parent _selectedParent;
        private ClassAdviser _selectedAdviser;
        private Relationship _selectedRelationship;
        private Advisory _selectedAdvisory;

        private string _parentNameInput;
        private string _adviserNameInput;
        private RelationshipType _selectedRelationshipType; // flattened property

        // Enum array for ComboBox
        public Array RelationshipTypes => Enum.GetValues(typeof(RelationshipType));

        // PROPERTIES
        public Student SelectedStudent
        {
            get => _selectedStudent;
            set { _selectedStudent = value; OnPropertyChanged(); }
        }

        public Parent SelectedParent
        {
            get => _selectedParent;
            set
            {
                if (_selectedParent != value)
                {
                    _selectedParent = value;
                    OnPropertyChanged();
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
                    OnPropertyChanged();
                    if (_selectedAdviser != null)
                        AdviserNameInput = $"{_selectedAdviser.FirstName} {_selectedAdviser.LastName}";
                }
            }
        }

        public Relationship SelectedRelationship
        {
            get => _selectedRelationship;
            set
            {
                if (_selectedRelationship != value)
                {
                    _selectedRelationship = value;
                    OnPropertyChanged();
                }
            }
        }

        public RelationshipType SelectedRelationshipType
        {
            get => _selectedRelationshipType;
            set
            {
                if (_selectedRelationshipType != value)
                {
                    _selectedRelationshipType = value;
                    OnPropertyChanged();

                    if (SelectedRelationship != null)
                        SelectedRelationship.TypeOfRelationship = value;
                }
            }
        }

        public Advisory SelectedAdvisory
        {
            get => _selectedAdvisory;
            set { _selectedAdvisory = value; OnPropertyChanged(); }
        }

        public string ParentNameInput
        {
            get => _parentNameInput;
            set
            {
                if (_parentNameInput != value)
                {
                    _parentNameInput = value;
                    OnPropertyChanged();
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
                    OnPropertyChanged();
                    UpdateFilteredAdvisers();
                }
            }
        }

        // COLLECTIONS
        public ObservableCollection<Parent> AllParents { get; } = new();
        public ObservableCollection<Parent> FilteredParents { get; } = new();
        public ObservableCollection<ClassAdviser> AllAdvisers { get; } = new();
        public ObservableCollection<ClassAdviser> FilteredAdvisers { get; } = new();

        // COMMANDS
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public event Action RequestClose;

        // CONSTRUCTOR
        public EditStudentViewModel(Student student)
        {
            SelectedStudent = student ?? new Student();

            LoadParents();
            LoadAdvisers();
            LoadExistingLinks();

            SaveCommand = new RelayCommand(SaveChanges, _ => true);
            CancelCommand = new RelayCommand(Cancel, _ => true);
        }

        // LOAD DATA
        private void LoadParents()
        {
            using var context = new Context();
            AllParents.Clear();
            foreach (var p in context.Parents.ToList())
                AllParents.Add(p);
        }

        private void LoadAdvisers()
        {
            using var context = new Context();
            AllAdvisers.Clear();
            foreach (var a in context.ClassAdvisers.ToList())
                AllAdvisers.Add(a);
        }

        private void LoadExistingLinks()
        {
            using var context = new Context();

            // --- RELATIONSHIP ---
            var relationship = context.Relationships
                .Include(r => r.ParentLink)
                .FirstOrDefault(r => r.StudentID == SelectedStudent.StudentID);

            if (relationship != null)
            {
                SelectedRelationship = relationship;
                SelectedParent = relationship.ParentLink;
                SelectedRelationshipType = relationship.TypeOfRelationship; // flattened property for ComboBox
            }
            else
            {
                SelectedRelationship = new Relationship();
                SelectedRelationshipType = RelationshipType.Other;
            }

            // --- ADVISORY ---
            var student = context.Students
                .Include(s => s.AdvisoryLink)
                .ThenInclude(a => a.ClassAdviserLink)
                .FirstOrDefault(s => s.StudentID == SelectedStudent.StudentID);

            if (student?.AdvisoryLink != null)
            {
                SelectedAdvisory = student.AdvisoryLink;
                SelectedAdviser = student.AdvisoryLink.ClassAdviserLink;
            }
        }

        // FILTERING
        private void UpdateFilteredParents()
        {
            FilteredParents.Clear();
            if (string.IsNullOrWhiteSpace(ParentNameInput)) return;

            var matches = AllParents.Where(p =>
                (!string.IsNullOrEmpty(p.FirstName) && p.FirstName.Contains(ParentNameInput, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(p.LastName) && p.LastName.Contains(ParentNameInput, StringComparison.OrdinalIgnoreCase)));

            foreach (var parent in matches)
                FilteredParents.Add(parent);
        }

        private void UpdateFilteredAdvisers()
        {
            FilteredAdvisers.Clear();
            if (string.IsNullOrWhiteSpace(AdviserNameInput)) return;

            var matches = AllAdvisers.Where(a =>
                (!string.IsNullOrEmpty(a.FirstName) && a.FirstName.Contains(AdviserNameInput, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(a.LastName) && a.LastName.Contains(AdviserNameInput, StringComparison.OrdinalIgnoreCase)));

            foreach (var adviser in matches)
                FilteredAdvisers.Add(adviser);
        }

        // SAVE
        private void SaveChanges(object obj)
        {
            using var context = new Context();
            var student = context.Students.FirstOrDefault(s => s.StudentID == SelectedStudent.StudentID);
            if (student == null) return;

            // --- BASIC INFO ---
            student.FirstName = SelectedStudent.FirstName;
            student.LastName = SelectedStudent.LastName;
            student.LRN = SelectedStudent.LRN;
            student.EnrollmentStatus = SelectedStudent.EnrollmentStatus;

            // --- RELATIONSHIP ---
            var existingRel = context.Relationships.FirstOrDefault(r => r.StudentID == student.StudentID);

            if (SelectedParent == null)
            {
                if (existingRel != null)
                    context.Relationships.Remove(existingRel);
            }
            else
            {
                if (existingRel != null)
                {
                    existingRel.ParentID = SelectedParent.ParentID;
                    existingRel.TypeOfRelationship = SelectedRelationshipType;
                }
                else
                {
                    context.Relationships.Add(new Relationship
                    {
                        StudentID = student.StudentID,
                        ParentID = SelectedParent.ParentID,
                        TypeOfRelationship = SelectedRelationshipType
                    });
                }
            }

            // --- ADVISORY ---
            if (SelectedAdviser == null)
            {
                student.AdvisoryID = null;
            }
            else
            {
                string sectionName = SelectedAdvisory?.SectionName ?? string.Empty;
                string schoolYear = SelectedAdvisory?.SchoolYear ?? DateTime.Now.Year.ToString();

                var advisory = context.Advisories
                    .Include(a => a.ClassAdviserLink)
                    .FirstOrDefault(a =>
                        a.ClassAdviserID == SelectedAdviser.ClassAdviserID &&
                        a.SectionName == sectionName &&
                        a.SchoolYear == schoolYear);

                if (advisory == null)
                {
                    advisory = new Advisory
                    {
                        Name = $"{SelectedAdviser.FirstName} {SelectedAdviser.LastName} - {sectionName}",
                        SectionName = sectionName,
                        SchoolYear = schoolYear,
                        ClassAdviserID = SelectedAdviser.ClassAdviserID
                    };
                    context.Advisories.Add(advisory);
                    context.SaveChanges();
                }

                student.AdvisoryID = advisory.AdvisoryID;
            }

            context.SaveChanges();
            MessageBox.Show("Student successfully updated!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            var x = new StudentListWindow();
            x.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            x.Show();
            RequestClose?.Invoke();
        }

        // CANCEL
        private void Cancel(object obj)
        {
            var x = new StudentListWindow();
            x.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            x.Show();
            RequestClose?.Invoke();

        }
    }
}
