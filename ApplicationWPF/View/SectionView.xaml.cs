using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ApplicationWPF.ViewModel;
using SoftwareDesignQueenAnneCuriosityShopProject.Entities;

namespace ApplicationWPF.View
{
    public partial class SectionView : Window
    {
        public SectionView()
        {
            InitializeComponent();
            var vm = new SectionViewModel();
            this.DataContext = vm;
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dg && dg.SelectedItem is Advisory selectedAdvisory)
            {
                var detailWindow = new SectionDetailView(selectedAdvisory); // your detail window
                detailWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                detailWindow.Show();

                var parentWindow = Window.GetWindow(this);
                parentWindow?.Close();
            }
        }
    }
}
