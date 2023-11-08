using HumanCapitalManagement.Web.ViewModels.Project;

namespace HumanCapitalManagement.Web.ViewModels.Employee
{
    public class EmployeeDetailsViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string MiddleName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Town { get; set; } = null!;

        public int Age { get; set; }

        public string JobTitle { get; set; } = null!;

        public string HireDate { get; set; } = null!;

        public decimal? AverageSalary { get; set; }

        public string Manager { get; set; } = null!;

        public string Department { get; set; } = null!;

        public string ImgUrl { get; set; } = null!;

        public string Role { get; set; } = null!;

        public virtual ICollection<ProjectViewModel> Projects { get; set; } = new List<ProjectViewModel>();
    }
}
