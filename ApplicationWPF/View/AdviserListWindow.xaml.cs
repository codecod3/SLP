using ApplicationWPF.ViewModel;
using SoftwareDesignQueenAnneCuriosityShopProject.Entities;
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

namespace ApplicationWPF.View
{
    /// <summary>
    /// Interaction logic for AdviserListWindow.xaml
    /// </summary>
    public partial class AdviserListWindow : Window
    {
        public AdviserListWindow()
        {
            InitializeComponent();
            var vm = new AdviserListViewModel();
            this.DataContext = vm;
        }

        private void AdviserListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is AdviserListViewModel viewModel)
            {
                if ((sender as ListView)?.SelectedItem is ClassAdviser selectedAdviser)
                {
                    viewModel.DoubleClickCommand?.Execute(selectedAdviser);

                    var parentWindow = Window.GetWindow(this);
                    parentWindow?.Close();
                }
            }
        }
    }
}
