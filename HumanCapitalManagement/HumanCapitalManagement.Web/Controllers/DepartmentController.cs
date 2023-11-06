using HumanCapitalManagement.Web.ViewModels.Department;
using HumanCapitalManagement.Web.ViewModels.Employee;
using HumanCapitalManagement.Web.ViewModels.Project;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using static HumanCapitalManagement.Common.GeneralConstants;

namespace HumanCapitalManagement.Web.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly HttpClient apiClient;

        public DepartmentController(IHttpClientFactory httpClientFactory)
        {
            this.apiClient = httpClientFactory.CreateClient("DbApi");
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            string currentUserName = this.HttpContext.Session.GetString("Username")!;

            if (String.IsNullOrEmpty(currentUserName))
            {
                return this.Unauthorized();
            }

            string token = this.HttpContext.Session.GetString("JWT")!;
            this.apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            IEnumerable<DepartmentViewModel>? departments = await this.apiClient
                .GetFromJsonAsync<IEnumerable<DepartmentViewModel>>("Department");

            return View(departments);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            string currentUserName = this.HttpContext.Session.GetString("Username")!;

            if (String.IsNullOrEmpty(currentUserName))
            {
                return this.Unauthorized();
            }

            string token = this.HttpContext.Session.GetString("JWT")!;
            this.apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            DepartmentViewModel? department = await this.apiClient.GetFromJsonAsync<DepartmentViewModel>($"Department/{id}");

            return View(department);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            string currentUserRole = this.HttpContext.Session.GetString("Role")!;
            string currentUserName = this.HttpContext.Session.GetString("Username")!;

            if (String.IsNullOrEmpty(currentUserName))
            {
                return this.Unauthorized();
            }

            if (currentUserRole != AdminRoleName)
            {
                return this.Forbid();
            }

            string token = this.HttpContext.Session.GetString("JWT")!;
            this.apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            HashSet<EmployeeViewModel>? managers = await this.apiClient
                .GetFromJsonAsync<HashSet<EmployeeViewModel>>("Employee");

            DepartmentAddEditViewModel departmentModel = new DepartmentAddEditViewModel()
            {
                Managers = managers!
            };

            return View(departmentModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(DepartmentAddEditViewModel addModel)
        {
            string token = this.HttpContext.Session.GetString("JWT")!;
            this.apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            if (!ModelState.IsValid)
            {
                HashSet<EmployeeViewModel>? managers = await this.apiClient
                .GetFromJsonAsync<HashSet<EmployeeViewModel>>("Employee");

                addModel.Managers = managers!;

                return View(managers);
            }

            try
            {
                HttpResponseMessage response = await this.apiClient.PostAsJsonAsync("Department", addModel);

                if (!response.IsSuccessStatusCode)
                {
                    return this.BadRequest();
                }

                return RedirectToAction("All", "Department");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Failed to add department!");

                return View(addModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            string currentUserRole = this.HttpContext.Session.GetString("Role")!;
            string currentUserName = this.HttpContext.Session.GetString("Username")!;

            if (String.IsNullOrEmpty(currentUserName))
            {
                return this.Unauthorized();
            }

            if (currentUserRole != AdminRoleName)
            {
                return this.Forbid();
            }

            string token = this.HttpContext.Session.GetString("JWT")!;
            this.apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            DepartmentViewModel? department = await this.apiClient
                .GetFromJsonAsync<DepartmentViewModel>($"Department/{id}");

            if (department == null)
            {
                return this.BadRequest();
            }

            HashSet<EmployeeViewModel>? managers = await this.apiClient
               .GetFromJsonAsync<HashSet<EmployeeViewModel>>("Employee");

            DepartmentAddEditViewModel editModel = new DepartmentAddEditViewModel()
            {
                Title = department.Title,
                ManagerId = department.ManagerId,
                Managers = managers!
            };

            return View(editModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, DepartmentAddEditViewModel editModel)
        {
            string token = this.HttpContext.Session.GetString("JWT")!;
            this.apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            if (!ModelState.IsValid)
            {
                HashSet<EmployeeViewModel>? managers = await this.apiClient
               .GetFromJsonAsync<HashSet<EmployeeViewModel>>("Employee");

                editModel.Managers = managers!;

                return View(editModel);
            }

            try
            {
                HttpResponseMessage response = await this.apiClient
                    .PutAsJsonAsync<DepartmentAddEditViewModel>($"Department/{id}", editModel);

                if (!response.IsSuccessStatusCode)
                {
                    return this.BadRequest();
                }

                return RedirectToAction("Details", "Department", new { id = id });
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Failed to edit department!");

                return View(editModel);
            }
        }

        public async Task<IActionResult> AddEmployee(int id)
        {
            string currentUserRole = this.HttpContext.Session.GetString("Role")!;
            string currentUserName = this.HttpContext.Session.GetString("Username")!;

            if (String.IsNullOrEmpty(currentUserName))
            {
                return this.Unauthorized();
            }

            if (currentUserRole != AdminRoleName)
            {
                return this.Forbid();
            }

            string token = this.HttpContext.Session.GetString("JWT")!;
            this.apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            int? departmentId = this.HttpContext.Session.GetInt32("DepartmentId");

            HttpResponseMessage addResponse = await this.apiClient
                .PostAsJsonAsync<int>($"Department/AddEmployee/{departmentId}", id);

            if (!addResponse.IsSuccessStatusCode)
            {
                return this.BadRequest();
            }

            return RedirectToAction("AllAvailable", "Department");
        }

        public async Task<IActionResult> AllEmployees(int id)
        {
            string currentUserName = this.HttpContext.Session.GetString("Username")!;

            if (String.IsNullOrEmpty(currentUserName))
            {
                return this.Unauthorized();
            }

            string token = this.HttpContext.Session.GetString("JWT")!;
            this.apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            IEnumerable<EmployeeViewModel>? allEmployees = await this.apiClient
                .GetFromJsonAsync<IEnumerable<EmployeeViewModel>>($"Department/AllEmployees/{id}");

            return View(allEmployees!);
        }

        public async Task<IActionResult> AllAvailable(int id)
        {
            string currentUserRole = this.HttpContext.Session.GetString("Role")!;
            string currentUserName = this.HttpContext.Session.GetString("Username")!;

            if (String.IsNullOrEmpty(currentUserName))
            {
                return this.Unauthorized();
            }

            if (currentUserRole != AdminRoleName)
            {
                return this.Forbid();
            }

            string token = this.HttpContext.Session.GetString("JWT")!;
            this.apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            IEnumerable<EmployeeViewModel>? allAvailableEmployees = await this.apiClient
                .GetFromJsonAsync<IEnumerable<EmployeeViewModel>>($"Department/AllAvailableEmployees/{id}");

            return View(allAvailableEmployees!);
        }

        public async Task<IActionResult> Delete(int id)
        {
            string currentUserRole = this.HttpContext.Session.GetString("Role")!;
            string currentUserName = this.HttpContext.Session.GetString("Username")!;

            if (String.IsNullOrEmpty(currentUserName))
            {
                return this.Unauthorized();
            }

            if (currentUserRole != AdminRoleName)
            {
                return this.Forbid();
            }

            string token = this.HttpContext.Session.GetString("JWT")!;
            this.apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            bool isDeleted = false;

            HttpResponseMessage response = await this.apiClient.DeleteAsync($"Department/{id}");

            if (response.IsSuccessStatusCode)
            {
                isDeleted = true;
            }

            if (!isDeleted)
            {
                return this.BadRequest();
            }

            return RedirectToAction("All", "Department");
        }
    }
}
