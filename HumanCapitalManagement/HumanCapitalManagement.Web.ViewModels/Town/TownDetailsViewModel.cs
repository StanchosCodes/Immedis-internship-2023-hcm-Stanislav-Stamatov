using HumanCapitalManagement.Web.ViewModels.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Web.ViewModels.Town
{
    public class TownDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string ImgUrl { get; set; } = null!;

        public IEnumerable<EmployeeViewModel> Employees { get; set; } = new List<EmployeeViewModel>();
    }
}
