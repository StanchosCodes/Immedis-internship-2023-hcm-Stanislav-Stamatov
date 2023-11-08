namespace HumanCapitalManagement.Web.ViewModels.Project
{
    public class ProjectViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string ImgUrl { get; set; } = null!;

        public decimal Salary { get; set; }

        public string EndDate { get; set; } = null!;
    }
}
