using HumanCapitalManagement.Data.Models;
using HumanCapitalManagement.Web.ViewModels.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Services.Data.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectViewModel>> GetAllAsync();

        Task<ProjectDetailsViewModel> GetByIdAsync(int id);

        Task AddAsync(AddEditProjectViewModel projectModel);

        Task EditAsync(int id, AddEditProjectViewModel projectModel);

        Task<bool> AssignAsync(int projectId, int employeeId);

        Task<bool> LeaveAsync(int projectId, int employeeId);

        Task<IEnumerable<ProjectViewModel>> GetAllAssignedAsync(int employeeId);

        Task<bool> DeleteAsync(int id);
    }
}
