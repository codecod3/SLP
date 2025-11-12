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
using ApplicationWPF.Commands;
using ApplicationWPF.View;
using ApplicationWPF.ViewModel;
using Microsoft.EntityFrameworkCore;
using SoftwareDesignQueenAnneCuriosityShopProject;
using SoftwareDesignQueenAnneCuriosityShopProject.Entities;

namespace ApplicationWPF.View
{
    /// <summary>
    /// Interaction logic for HomePageView.xaml
    /// </summary>
    public partial class HomePageView : Window
    {
       

        public HomePageView()
        {
            InitializeComponent();
            HomePageViewModel vm = new HomePageViewModel();
            this.DataContext = vm;
        }
    }
}
