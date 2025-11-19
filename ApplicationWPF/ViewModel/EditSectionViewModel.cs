using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using SoftwareDesignQueenAnneCuriosityShopProject;
using SoftwareDesignQueenAnneCuriosityShopProject.Entities;
using ApplicationWPF.Commands; // RelayCommand or your command implementation

namespace ApplicationWPF.ViewModel
{
    public class EditSectionViewModel : NotifyPropertyChanged
    {
        public Advisory SelectedSection { get; set; }

        // advisers
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
                // reflect adviser into the SelectedSection so UI shows it
                if (SelectedSection != null && _selectedAdviser != null)
                {
                    SelectedSection.ClassAdviserLink = _selectedAdviser;
                    SelectedSection.ClassAdviserID = _selectedAdviser.ClassAdviserID;
                }
            }
        }

        public string SelectedAdviserDisplay =>
            SelectedAdviser != null ? $"{SelectedAdviser.FirstName} {SelectedAdviser.LastName} (ID:{SelectedAdviser.ClassAdviserID})"
                                    : (SelectedSection?.ClassAdviserLink != null
                                        ? $"{SelectedSection.ClassAdviserLink.FirstName} {SelectedSection.ClassAdviserLink.LastName}"
                                        : "— none —");

        private string _adviserNameInput;
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

        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public event Action RequestClose;

        public EditSectionViewModel(Advisory advisory)
        {
            SelectedSection = advisory ?? throw new ArgumentNullException(nameof(advisory));

            LoadAdvisers();
            // if advisory already has an adviser (linked), set SelectedAdviser so UI shows it:
            if (SelectedSection.ClassAdviserLink != null)
            {
                // if the ClassAdviserLink does not have all fields (detached), try to get from DB:
                using var ctx = new Context();
                var adv = ctx.ClassAdvisers.FirstOrDefault(a => a.ClassAdviserID == SelectedSection.ClassAdviserLink.ClassAdviserID)
                          ?? SelectedSection.ClassAdviserLink;
                SelectedAdviser = adv;
                // set input text
                AdviserNameInput = SelectedAdviser != null ? $"{SelectedAdviser.FirstName} {SelectedAdviser.LastName}" : string.Empty;
            }

            SaveCommand = new RelayCommand(Save, _ => true);
            CancelCommand = new RelayCommand(Cancel, _ => true);
        }

        private void LoadAdvisers()
        {
            using var context = new Context();
            var list = context.ClassAdvisers.ToList();
            AllAdvisers.Clear();
            foreach (var a in list) AllAdvisers.Add(a);
            UpdateFilteredAdvisers();
        }

        public void UpdateFilteredAdvisers()
        {
            FilteredAdvisers.Clear();
            if (string.IsNullOrWhiteSpace(AdviserNameInput)) return;

            var matches = AllAdvisers.Where(a =>
                (!string.IsNullOrEmpty(a.FirstName) && a.FirstName.Contains(AdviserNameInput, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(a.LastName) && a.LastName.Contains(AdviserNameInput, StringComparison.OrdinalIgnoreCase)));

            foreach (var m in matches) FilteredAdvisers.Add(m);
        }

        private void Save(object obj)
        {
            try
            {
                using var context = new Context();

                // get advisory in tracked context
                var advisoryInDb = context.Advisories
                    .FirstOrDefault(a => a.AdvisoryID == SelectedSection.AdvisoryID);

                if (advisoryInDb == null)
                {
                    MessageBox.Show("Advisory not found in database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // update fields
                advisoryInDb.SectionName = SelectedSection.SectionName;
                advisoryInDb.SchoolYear = SelectedSection.SchoolYear;
                advisoryInDb.Name = SelectedSection.Name ?? $"{SelectedSection.SectionName}";

                if (SelectedAdviser != null)
                {
                    // attach or find adviser
                    var adviserInDb = context.ClassAdvisers.FirstOrDefault(a => a.ClassAdviserID == SelectedAdviser.ClassAdviserID);
                    if (adviserInDb == null)
                    {
                        // it's unlikely but attach
                        context.ClassAdvisers.Attach(SelectedAdviser);
                        advisoryInDb.ClassAdviserID = SelectedAdviser.ClassAdviserID;
                    }
                    else
                    {
                        advisoryInDb.ClassAdviserID = adviserInDb.ClassAdviserID;
                    }
                }
                else
                {
                    advisoryInDb.ClassAdviserID = null;
                }

                context.SaveChanges();
                MessageBox.Show("Section updated.", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                RequestClose?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel(object obj)
        {
            RequestClose?.Invoke();
        }
    }
}
