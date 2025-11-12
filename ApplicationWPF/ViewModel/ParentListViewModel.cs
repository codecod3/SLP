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
    public class ParentListViewModel: NotifyPropertyChanged
    {
        public ObservableCollection<Parent> Parents { get; set; }
        public Parent SelectedParent { get; set; }

        public ICommand AddParentCommand { get; set; }
        public ICommand DeletedParentCommand { get; set; }
        public ICommand DoubleClickTrigger { get; set; }

        public ParentListViewModel()
        {
            LoadParents();
            AddParentCommand = new RelayCommand(AddParent, (s) => true);
            DeletedParentCommand = new RelayCommand(DeletedParent, (s) => true);
            DoubleClickTrigger = new RelayCommand(OnDoubleClickTrigger, (s) => true);

        }

        private void AddParent(object obj)
        {
            var addParent = new AddParentWindow();
            addParent.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            addParent.Show();

            var w = obj as Window;
            w.Close();

        }

        private void DeletedParent(object obj)
        {
            if (SelectedParent == null)
            {
                MessageBox.Show("Please select a parent to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to delete {SelectedParent.FirstName} {SelectedParent.LastName}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using var context = new Context();

                    var parentToDelete = context.Parents
                        .Include(p => p.Contacts).Include(p=>p.Relationships)
                        .FirstOrDefault(p => p.ParentID == SelectedParent.ParentID);
                    

                    if (parentToDelete != null)
                    {
                        var relatedContacts = context.Contacts
                            .Where(c => c.ParentID == parentToDelete.ParentID)
                            .ToList();

                        var relatedRelationships = context.Relationships.Where(c=>c.ParentID == parentToDelete.ParentID).ToList();

                        context.Relationships.RemoveRange(relatedRelationships);
                        context.Contacts.RemoveRange(relatedContacts);
                        context.Parents.Remove(parentToDelete);
                        context.SaveChanges();

                        Parents.Remove(SelectedParent);

                        MessageBox.Show("Parent deleted successfully.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting parent: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void OnDoubleClickTrigger(object obj)
        {
            if(SelectedParent != null)
            {
                var editParent = new EditParentWindow(SelectedParent);
                editParent.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                editParent.Show();
                
            }
        }

        public void LoadParents()
        {
            using var context = new Context();
            var parents = context.Parents.ToList();
            Parents = new ObservableCollection<Parent>(parents);
        }
    }
}
