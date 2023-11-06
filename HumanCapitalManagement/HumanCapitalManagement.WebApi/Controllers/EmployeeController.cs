using HumanCapitalManagement.Services.Data.Interfaces;
using HumanCapitalManagement.Web.ViewModels.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HumanCapitalManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<EmployeeViewModel> allEmployees = await this.employeeService.GetAllAsync();

            return this.Ok(allEmployees);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            EmployeeDetailsViewModel employeeDetails = await this.employeeService.GetByIdAsync(id);

            return this.Ok(employeeDetails);
        }

        [HttpPost]
        [Route("GetId")]
        public async Task<IActionResult> GetId([FromBody] string username)
        {
            int employeeId = await this.employeeService.GetIdByUsername(username);

            return this.Ok(employeeId);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel registerModel)
        {
            bool isSuccessfulRegister = await this.employeeService.RegisterAsync(registerModel);

            return this.Ok(isSuccessfulRegister);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginModel)
        {
            string resultToken = await this.employeeService.LoginAsync(loginModel);

            return this.Ok(resultToken);
        }

        [HttpGet]
        [Route("GetCollections")]
        public async Task<IActionResult> GetCollections()
        {
            RegisterHelperViewModel registerHelper = await this.employeeService.FillRegisterCollectionsAsync();

            return this.Ok(registerHelper);
        }

        [HttpGet("GenerateEditModel/{id}")]
        public async Task<IActionResult> GenerateEditModel(int id)
        {
            EditViewModel editModel = await this.employeeService.GenerateEditViewModelAsync(id);

            return this.Ok(editModel);
        }

        [HttpGet("MakeAdmin/{id}")]
        [Authorize]
        public async Task<IActionResult> MakeAdmin(int id)
        {
            await this.employeeService.MakeAdminAsync(id);

            return this.Ok();
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [FromBody] EditViewModel editModel)
        {
            await this.employeeService.EditAsync(id, editModel);

            return this.Ok(editModel);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            bool isDeleted = await this.employeeService.DeleteAsync(id);

            return this.Ok(isDeleted);
        }
    }
}