using HumanCapitalManagement.Data;
using HumanCapitalManagement.Data.Models;
using HumanCapitalManagement.Services.Data.Interfaces;
using HumanCapitalManagement.Web.ViewModels.Department;
using HumanCapitalManagement.Web.ViewModels.Employee;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Services.Data
{
    public class DepartmentService : IDepartmentService
    {
        private readonly HumanCapitalManagementContext context;

        public DepartmentService(HumanCapitalManagementContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<DepartmentViewModel>> GetAllAsync()
        {
            IEnumerable<DepartmentViewModel> allDepartments = await this.context
                .Departments
                .Include(d => d.Employees)
                .Where(d => d.IsDeleted == false)
                .Select(d => new DepartmentViewModel()
                {
                    Id = d.Id,
                    Title = d.Title,
                    ManagerUsername = d.Manager.Username,
                    ManagerFirstName = d.Manager.FirstName,
                    ManagerMiddleName = d.Manager.MiddleName,
                    ManagerLastName = d.Manager.LastName,
                    ManagerId = d.ManagerId,
                    Employees = d.Employees
                        .Where(e => e.IsEmployed)
                        .Select(e => new EmployeeViewModel()
                        {
                            Username = e.Username,
                            FirstName = e.FirstName,
                            MiddleName = e.MiddleName,
                            LastName = e.LastName
                        })
                        .ToList()
                })
                .ToListAsync();

            return allDepartments;
        }

        public async Task<DepartmentViewModel> GetByIdAsync(int id)
        {
            DepartmentViewModel? department = await this.context
                .Departments
                .Include(d => d.Employees)
                .Where(d => d.IsDeleted == false && d.Id == id)
                .Select(d => new DepartmentViewModel()
                {
                    Id = d.Id,
                    Title = d.Title,
                    ManagerUsername = d.Manager.Username,
                    ManagerFirstName = d.Manager.FirstName,
                    ManagerMiddleName = d.Manager.MiddleName,
                    ManagerLastName = d.Manager.LastName,
                    ManagerId = d.ManagerId,
                    Employees = d.Employees
                        .Where(e => e.IsEmployed)
                        .Select(e => new EmployeeViewModel()
                        {
                            Username = e.Username,
                            FirstName = e.FirstName,
                            MiddleName = e.MiddleName,
                            LastName = e.LastName
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            return department!;
        }

        public async Task AddAsync(DepartmentAddEditViewModel addModel)
        {
            Department department = new Department()
            {
                Title = addModel.Title,
                ManagerId = addModel.ManagerId
            };

            await this.context.Departments.AddAsync(department);
            await this.context.SaveChangesAsync();
        }

        public async Task EditAsync(int id, DepartmentAddEditViewModel editModel)
        {
            Department? department = await this.context
                .Departments
                .FirstOrDefaultAsync(d => d.Id == id && d.IsDeleted == false);

            if (department != null)
            {
                department.Title = editModel.Title;
                department.ManagerId = editModel.ManagerId;
            }

            await this.context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Department? departmentToDelete = await this.context
                .Departments
                .FirstOrDefaultAsync(d => d.Id == id && d.IsDeleted == false);

            if (departmentToDelete == null)
            {
                return false;
            }

            departmentToDelete.IsDeleted = true;

            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddEmployeeAsync(int departmentId, int employeeId)
        {
            Department? department = await this.context
                .Departments
                .FirstOrDefaultAsync(d => d.Id == departmentId && d.IsDeleted == false);

            if (department == null)
            {
                return false;
            }

            Employee? employee = await this.context
                .Employees
                .FirstOrDefaultAsync(e => e.Id == employeeId && e.IsEmployed);

            if (employee == null)
            {
                return false;
            }

            if (!department.Employees.Any(e => e.Id == employeeId))
            {
                department.Employees.Add(employee);

                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<EmployeeViewModel>> GetAllEmployeesAsync(int id)
        {
            Department? department = await this.context
                .Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == id && d.IsDeleted == false);

            if (department == null)
            {
                return new List<EmployeeViewModel>();
            }

            IEnumerable<EmployeeViewModel> allEmployees = department.Employees
                .Select(e => new EmployeeViewModel()
                {
                    Id = e.Id,
                    Username = e.Username,
                    FirstName = e.FirstName,
                    MiddleName = e.MiddleName,
                    LastName = e.LastName,
                    ImgUrl = e.ImgUrl
                })
                .ToList();

            return allEmployees;
        }

        public async Task<IEnumerable<EmployeeViewModel>> GetAllAvailableEmployeesAsync(int id)
        {
            IEnumerable<EmployeeViewModel>? allAvailable = await this.context
                .Employees
                .Where(e => e.DepartmentId != id && e.IsEmployed)
                .Select(e => new EmployeeViewModel()
                {
                    Id = e.Id,
                    Username = e.Username,
                    FirstName = e.FirstName,
                    MiddleName = e.MiddleName,
                    LastName = e.LastName,
                    ImgUrl = e.ImgUrl
                })
                .ToListAsync();

            return allAvailable!;
        }
    }
}
