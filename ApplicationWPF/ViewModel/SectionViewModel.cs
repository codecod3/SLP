using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationWPF.Commands;
using ApplicationWPF.View;
using ApplicationWPF.Commands;
using Microsoft.EntityFrameworkCore;
using SoftwareDesignQueenAnneCuriosityShopProject;
using SoftwareDesignQueenAnneCuriosityShopProject.Entities;
using System.Windows.Input;

namespace ApplicationWPF.ViewModel
{
    public class SectionViewModel: NotifyPropertyChanged
    {
        public ObservableCollection<Advisory> Sections { get; set; }
        public ICommand AddSectionCommand { get; set; }

       








    }
}
