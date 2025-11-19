using System;
using System.Collections.ObjectModel;
using System.Linq;
using ApplicationWPF.Commands;
using Microsoft.EntityFrameworkCore;
using SoftwareDesignQueenAnneCuriosityShopProject;
using SoftwareDesignQueenAnneCuriosityShopProject.Entities;

namespace ApplicationWPF.ViewModel
{
    public class AttendanceViewModel : NotifyPropertyChanged
    {
        public Student SelectedStudent { get; set; }

        private ObservableCollection<Attendance> _attendances;
        public ObservableCollection<Attendance> Attendances
        {
            get => _attendances;
            set
            {
                _attendances = value;
                OnPropertyChanged(nameof(Attendances));
            }
        }

        public string SelectedStudentFullName => SelectedStudent != null
            ? $"{SelectedStudent.FirstName} {SelectedStudent.LastName}"
            : "Unknown";

        public AttendanceViewModel(Student stu)
        {
            SelectedStudent = stu;
            LoadAttendances();
        }

        private void LoadAttendances()
        {
            using var db = new Context();
            var records = db.Attendances
                .Where(a => a.StudentID == SelectedStudent.StudentID)
                .OrderByDescending(a => a.Date)
                .ToList();

            Attendances = new ObservableCollection<Attendance>(records);
        }
    }
}
