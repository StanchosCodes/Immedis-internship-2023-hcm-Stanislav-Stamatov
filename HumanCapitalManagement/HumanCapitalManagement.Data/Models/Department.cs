using System;
using System.Collections.Generic;

namespace HumanCapitalManagement.Data.Models;

public partial class Department
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid ManagerId { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual Employee Manager { get; set; } = null!;
}
