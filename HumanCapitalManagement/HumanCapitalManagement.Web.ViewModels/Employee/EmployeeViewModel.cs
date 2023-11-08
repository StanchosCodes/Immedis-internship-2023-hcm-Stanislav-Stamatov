namespace HumanCapitalManagement.Web.ViewModels.Employee
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string MiddleName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string ImgUrl { get; set; } = null!;
    }
}
