using HumanCapitalManagement.Web.ViewModels.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
