using HumanCapitalManagement.Web.ViewModels.Department;
using HumanCapitalManagement.Web.ViewModels.Town;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Web.ViewModels.Employee
{
    public class RegisterHelperViewModel
    {
        public RegisterHelperViewModel()
        {
            this.Towns = new HashSet<TownViewModel>();
            this.Departments = new HashSet<DepartmentViewModel>();
            this.Managers = new HashSet<EmployeeViewModel>();
        }

        public virtual IEnumerable<TownViewModel> Towns { get; set; }

        public virtual IEnumerable<DepartmentViewModel> Departments { get; set; }

        public virtual IEnumerable<EmployeeViewModel> Managers { get; set; }
    }
}
