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
using ApplicationWPF.ViewModel;

namespace ApplicationWPF.View
{
    /// <summary>
    /// Interaction logic for SectionDetailView.xaml
    /// </summary>
    public partial class SectionDetailView : Window
    {
        public SectionDetailView(Advisory section)
        {
            InitializeComponent();
            var model = new SectionDetailViewModel(section);
            this.DataContext = model;
        }


        private void StudentList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox?.SelectedItem is Student selectedStudent)
            {
                // Open a new window for the selected student
                var studentWindow = new EditStudentView(selectedStudent);
                studentWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                studentWindow.Show();

                var parentWindow = Window.GetWindow(this);
                parentWindow?.Close();



            }
        }
    }
}
