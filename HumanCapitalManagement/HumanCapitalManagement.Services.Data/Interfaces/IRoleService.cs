using HumanCapitalManagement.Web.ViewModels.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Services.Data.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleViewModel>> GetAllAsync();

        Task<RoleViewModel> GetByIdAsync(int id);

        Task<bool> IsAdminByUsernameAsync(string username);

        Task AddAsync(AddRoleViewModel roleModel);
    }
}
