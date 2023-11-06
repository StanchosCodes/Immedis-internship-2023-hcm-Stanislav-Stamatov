using HumanCapitalManagement.Services.Data.Interfaces;
using HumanCapitalManagement.Web.ViewModels.Department;
using HumanCapitalManagement.Web.ViewModels.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HumanCapitalManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            this.departmentService = departmentService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<DepartmentViewModel> allDepartments = await this.departmentService.GetAllAsync();

            return this.Ok(allDepartments);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            DepartmentViewModel department = await this.departmentService.GetByIdAsync(id);

            return this.Ok(department);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(DepartmentAddEditViewModel addModel)
        {
            await this.departmentService.AddAsync(addModel);

            return this.Ok(addModel);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(int id, DepartmentAddEditViewModel editModel)
        {
            await this.departmentService.EditAsync(id, editModel);

            return this.Ok(editModel);
        }

        [HttpPost("AddEmployee/{id}")]
        [Authorize]
        public async Task<IActionResult> AddEmployee(int id, [FromBody]int employeeId)
        {
            bool isAdded = await this.departmentService.AddEmployeeAsync(id, employeeId);

            return this.Ok(isAdded);
        }

        [HttpGet("AllEmployees/{id}")]
        [Authorize]
        public async Task<IActionResult> AllEmployees(int id)
        {
            IEnumerable<EmployeeViewModel> allEmployees = await this.departmentService.GetAllEmployeesAsync(id);

            return this.Ok(allEmployees);
        }

        [HttpGet("AllAvailableEmployees/{id}")]
        [Authorize]
        public async Task<IActionResult> AllAvailable(int id)
        {
            IEnumerable<EmployeeViewModel>? allAvailableEmployees = await this.departmentService
                .GetAllAvailableEmployeesAsync(id);

            return this.Ok(allAvailableEmployees!);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            bool isDeleted = await this.departmentService.DeleteAsync(id);

            return this.Ok(isDeleted);
        }
    }
}
