using System;
using System.Collections.Generic;

namespace HumanCapitalManagement.Data.Models;

public partial class Project
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Salary { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
