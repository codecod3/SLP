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
    /// Interaction logic for EditAdviserWindow.xaml
    /// </summary>
    public partial class EditAdviserWindow : Window
    {
        public EditAdviserWindow(ClassAdviser adviser)
        {
            InitializeComponent();
            var vm = new EditAdviserViewModel(adviser);
            this.DataContext = vm;

        }
    }
}
