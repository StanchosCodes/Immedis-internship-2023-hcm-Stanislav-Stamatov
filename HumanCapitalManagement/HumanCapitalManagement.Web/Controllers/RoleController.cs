using HumanCapitalManagement.Web.ViewModels.Role;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using static HumanCapitalManagement.Common.GeneralConstants;

namespace HumanCapitalManagement.Web.Controllers
{
    public class RoleController : Controller
    {
        private readonly HttpClient apiClient;

        public RoleController(IHttpClientFactory httpClientFactory)
        {
            this.apiClient = httpClientFactory.CreateClient("DbApi");
        }

        public async Task<IActionResult> All()
        {
            IEnumerable<RoleViewModel>? allRoles = await this.apiClient
                .GetFromJsonAsync<IEnumerable<RoleViewModel>>("Role");

            return View(allRoles);
        }

        [HttpGet]
        public IActionResult Add()
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

            AddRoleViewModel roleModel = new AddRoleViewModel();

            return View(roleModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRoleViewModel roleModel)
        {
            if (!ModelState.IsValid)
            {
                return View(roleModel);
            }

            try
            {
                string token = this.HttpContext.Session.GetString("JWT")!;
                this.apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                HttpResponseMessage response = await this.apiClient
                    .PostAsJsonAsync<AddRoleViewModel>("Role", roleModel);

                if (!response.IsSuccessStatusCode)
                {
                    return this.BadRequest();
                }

                return RedirectToAction("All", "Role");
            }
            catch (Exception)
            {
                this.ModelState.AddModelError("", "Failed to create role");

                return View(roleModel);
            }
        }
    }
}
