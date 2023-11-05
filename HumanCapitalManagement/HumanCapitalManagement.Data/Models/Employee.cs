using System;
using System.Collections.Generic;

namespace HumanCapitalManagement.Data.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int TownId { get; set; }

    public int Age { get; set; }

    public string JobTitle { get; set; } = null!;

    public DateTime HireDate { get; set; }

    public int DepartmentId { get; set; }

    public int ManagerId { get; set; }

    public string ImgUrl { get; set; } = null!;

    public bool IsEmployed { get; set; }

    public virtual Town Town { get; set; } = null!;

    public virtual Department Department { get; set; } = null!;

    public virtual Employee Manager { get; set; } = null!;

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>(); // Old/Original Name - InverseManager

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
