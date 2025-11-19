using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using SoftwareDesignQueenAnneCuriosityShopProject.Entities;
using ApplicationWPF.Commands;
using System.Windows.Input;
using ApplicationWPF.View;
using System.Windows;

namespace ApplicationWPF.ViewModel
{
    public class SectionDetailViewModel : NotifyPropertyChanged
    {
       

        private Advisory _section;
        public Advisory Section
        {
            get => _section;
            set
            {
                _section = value;
                OnPropertyChanged(nameof(Section));
                LoadSectionDetails();
            }
        }

        public ObservableCollection<Student> Students { get; set; } = new();
        private Student _selectedStudent;
        public Student SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                OnPropertyChanged(nameof(SelectedStudent));
                OnPropertyChanged(nameof(SelectedStudentFullName));
                OnPropertyChanged(nameof(SelectedStudentPicture));
            }
        }

        public string AdviserFullName => Section?.ClassAdviserLink != null
            ? $"{Section.ClassAdviserLink.FirstName} {Section.ClassAdviserLink.LastName}"
            : "No Adviser";

        public string SelectedStudentFullName => SelectedStudent != null
            ? $"{SelectedStudent.FirstName} {SelectedStudent.LastName}"
            : "Select a student";

        // Placeholder image path or can be dynamic
        public string SelectedStudentPicture => "pack://application:,,,/Resources/placeholder.png";

        private void LoadSectionDetails()
        {
            if (Section == null) return;

            Students.Clear();

            // Ensure adviser comes first
            var sortedStudents = Section.Students.OrderBy(s => s.FirstName).ToList();
            foreach (var student in sortedStudents)
            {
                Students.Add(student);
            }

            if (Students.Any())
                SelectedStudent = Students.First();
        }



        public ICommand BackCommand { get; set; }
        public SectionDetailViewModel(Advisory ad)
        {
            LoadSectionDetails();
            BackCommand = new RelayCommand(Back, (s) => true);
            Section = ad;
        }


        private void Back(object obj)
        {

            var window = new SectionView();
            window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            window.Show();


            var w = obj as Window;
            w.Close();



        }
    }
}
