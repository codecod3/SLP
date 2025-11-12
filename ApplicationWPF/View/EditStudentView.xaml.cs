using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SoftwareDesignQueenAnneCuriosityShopProject.Entities;
using ApplicationWPF.ViewModel;

namespace ApplicationWPF.View
{
    public partial class EditStudentView : Window
    {
        
        public EditStudentViewModel ViewModel => DataContext as EditStudentViewModel;
        public EditStudentView(Student student)
        {
            InitializeComponent();
            EditStudentViewModel ViewModel = new EditStudentViewModel(student);
            this.DataContext = ViewModel;

            ViewModel.RequestClose += () => this.Close();
        }

        // =========================
        // PARENT AUTOFILTER
        // =========================
        private void ParentSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ParentSuggestionsPopup.IsOpen = ViewModel.FilteredParents.Any();
        }

        private void ParentSuggestionsList_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ParentSuggestionsList.SelectedItem is Parent parent)
            {
                ViewModel.SelectedParent = parent;
                ParentSuggestionsPopup.IsOpen = false;
            }
        }

        private void ParentSearchBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down && ParentSuggestionsPopup.IsOpen)
            {
                ParentSuggestionsList.Focus();
                ParentSuggestionsList.SelectedIndex = 0;
                e.Handled = true;
            }
        }

        // =========================
        // ADVISER AUTOFILTER
        // =========================
        private void AdviserSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            AdviserSuggestionsPopup.IsOpen = ViewModel.FilteredAdvisers.Any();
        }

        private void AdviserSuggestionsList_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (AdviserSuggestionsList.SelectedItem is ClassAdviser adviser)
            {
                ViewModel.SelectedAdviser = adviser;
                AdviserSuggestionsPopup.IsOpen = false;
            }
        }

        private void AdviserSearchBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down && AdviserSuggestionsPopup.IsOpen)
            {
                AdviserSuggestionsList.Focus();
                AdviserSuggestionsList.SelectedIndex = 0;
                e.Handled = true;
            }
        }

       
    }
}
