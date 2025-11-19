using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ApplicationWPF.ViewModel;
using SoftwareDesignQueenAnneCuriosityShopProject.Entities;

namespace ApplicationWPF.View
{
    public partial class AddSectionView : Window
    {
        public AddSectionViewModel ViewModel => DataContext as AddSectionViewModel;

        public AddSectionView()
        {
            InitializeComponent();
            var vm = new AddSectionViewModel();
            DataContext = vm;

            // Events for popup behavior
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
