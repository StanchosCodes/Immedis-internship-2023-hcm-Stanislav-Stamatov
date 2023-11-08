using HumanCapitalManagement.Web.ViewModels.Employee;

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
