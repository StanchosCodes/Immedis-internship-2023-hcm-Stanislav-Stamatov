using System;
using System.Collections.Generic;

namespace HumanCapitalManagement.Data.Models;

public partial class Role
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string NormalizedName { get; set; } = null!;

    public virtual ICollection<Employee> Users { get; set; } = new List<Employee>();
}
