using HumanCapitalManagement.Data;
using HumanCapitalManagement.Data.Models;
using HumanCapitalManagement.Web.ViewModels.Role;
using HumanCapitalManagement.Web.ViewModels.Town;
using HumanCapitalManagement.Web.ViewModels.Project;
using HumanCapitalManagement.Web.ViewModels.Employee;
using HumanCapitalManagement.Services.Data.Interfaces;
using HumanCapitalManagement.Web.ViewModels.Department;
using static HumanCapitalManagement.Common.GeneralConstants;

using System.Text;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace HumanCapitalManagement.Services.Data
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HumanCapitalManagementContext context;
        private readonly ITownService townService;
        private readonly IDepartmentService departmentService;
        private readonly IRoleService roleService;
        private readonly IConfiguration configuration;

        public EmployeeService(HumanCapitalManagementContext context,
                               ITownService townService,
                               IDepartmentService departmentService,
                               IRoleService roleService,
                               IConfiguration configuration)
        {
            this.context = context;
            this.townService = townService;
            this.departmentService = departmentService;
            this.roleService = roleService;
            this.configuration = configuration;
        }

        public async Task<IEnumerable<EmployeeViewModel>> GetAllAsync()
        {
            IEnumerable<EmployeeViewModel> employees = await this.context
                .Employees
                .Where(e => e.IsEmployed)
                .Select(e => new EmployeeViewModel()
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    MiddleName = e.MiddleName,
                    LastName = e.LastName,
                    Username = e.Username,
                    ImgUrl = e.ImgUrl
                })
                .ToListAsync();

            return employees;
        }

        public async Task<EmployeeDetailsViewModel> GetByIdAsync(int id)
        {
            EmployeeDetailsViewModel? employeeDetails = await this.context
                .Employees
                .Include(e => e.Projects)
                .Include(e => e.Roles)
                .Where(e => e.IsEmployed && e.Id == id)
                .Select(e => new EmployeeDetailsViewModel()
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    MiddleName = e.MiddleName,
                    LastName = e.LastName,
                    Username = e.Username,
                    ImgUrl = e.ImgUrl,
                    Role = e.Roles.Select(r => r.Name).First(),
                    Age = e.Age,
                    Town = e.Town.Name,
                    Email = e.Email,
                    JobTitle = e.JobTitle,
                    HireDate = e.HireDate.ToString("f"),
                    Manager = $"{e.Manager.Username} - {e.Manager.FirstName} {e.Manager.MiddleName} {e.Manager.LastName}",
                    Department = e.Department.Title,
                    AverageSalary = e.Projects.Select(p => p.Salary).ToArray().Average(),
                    Projects = e.Projects.Select(p => new ProjectViewModel()
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Salary = p.Salary,
                        ImgUrl = p.ImgUrl,
                        EndDate = p.EndDate.ToString("f")
                    })
                    .ToList()
                }).FirstOrDefaultAsync();

            return employeeDetails!;
        }

        public async Task<int> GetIdByUsername(string username)
        {
            Employee? employee = await this.context
                .Employees
                .FirstOrDefaultAsync(e => e.Username == username && e.IsEmployed);

            if (employee == null)
            {
                return 0;
            }

            int employeeId = employee.Id;

            return employeeId;
        }

        public async Task<bool> RegisterAsync(RegisterViewModel registerModel)
        {
            bool isUsernameTaken = await this.context
                .Employees.AnyAsync(e => e.Username == registerModel.Username);

            if (isUsernameTaken)
            {
                return false;
            }

            Employee newUser = new Employee()
            {
                Email = registerModel.Email,
                Username = registerModel.Username,
                FirstName = registerModel.FirstName,
                MiddleName = registerModel.MiddleName,
                LastName = registerModel.LastName,
                JobTitle = registerModel.JobTitle,
                ImgUrl = registerModel.ImgUrl,
                Age = registerModel.Age,
                DepartmentId = registerModel.DepartmentId,
                ManagerId = registerModel.ManagerId,
                TownId = registerModel.TownId,
                HireDate = DateTime.UtcNow,
                IsEmployed = true
            };

            newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerModel.Password);

            await this.context.Employees.AddAsync(newUser);
            await this.context.SaveChangesAsync();

            int newUserId = await this.GetIdByUsername(registerModel.Username);
            Employee? currentNewEmployee = await this.context.Employees.FirstOrDefaultAsync(e => e.Id == newUserId);
            Role? employeeRole = await this.context.Roles.FirstOrDefaultAsync(r => r.Name == EmployeeRoleName);

            currentNewEmployee!.Roles.Add(employeeRole!);
            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<string> LoginAsync(LoginViewModel loginModel)
        {
            Employee? currentUser = await this.context
                .Employees
                .Include(e => e.Roles)
                .FirstOrDefaultAsync(e => e.Username == loginModel.Username);

            if (currentUser == null)
            {
                return string.Empty;
            }

            bool isEqual = BCrypt.Net.BCrypt.Verify(loginModel.Password, currentUser.PasswordHash);

            if (!isEqual)
            {
                return string.Empty;
            }

            RoleViewModel? choosenRole = await this.roleService.GetByIdAsync(loginModel.RoleId);

            if (!currentUser.Roles.Any(r => r.Name == choosenRole.Name))
            {
                return string.Empty;
            }

            string stringToken = this.GenerateJWT(loginModel, choosenRole.Name);

            return stringToken;
        }

        public async Task<RegisterHelperViewModel> FillRegisterCollectionsAsync()
        {
            IEnumerable<TownViewModel>? towns = await this.townService.GetAllAsync();

            IEnumerable<DepartmentViewModel>? departments = await this.departmentService.GetAllAsync();

            IEnumerable<EmployeeViewModel>? managers = await this.GetAllAsync();

            RegisterHelperViewModel registerCollections = new RegisterHelperViewModel()
            {
                Towns = towns,
                Departments = departments,
                Managers = managers
            };

            return registerCollections;
        }

        private string GenerateJWT(LoginViewModel loginModel, string roleName)
        {
            SymmetricSecurityKey? securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Jwt:Key"]!));
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, loginModel.Username),
                new Claim(ClaimTypes.Role, roleName)
            };

            JwtSecurityToken securityToken = new JwtSecurityToken(
                this.configuration["Jwt:Issuer"],
                this.configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(180),
                signingCredentials: signingCredentials
                );

            string stringToken = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return stringToken;
        }

        public async Task EditAsync(int id, EditViewModel model)
        {
            Employee? employee = await this.context
                .Employees
                .FirstOrDefaultAsync(e => e.Id == id && e.IsEmployed);

            if (employee != null)
            {
                employee.Email = model.Email;
                employee.Username = model.Username;
                employee.FirstName = model.FirstName;
                employee.MiddleName = model.MiddleName;
                employee.LastName = model.LastName;
                employee.JobTitle = model.JobTitle;
                employee.ImgUrl = model.ImgUrl;
                employee.Age = model.Age;
                employee.DepartmentId = model.DepartmentId;
                employee.ManagerId = model.ManagerId;
                employee.TownId = model.TownId;
            }

            await this.context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Employee? employeeToDelete = await this.context
                .Employees
                .FirstOrDefaultAsync(e => e.Id == id && e.IsEmployed);

            if (employeeToDelete == null)
            {
                return false;
            }

            employeeToDelete.IsEmployed = false;

            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<EditViewModel> GenerateEditViewModelAsync(int id)
        {
            RegisterHelperViewModel collections = await this.FillRegisterCollectionsAsync();

            Employee? employee = await this.context
                .Employees
                .FirstOrDefaultAsync(e => e.Id == id && e.IsEmployed);

            if (employee == null)
            {
                return new EditViewModel();
            }

            EditViewModel model = new EditViewModel()
            {
                Departments = collections.Departments,
                Towns = collections.Towns,
                Managers = collections.Managers,
                DepartmentId = employee.DepartmentId,
                TownId = employee.TownId,
                ManagerId = employee.ManagerId,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                LastName = employee.LastName,
                Age = employee.Age,
                Username = employee.Username,
                Email = employee.Email,
                JobTitle = employee.JobTitle,
                ImgUrl = employee.ImgUrl
            };

            return model;
        }

        public async Task MakeAdminAsync(int id)
        {
            Employee? employee = await this.context
                .Employees
                .Include(e => e.Roles)
                .FirstOrDefaultAsync(e => e.Id == id && e.IsEmployed);

            Role? adminRole = await this.context
                .Roles
                .FirstOrDefaultAsync(r => r.Name == AdminRoleName);

            if (!employee!.Roles.Any(r => r.Name == AdminRoleName))
            {
                employee.Roles.Add(adminRole!);
                await this.context.SaveChangesAsync();
            }
        }
    }
}
