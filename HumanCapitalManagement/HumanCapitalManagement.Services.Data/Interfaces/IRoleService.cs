using HumanCapitalManagement.Web.ViewModels.Role;

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
