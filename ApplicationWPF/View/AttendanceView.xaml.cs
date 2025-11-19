using ApplicationWPF.ViewModel;
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
using SoftwareDesignQueenAnneCuriosityShopProject.Entities;

namespace ApplicationWPF.View
{
    /// <summary>
    /// Interaction logic for AttendanceView.xaml
    /// </summary>
    public partial class AttendanceView : Window
    {
        public AttendanceView(Student stud)
        {
            InitializeComponent();
            var vm = new AttendanceViewModel(stud);
            this.DataContext = vm;
        }
    }
}
