using HumanCapitalManagement.Web.ViewModels.Employee;

namespace HumanCapitalManagement.Web.ViewModels.Department
{
    public class DepartmentViewModel
    {
        public DepartmentViewModel()
        {
            this.Employees = new HashSet<EmployeeViewModel>();
        }

        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public int ManagerId { get; set; }

        public string ManagerUsername { get; set; } = null!;

        public string ManagerFirstName { get; set; } = null!;

        public string ManagerMiddleName { get; set; } = null!;

        public string ManagerLastName { get; set; } = null!;

        public virtual IEnumerable<EmployeeViewModel> Employees { get; set; }
    }
}
