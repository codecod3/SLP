using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ApplicationWPF.Commands;
using ApplicationWPF.View;
using Microsoft.EntityFrameworkCore;
using SoftwareDesignQueenAnneCuriosityShopProject;
using SoftwareDesignQueenAnneCuriosityShopProject.Entities;
using WindowStartupLocation = System.Windows.WindowStartupLocation;

namespace ApplicationWPF.ViewModel
{
    internal class StudentListViewModel: NotifyPropertyChanged
    {
        
        public ObservableCollection<Student> Students { get; set; }
      
        public Student SelectedStudent { get; set; }
        public ICommand AddStudentCommand { get; set; }
        public ICommand EditStudentCommand { get; set; }
        public ICommand DeleteStudentCommand { get; set; }
        public ICommand DoubleClickTrigger { get; set; }

        public StudentListViewModel()
        {
            LoadStudents();
            DoubleClickTrigger = new RelayCommand(OnDoubleClick, (s) => true);
            AddStudentCommand = new RelayCommand(OnAddStudent, (s) => true);
            DeleteStudentCommand = new RelayCommand(DeleteCommand, (s) => true);
        }

        private void DeleteCommand(object obj)
        {

            if (SelectedStudent == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete {SelectedStudent.FirstName} {SelectedStudent.LastName}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            using var context = new Context();

           
            var studentToDelete = context.Students
                .Include(s => s.Relationships)
                .Include(s => s.AdvisoryLink) 
                .ThenInclude(a => a.Students) 
                .FirstOrDefault(s => s.StudentID == SelectedStudent.StudentID);

            if (studentToDelete == null)
            {
                MessageBox.Show("Student not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var relations = context.Relationships
                .Where(r => r.StudentID == studentToDelete.StudentID)
                .ToList();

            foreach (var rel in relations)
            {
                context.Relationships.Remove(rel);
            }

            
            if (studentToDelete.AdvisoryLink != null)
            {
                var advisory = studentToDelete.AdvisoryLink;
                advisory.Students.Remove(studentToDelete);

              
                if (!advisory.Students.Any())
                {
                    context.Advisories.Remove(advisory);
                }
            }

          
            context.Students.Remove(studentToDelete);
            context.SaveChanges();

            Students.Remove(SelectedStudent);

            MessageBox.Show("Student successfully deleted!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

           

        }




        private void OnAddStudent(object obj)
        {
            var addStudentView = new AddStudentView();
            addStudentView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            addStudentView.Show();
            var w = obj as Window;
            w.Close();

        }


        private void OnDoubleClick(object obj)
        {
            if(SelectedStudent != null)
            {
                var editStudentView = new EditStudentView(SelectedStudent);
                editStudentView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                editStudentView.Show();
                LoadStudents();
            }
        }




        public void LoadStudents()
        {
            using var context = new Context();
            var students = context.Students.ToList();
            Students = new ObservableCollection<Student>(students);
        }


    }
}
