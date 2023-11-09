using HumanCapitalManagement.Web.ViewModels.Role;
using HumanCapitalManagement.Web.ViewModels.Employee;
using static HumanCapitalManagement.Common.GeneralConstants;

using Microsoft.AspNetCore.Mvc;

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
                return View("Unauthorized");
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
                return View("Unauthorized");
            }

            string token = this.HttpContext.Session.GetString("JWT")!;
            this.apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            EmployeeDetailsViewModel? employeeDetails = new EmployeeDetailsViewModel();

            try
            {
                employeeDetails = await this.apiClient
                    .GetFromJsonAsync<EmployeeDetailsViewModel>($"Employee/{id}");
            }
            catch (Exception)
            {
                return View("NotFound");
            }

            return View(employeeDetails);
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
			string? currentUserName = this.HttpContext.Session.GetString("Username");

			if (!string.IsNullOrEmpty(currentUserName))
			{
				return RedirectToAction("All", "Employee");
			}

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
                    return View("BadRequest");
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
			string? currentUserName = this.HttpContext.Session.GetString("Username");

			if (!string.IsNullOrEmpty(currentUserName))
			{
				return RedirectToAction("All", "Employee");
			}

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
                return View("Unauthorized");
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
                return View("Unauthorized");
            }

            EditViewModel? editModel = new EditViewModel();

            try
            {
                editModel = await this.apiClient
                    .GetFromJsonAsync<EditViewModel>($"Employee/GenerateEditModel/{id}");
            }
            catch (Exception)
            {
                return View("NotFound");
            }

            string currentUserRole = this.HttpContext.Session.GetString("Role")!;

            bool isEditModelAdmin = await this.apiClient.GetFromJsonAsync<bool>($"Role/IsAdmin/{editModel!.Username}");

            if (currentUserRole == EmployeeRoleName)
            {
                if (currentUserName == editModel!.Username)
                {
                    return View(editModel);
                }

                return View("Forbidden");
            }
            else
            {
                if (currentUserName == editModel!.Username || isEditModelAdmin == false)
                {
                    return View(editModel);
                }
            }

            return View("Forbidden");
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
                return View("BadRequest");
            }

            return RedirectToAction("Details", "Employee", new { id = id });
        }

        public async Task<IActionResult> MakeAdmin(int id)
        {
            string currentUserRole = this.HttpContext.Session.GetString("Role")!;
            string currentUserName = this.HttpContext.Session.GetString("Username")!;

            if (String.IsNullOrEmpty(currentUserName))
            {
                return View("Unauthorized");
            }

            if (currentUserRole != AdminRoleName)
            {
                return View("Forbidden");
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
                return View("Unauthorized");
            }

            if (currentUserRole != AdminRoleName)
            {
                return View("Forbidden");
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
                return View("BadRequest");
            }

            return RedirectToAction("All", "Employee");
        }
    }
}
