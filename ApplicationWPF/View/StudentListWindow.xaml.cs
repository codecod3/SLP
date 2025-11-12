using ApplicationWPF.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SoftwareDesignQueenAnneCuriosityShopProject.Entities;

namespace ApplicationWPF.View
{
    /// <summary>
    /// Interaction logic for StudentListWindow.xaml
    /// </summary>
    public partial class StudentListWindow : Window
    {
        public StudentListWindow()
        {
            InitializeComponent();
            StudentListViewModel model = new StudentListViewModel();
            this.DataContext = model;
        }

        private void StudentListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is StudentListViewModel viewModel)
            {
                if ((sender as ListView)?.SelectedItem is Student selectedStudent)
                {
                    viewModel.DoubleClickTrigger?.Execute(selectedStudent);

                    var parentWindow = Window.GetWindow(this);
                    parentWindow?.Close();
                }
            }
        }
    }
}

