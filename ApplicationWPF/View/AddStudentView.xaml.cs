using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ApplicationWPF;
using ApplicationWPF.ViewModel;
using SoftwareDesignQueenAnneCuriosityShopProject.Entities;

namespace ApplicationWPF.View
{
    /// <summary>
    /// Interaction logic for AddStudentView.xaml
    /// </summary>
    public partial class AddStudentView : Window
    {
       
        public AddStudentViewModel ViewModel => DataContext as AddStudentViewModel;
        public AddStudentView()
        {
            InitializeComponent();
           AddStudentViewModel ViewModel = new AddStudentViewModel();
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
