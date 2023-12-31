﻿using HumanCapitalManagement.Web.ViewModels.Department;
using HumanCapitalManagement.Web.ViewModels.Employee;

namespace HumanCapitalManagement.Services.Data.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentViewModel>> GetAllAsync();

        Task<DepartmentViewModel> GetByIdAsync(int id);

        Task AddAsync(DepartmentAddEditViewModel addModel);

        Task EditAsync(int id, DepartmentAddEditViewModel editModel);

        Task<bool> AddEmployeeAsync(int departmentId, int employeeId);

        Task<IEnumerable<EmployeeViewModel>> GetAllEmployeesAsync(int id);

        Task<IEnumerable<EmployeeViewModel>> GetAllAvailableEmployeesAsync(int id);

        Task<bool> DeleteAsync(int id);
    }
}
