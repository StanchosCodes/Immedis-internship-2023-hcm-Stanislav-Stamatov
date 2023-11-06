using HumanCapitalManagement.Web.ViewModels.Department;
using HumanCapitalManagement.Web.ViewModels.Employee;
using HumanCapitalManagement.Web.ViewModels.Project;
using HumanCapitalManagement.Web.ViewModels.Role;
using HumanCapitalManagement.Web.ViewModels.Town;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Numerics;
using System.Text.Json;
using static HumanCapitalManagement.Common.GeneralConstants;

namespace HumanCapitalManagement.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly HttpClient apiClient;

        public EmployeeController(IHttpClientFactory httpClientFactory)
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

            IEnumerable<EmployeeViewModel>? allEmployees = await this.apiClient
                .GetFromJsonAsync<IEnumerable<EmployeeViewModel>>("Employee");

            return View(allEmployees);
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

            EmployeeDetailsViewModel? employeeDetails = await this.apiClient
                .GetFromJsonAsync<EmployeeDetailsViewModel>($"Employee/{id}");

            return View(employeeDetails);
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            RegisterHelperViewModel? registerCollections = await this.apiClient
                .GetFromJsonAsync<RegisterHelperViewModel>("Employee/GetCollections");

            RegisterViewModel registerModel = new RegisterViewModel()
            {
                Departments = registerCollections!.Departments,
                Towns = registerCollections.Towns,
                Managers = registerCollections.Managers
            };

            return View(registerModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                RegisterHelperViewModel? registerCollections = await this.apiClient
                .GetFromJsonAsync<RegisterHelperViewModel>("Employee/GetCollections");

                registerModel.Departments = registerCollections!.Departments;
                registerModel.Towns = registerCollections.Towns;
                registerModel.Managers = registerCollections.Managers;

                return View(registerModel);
            }

            try
            {
                HttpResponseMessage response = await this.apiClient.PostAsJsonAsync("Employee/Register", registerModel);

                if (!response.IsSuccessStatusCode)
                {
                    return this.BadRequest();
                }

                return RedirectToAction("Login", "Employee");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Failed register!");

                return View(registerModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            IEnumerable<RoleViewModel>? allRoles = await this.apiClient
                .GetFromJsonAsync<IEnumerable<RoleViewModel>>("Role");

            LoginViewModel loginModel = new LoginViewModel()
            {
                Roles = allRoles!
            };

            return View(loginModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<RoleViewModel>? allRoles = await this.apiClient
                .GetFromJsonAsync<IEnumerable<RoleViewModel>>("Role");

                loginModel.Roles = allRoles!;

                return View(loginModel);
            }

            try
            {
                HttpResponseMessage jwtResponse = await this.apiClient.PostAsJsonAsync("Employee/Login", loginModel);

                if (!jwtResponse.IsSuccessStatusCode)
                {
                    this.ModelState.AddModelError("", "Failed to login");

                    IEnumerable<RoleViewModel>? allRoles = await this.apiClient
                        .GetFromJsonAsync<IEnumerable<RoleViewModel>>("Role");

                    loginModel.Roles = allRoles!;

                    return View(loginModel);
                }

                string? jwt = await jwtResponse.Content.ReadFromJsonAsync<string>();

                if (string.IsNullOrEmpty(jwt))
                {
                    this.ModelState.AddModelError("", "Failed to login");

                    IEnumerable<RoleViewModel>? allRoles = await this.apiClient
                        .GetFromJsonAsync<IEnumerable<RoleViewModel>>("Role");

                    loginModel.Roles = allRoles!;

                    return View(loginModel);
                }

                RoleViewModel? currentRole = await this.apiClient.GetFromJsonAsync<RoleViewModel>($"Role/{loginModel.RoleId}");

                this.HttpContext.Session.SetString("JWT", $"{jwt}");
                this.HttpContext.Session.SetString("Username", $"{loginModel.Username}");
                this.HttpContext.Session.SetString("Role", $"{currentRole!.Name}");

                return RedirectToAction("All", "Employee");
            }
            catch (Exception)
            {
                this.ModelState.AddModelError("", "Failed to login");

                return View(loginModel);
            }
        }

        public IActionResult Logout()
        {
            string currentUserName = this.HttpContext.Session.GetString("Username")!;

            if (String.IsNullOrEmpty(currentUserName))
            {
                return this.Unauthorized();
            }

            string token = this.HttpContext.Session.GetString("JWT")!;
            this.apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            this.HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            string currentUserName = this.HttpContext.Session.GetString("Username")!;

            if (String.IsNullOrEmpty(currentUserName))
            {
                return this.Unauthorized();
            }

            EditViewModel? editModel = await this.apiClient
                .GetFromJsonAsync<EditViewModel>($"Employee/GenerateEditModel/{id}");

            if (editModel!.Username == null)
            {
                return this.NotFound();
            }

            string currentUserRole = this.HttpContext.Session.GetString("Role")!;

            bool isEditModelAdmin = await this.apiClient.GetFromJsonAsync<bool>($"Role/IsAdmin/{editModel!.Username}");

            if (currentUserRole == EmployeeRoleName)
            {
                if (currentUserName == editModel!.Username)
                {
                    return View(editModel);
                }

                return this.Forbid();
            }
            else
            {
                if (currentUserName == editModel!.Username || isEditModelAdmin == false)
                {
                    return View(editModel);
                }
            }

            return this.Forbid();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditViewModel editModel)
        {
            if (!ModelState.IsValid)
            {
                RegisterHelperViewModel? collections = await this.apiClient
                .GetFromJsonAsync<RegisterHelperViewModel>("Employee/GetCollections");

                editModel.Departments = collections!.Departments;
                editModel.Towns = collections.Towns;
                editModel.Managers = collections.Managers;

                return View(editModel);
            }

            string token = this.HttpContext.Session.GetString("JWT")!;
            this.apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            HttpResponseMessage response = await this.apiClient
                .PutAsJsonAsync<EditViewModel>($"Employee/{id}", editModel);

            if (!response.IsSuccessStatusCode)
            {
                return this.BadRequest();
            }

            return RedirectToAction("Details", "Employee", new { id = id });
        }

        public async Task<IActionResult> MakeAdmin(int id)
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

            await this.apiClient.GetAsync($"Employee/MakeAdmin/{id}");

            return RedirectToAction("Details", "Employee", new { id = id });
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

            HttpResponseMessage response = await this.apiClient.DeleteAsync($"Employee/{id}");

            if (response.IsSuccessStatusCode)
            {
                isDeleted = true;
            }

            if (!isDeleted)
            {
                return this.BadRequest();
            }

            return RedirectToAction("All", "Employee");
        }
    }
}
