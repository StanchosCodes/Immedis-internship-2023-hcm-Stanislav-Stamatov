using HumanCapitalManagement.Web.ViewModels.Employee;

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
