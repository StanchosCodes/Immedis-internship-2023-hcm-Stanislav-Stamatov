using HumanCapitalManagement.Web.ViewModels.Employee;

namespace HumanCapitalManagement.Web.ViewModels.Department
{
    public class DepartmentAddEditViewModel
    {
        public DepartmentAddEditViewModel()
        {
            this.Managers = new HashSet<EmployeeViewModel>();
        }

        public string Title { get; set; } = null!;

        public int ManagerId { get; set; }

        public virtual ICollection<EmployeeViewModel> Managers { get; set; }
    }
}
