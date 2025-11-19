using ApplicationWPF.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationWPF.View;
using Microsoft.EntityFrameworkCore;
using SoftwareDesignQueenAnneCuriosityShopProject;
using SoftwareDesignQueenAnneCuriosityShopProject.Entities;
using System.Windows.Input;
using System.Windows;

namespace ApplicationWPF.ViewModel
{
    public class HomePageViewModel: NotifyPropertyChanged
    {
        public ICommand OpenStudentListCommand { get; set; }
        public ICommand OpenAdviserListCommand { get; set; }
        public ICommand OpenParentListCommand { get; set; }
        public ICommand OpenSectionListCommand { get; set; }
        public ICommand LogOutCommand { get; set; }



        public HomePageViewModel()
        {

            OpenStudentListCommand = new RelayCommand(OpenStudentList, (s) => true);
            OpenAdviserListCommand = new RelayCommand(OpenAdviserList, (s)=> true);
            OpenParentListCommand = new RelayCommand(OpenParentList, (s)=> true);
            OpenSectionListCommand = new RelayCommand(OpenSectionList, (s) => true);
            LogOutCommand = new RelayCommand(LogOut, (s) => true);

        }

        private void LogOut(object obj)
        {

            var window = new MainWindow();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Show();

            var w = obj as Window;
            w.Close();



        }

        private void OpenSectionList(object obj)
        {

            var sectionView = new SectionView();
            sectionView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            sectionView.Show();

            var w = obj as Window;
            w.Close();
        }


        private void OpenStudentList(object obj)
        {

            var studentList = new StudentListWindow();
            studentList.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            studentList.Show();

            var w = obj as Window;
            w.Close();


        }


        private void OpenAdviserList(object obj)
        {
            var adviserList = new AdviserListWindow();
            adviserList.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            adviserList.Show();

            var w = obj as Window;
            w.Close();


        }

        private void OpenParentList(object obj)
        {
            var parentList = new ParentListWindow();
            parentList.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            parentList.Show();

            var w = obj as Window;
            w.Close();
        }
    }
}
