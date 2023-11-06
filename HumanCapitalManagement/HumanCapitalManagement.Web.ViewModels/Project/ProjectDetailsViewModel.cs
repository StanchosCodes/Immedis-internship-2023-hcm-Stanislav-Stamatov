using HumanCapitalManagement.Web.ViewModels.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Web.ViewModels.Project
{
    public class ProjectDetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string ImgUrl { get; set; } = null!;

        public decimal Salary { get; set; }

        public string StartDate { get; set; } = null!;

        public string EndDate { get; set; } = null!;

        public virtual ICollection<EmployeeViewModel> Employees { get; set; } = new List<EmployeeViewModel>();
    }
}
