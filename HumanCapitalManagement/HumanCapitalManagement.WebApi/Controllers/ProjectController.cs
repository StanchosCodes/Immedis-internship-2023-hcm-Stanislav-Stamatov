using HumanCapitalManagement.Data.Models;
using HumanCapitalManagement.Services.Data.Interfaces;
using HumanCapitalManagement.Web.ViewModels.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HumanCapitalManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService projectService;

        public ProjectController(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<ProjectViewModel> allProjects = await this.projectService.GetAllAsync();

            return this.Ok(allProjects);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            ProjectDetailsViewModel projectDetails = await this.projectService.GetByIdAsync(id);

            return this.Ok(projectDetails);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody]AddEditProjectViewModel projectModel)
        {
            await this.projectService.AddAsync(projectModel);

            return this.Ok(projectModel);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [FromBody]AddEditProjectViewModel projectModel)
        {
           await this.projectService.EditAsync(id, projectModel);

            return this.Ok(projectModel);
        }

        [HttpPost("Assign/{id}")]
        [Authorize]
        public async Task<IActionResult> Assign(int id, [FromBody]int employeeId)
        {
            bool isAssigned = await this.projectService.AssignAsync(id, employeeId);

            return this.Ok(isAssigned);
        }

        [HttpPost("Leave/{id}")]
        [Authorize]
        public async Task<IActionResult> Leave(int id, [FromBody]int employeeId)
        {
            bool isLeaved = await this.projectService.LeaveAsync(id, employeeId);

            if (!isLeaved)
            {
                return this.BadRequest(isLeaved);
            }

            return this.Ok(isLeaved);
        }

        [HttpPost]
        [Route("AllAssigned")]
        [Authorize]
        public async Task<IActionResult> GetAllAssigned([FromBody]int employeeId)
        {
            IEnumerable<ProjectViewModel> allAssignedProjects = await this.projectService.GetAllAssignedAsync(employeeId);

            return this.Ok(allAssignedProjects);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            bool isDeleted = await this.projectService.DeleteAsync(id);

            return this.Ok(isDeleted);
        }
    }
}
