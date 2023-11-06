using HumanCapitalManagement.Data;
using HumanCapitalManagement.Data.Models;
using HumanCapitalManagement.Services.Data.Interfaces;
using HumanCapitalManagement.Web.ViewModels.Role;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HumanCapitalManagement.Common.GeneralConstants;

namespace HumanCapitalManagement.Services.Data
{
    public class RoleService : IRoleService
    {
        private readonly HumanCapitalManagementContext context;

        public RoleService(HumanCapitalManagementContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<RoleViewModel>> GetAllAsync()
        {
            IEnumerable<RoleViewModel> allRoles = await this.context
                .Roles
                .Select(r => new RoleViewModel()
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .ToListAsync();

            return allRoles;
        }

        public async Task<RoleViewModel> GetByIdAsync(int id)
        {
            RoleViewModel? role = await this.context
                .Roles
                .Select(r => new RoleViewModel()
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .FirstOrDefaultAsync(r => r.Id == id);

            return role!;
        }

        public async Task AddAsync(AddRoleViewModel roleModel)
        {
            Role newRole = new Role()
            {
                Name = roleModel.Name,
                NormalizedName = roleModel.Name.ToUpper()
            };

            await this.context.Roles.AddAsync(newRole);
            await this.context.SaveChangesAsync();
        }

        public async Task<bool> IsAdminByUsernameAsync(string username)
        {
            bool isAdmin = await this.context
                .Employees
                .Include(e => e.Roles)
                .AnyAsync(e => e.Username == username && e.Roles.Any(r => r.Name == AdminRoleName));

            return isAdmin;
        }
    }
}
