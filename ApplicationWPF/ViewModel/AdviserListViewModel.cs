using ApplicationWPF.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationWPF.Commands;
using ApplicationWPF.View;
using Microsoft.EntityFrameworkCore;
using SoftwareDesignQueenAnneCuriosityShopProject;
using SoftwareDesignQueenAnneCuriosityShopProject.Entities;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;

namespace ApplicationWPF.ViewModel
{
    public class AdviserListViewModel: NotifyPropertyChanged
    {
        public ObservableCollection<ClassAdviser> ClassAdvisers { get; set;}
        public ClassAdviser SelectedAdviser { get; set; }

        public ICommand AddAdviserCommand { get; set; }
        public ICommand DeleteAdviserCommand { get; set; }
        public ICommand DoubleClickCommand { get; set; }


        public AdviserListViewModel()
        {
            LoadAdvisers();
            AddAdviserCommand = new RelayCommand(AddAdviser, (s) => true);
            DeleteAdviserCommand = new RelayCommand(DeleteAdviser, (s) => true);
            DoubleClickCommand = new RelayCommand(OnDoubleClick, (s) => true);

        }


        private void OnDoubleClick(object obj)
        {

            if (SelectedAdviser != null)
            {
                var editAdviserWindow = new EditAdviserWindow(SelectedAdviser);
                editAdviserWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                editAdviserWindow.Show();
                LoadAdvisers();
            }
        }

        private void AddAdviser(object obj)
        { 
            var addadviserwindow = new AddAdviserWindow();
            addadviserwindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            addadviserwindow.Show();
            var w = obj as Window;
            w.Close();

        }

        private void DeleteAdviser(object obj)
        {
            if (SelectedAdviser == null)
            {
                MessageBox.Show("Please select a adviser to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to delete {SelectedAdviser.FirstName} {SelectedAdviser.LastName}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using var context = new Context();

                    var adviserToDelete = context.ClassAdvisers
                        .Include(p => p.Advisories)
                        .FirstOrDefault(p => p.ClassAdviserID == SelectedAdviser.ClassAdviserID);


                    if (adviserToDelete != null)
                    {
                        var advisories = context.Advisories
                            .Where(c => c.ClassAdviserID == adviserToDelete.ClassAdviserID)
                            .ToList();




                        context.Advisories.RemoveRange(advisories);
                        context.ClassAdvisers.Remove(adviserToDelete);
                        context.SaveChanges();

                        ClassAdvisers.Remove(SelectedAdviser);

                        MessageBox.Show("Adviser deleted successfully.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting Adviser: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void LoadAdvisers()
        {

           using var context = new Context();
            var classAdvisers = context.ClassAdvisers.ToList();
            ClassAdvisers = new ObservableCollection<ClassAdviser>(classAdvisers);
        }


    }
}
