using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using ApplicationWPF.Commands;
using ApplicationWPF.View;
using SoftwareDesignQueenAnneCuriosityShopProject;
using SoftwareDesignQueenAnneCuriosityShopProject.Entities;
using System.Windows;


namespace ApplicationWPF.ViewModel
{
    public class SectionViewModel : NotifyPropertyChanged
    {
        // Observable collection bound to the DataGrid
        public ObservableCollection<Advisory> Sections { get; set; } = new();

        private Advisory _selectedAdvisory;
        public Advisory SelectedAdvisory
        {
            get => _selectedAdvisory;
            set
            {
                _selectedAdvisory = value;
                OnPropertyChanged();
                // Notify CanExecute changed for commands if needed
            }
        }

        public ICommand AddSectionCommand { get; set; }
     
        public ICommand DeleteSectionCommand { get; set; }

        public ICommand EditSectionCommand { get; set; }
        public ICommand BackCommand { get; set; }

        public SectionViewModel()
        {
            LoadSections();

            AddSectionCommand = new RelayCommand(AddSection, (s) => true);

            EditSectionCommand = new RelayCommand(EditSection, (s) => true);
            BackCommand = new RelayCommand(Back, (s) => true);
           
            DeleteSectionCommand = new RelayCommand(DeleteSection, (s) => true);
        }


        private void Back(object obj)
        {

            var window = new HomePageView();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Show();
            var w = obj as Window;
            w.Close();

        }


        private void AddSection(object obj)
        {
            var window = new AddSectionView();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Show();

            var w = obj as Window;
            w.Close();

        }


        private void EditSection(object obj)
        {

            var window = new EditSectionWindow(SelectedAdvisory);
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Show();




        }

        private void LoadSections()
        {
            using var context = new Context();
            var sections = context.Advisories
                                  .Include(a => a.ClassAdviserLink)
                                  .Include(a => a.Students)
                                  .Where(a => !string.IsNullOrWhiteSpace(a.SectionName)) // ignore empty names
                                  .ToList();

            Sections.Clear();
            foreach (var s in sections)
                Sections.Add(s);

            OnPropertyChanged(nameof(Sections));
        }

        private void DeleteSection(object obj)
        {
            if (SelectedAdvisory == null) return;

            var result = System.Windows.MessageBox.Show(
                $"Are you sure you want to delete section '{SelectedAdvisory.SectionName}'?",
                "Confirm Delete",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Question);

            if (result != System.Windows.MessageBoxResult.Yes) return;

            using var context = new Context();
            var sectionToDelete = context.Advisories
                                         .Include(a => a.Students)
                                         .FirstOrDefault(a => a.AdvisoryID == SelectedAdvisory.AdvisoryID);

            if (sectionToDelete != null)
            {
                // Clear students first if necessary
                sectionToDelete.Students.Clear();
                context.Advisories.Remove(sectionToDelete);
                context.SaveChanges();

                // Reload sections to update UI
                LoadSections();
            }
        }

       

       
    }
}
