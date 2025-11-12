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
    /// Interaction logic for ParentListWindow.xaml
    /// </summary>
    public partial class ParentListWindow : Window
    {
       
        public ParentListWindow()
        {
            InitializeComponent();
            ParentListViewModel parentListViewModel = new ParentListViewModel();
            this.DataContext = parentListViewModel;
        }

        private void ParentListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(DataContext is ParentListViewModel viewmodel)
            {
                if ((sender as ListView)?.SelectedItem is Parent selectedParent)
                {
                    viewmodel.DoubleClickTrigger?.Execute(selectedParent);

                    var parentWindow = Window.GetWindow(this);
                    parentWindow?.Close();
                }
            }



        }
    }
}
