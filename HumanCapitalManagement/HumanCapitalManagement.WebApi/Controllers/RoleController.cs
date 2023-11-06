using HumanCapitalManagement.Services.Data.Interfaces;
using HumanCapitalManagement.Web.ViewModels.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HumanCapitalManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService roleService;

        public RoleController(IRoleService roleService)
        {
            this.roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<RoleViewModel> allRoles = await this.roleService.GetAllAsync();

            return this.Ok(allRoles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            RoleViewModel role = await this.roleService.GetByIdAsync(id);

            return this.Ok(role);
        }

        [HttpGet("IsAdmin/{username}")]
        public async Task<IActionResult> IsAdmin(string username)
        {
            bool isAdmin = await this.roleService.IsAdminByUsernameAsync(username);

            return this.Ok(isAdmin);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddRoleViewModel roleModel)
        {
            await this.roleService.AddAsync(roleModel);

            return this.Ok();
        }
    }
}
