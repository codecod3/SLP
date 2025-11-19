using ApplicationWPF.Commands;
using ApplicationWPF.View;
using SoftwareDesignQueenAnneCuriosityShopProject;
using SoftwareDesignQueenAnneCuriosityShopProject.Entities;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ApplicationWPF.ViewModel
{
    public class AddSectionViewModel : NotifyPropertyChanged
    {
        public string SectionName { get; set; }
        public string SchoolYear { get; set; }

        private string adviserNameInput;
        public string AdviserNameInput
        {
            get => adviserNameInput;
            set
            {
                adviserNameInput = value;
                OnPropertyChanged(nameof(AdviserNameInput));
                UpdateFilteredAdvisers();
            }
        }

        public ObservableCollection<ClassAdviser> AllAdvisers { get; } = new ObservableCollection<ClassAdviser>();
        public ObservableCollection<ClassAdviser> FilteredAdvisers { get; } = new ObservableCollection<ClassAdviser>();

        private ClassAdviser _selectedAdviser;
        public ClassAdviser SelectedAdviser
        {
            get => _selectedAdviser;
            set
            {
                _selectedAdviser = value;
                OnPropertyChanged(nameof(SelectedAdviser));
                OnPropertyChanged(nameof(SelectedAdviserDisplay));
            }
        }

        public string SelectedAdviserDisplay => SelectedAdviser != null
            ? $"{SelectedAdviser.FirstName} {SelectedAdviser.LastName}"
            : "";

        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public AddSectionViewModel()
        {
            LoadAllAdvisers();
            SaveCommand = new RelayCommand(Save, _ => true);
            CancelCommand = new RelayCommand(Cancel, _ => true);
        }

        private void LoadAllAdvisers()
        {
            using var db = new Context();
            AllAdvisers.Clear();
            foreach (var adv in db.ClassAdvisers.ToList())
                AllAdvisers.Add(adv);

            UpdateFilteredAdvisers();
        }

        public void UpdateFilteredAdvisers()
        {
            FilteredAdvisers.Clear();
            if (string.IsNullOrWhiteSpace(AdviserNameInput)) return;

            var matches = AllAdvisers.Where(a =>
                $"{a.FirstName} {a.LastName}".ToLower().Contains(AdviserNameInput.ToLower())
            );

            foreach (var adv in matches) FilteredAdvisers.Add(adv);
        }

        private void Save(object obj)
        {
            using var db = new Context();
            var newSection = new Advisory
            {
                SectionName = SectionName,
                SchoolYear = SchoolYear,
                ClassAdviserID = SelectedAdviser?.ClassAdviserID,
                Name = SectionName // <-- set Name so NOT NULL constraint is satisfied
            };
            db.Advisories.Add(newSection);
            db.SaveChanges();


            var window = new SectionView();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Show();

            if (obj is Window w) w.Close();

            MessageBox.Show("Section added.", "Added", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void Cancel(object obj)
        {
            if (obj is Window w) w.Close();
        }
    }
}
