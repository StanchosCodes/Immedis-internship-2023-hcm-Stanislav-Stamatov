using HumanCapitalManagement.Web.ViewModels.Town;
using HumanCapitalManagement.Web.ViewModels.Department;

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
