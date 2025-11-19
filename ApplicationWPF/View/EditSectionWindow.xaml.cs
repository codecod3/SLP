using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SoftwareDesignQueenAnneCuriosityShopProject.Entities;
using ApplicationWPF.ViewModel;

namespace ApplicationWPF.View
{
    public partial class EditSectionWindow : Window
    {
        public EditSectionViewModel ViewModel => DataContext as EditSectionViewModel;

        public EditSectionWindow(Advisory advisory)
        {
            InitializeComponent();
            DataContext = new EditSectionViewModel(advisory);
            ViewModel.RequestClose += () => this.Close();

            // wire events for the auto-filter popup behavior
            AdviserSearchBox.TextChanged += AdviserSearchBox_TextChanged;
            AdviserSearchBox.PreviewKeyDown += AdviserSearchBox_PreviewKeyDown;
        }

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
