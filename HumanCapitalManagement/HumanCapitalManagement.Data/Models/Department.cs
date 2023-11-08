namespace HumanCapitalManagement.Data.Models;

public partial class Department
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int ManagerId { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Employee Manager { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
