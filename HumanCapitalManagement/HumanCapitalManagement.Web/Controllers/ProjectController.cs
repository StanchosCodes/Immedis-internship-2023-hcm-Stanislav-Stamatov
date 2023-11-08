using HumanCapitalManagement.Web.ViewModels.Project;
using static HumanCapitalManagement.Common.GeneralConstants;

using Microsoft.AspNetCore.Mvc;

namespace HumanCapitalManagement.Web.Controllers
{
    public class ProjectController : Controller
    {
        private readonly HttpClient apiClient;

        public ProjectController(IHttpClientFactory httpClientFactory)
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

            IEnumerable<ProjectViewModel>? allProjects = await this.apiClient
                .GetFromJsonAsync<IEnumerable<ProjectViewModel>>("Project");

            return View(allProjects);
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

            ProjectDetailsViewModel? projectDetails = new ProjectDetailsViewModel();

            try
            {
                projectDetails = await this.apiClient
                    .GetFromJsonAsync<ProjectDetailsViewModel>($"Project/{id}");
            }
            catch (Exception)
            {
                return View("NotFound");
            }

            return View(projectDetails);
        }

        [HttpGet]
        public IActionResult Add()
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

            AddEditProjectViewModel projectModel = new AddEditProjectViewModel();

            return View(projectModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEditProjectViewModel projectModel)
        {
            if (!ModelState.IsValid)
            {
                return View(projectModel);
            }

            try
            {
                string token = this.HttpContext.Session.GetString("JWT")!;
                this.apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                HttpResponseMessage response = await this.apiClient.PostAsJsonAsync("Project", projectModel);

                if (!response.IsSuccessStatusCode)
                {
                    return View("BadRequest");
                }

                return RedirectToAction("All", "Project");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Failed to add project!");

                return View(projectModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
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

            ProjectDetailsViewModel? project = new ProjectDetailsViewModel();

            try
            {
                project = await this.apiClient
                    .GetFromJsonAsync<ProjectDetailsViewModel>($"Project/{id}");
            }
            catch (Exception)
            {
                return View("NotFound");
            }

            AddEditProjectViewModel projectModel = new AddEditProjectViewModel()
            {
                Title = project!.Title,
                Description = project.Description,
                Salary = project.Salary,
                ImgUrl = project.ImgUrl,
                StartDate = DateTime.Parse(project.StartDate),
                EndDate = DateTime.Parse(project.EndDate)
            };

            return View(projectModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AddEditProjectViewModel projectModel)
        {
            if (!ModelState.IsValid)
            {
                return View(projectModel);
            }

            string token = this.HttpContext.Session.GetString("JWT")!;
            this.apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            HttpResponseMessage response = await this.apiClient
                .PutAsJsonAsync<AddEditProjectViewModel>($"Project/{id}", projectModel);

            if (!response.IsSuccessStatusCode)
            {
                return View("BadRequest");
            }

            return RedirectToAction("Details", "Project", new { id = id });
        }

        public async Task<IActionResult> Assign(int id)
        {
            string? currentUsername = this.HttpContext.Session.GetString("Username");

            if (String.IsNullOrEmpty(currentUsername))
            {
                return View("Unauthorized");
            }

            string token = this.HttpContext.Session.GetString("JWT")!;
            this.apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            HttpResponseMessage response = await this.apiClient.PostAsJsonAsync<string>("Employee/GetId", currentUsername);

            if (!response.IsSuccessStatusCode)
            {
                return View("BadRequest");
            }

            int employeeId = await response.Content.ReadFromJsonAsync<int>();

            HttpResponseMessage assignResponse = await this.apiClient.PostAsJsonAsync<int>($"Project/Assign/{id}", employeeId);

            if (!assignResponse.IsSuccessStatusCode)
            {
                return View("BadRequest");
            }

            return RedirectToAction("AllAssigned", "Project");
        }

        public async Task<IActionResult> Leave(int id)
        {
            string? currentUsername = this.HttpContext.Session.GetString("Username");

            if (String.IsNullOrEmpty(currentUsername))
            {
                return View("Unauthorized");
            }

            string token = this.HttpContext.Session.GetString("JWT")!;
            this.apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            HttpResponseMessage response = await this.apiClient.PostAsJsonAsync<string>("Employee/GetId", currentUsername);

            if (!response.IsSuccessStatusCode)
            {
                return View("BadRequest");
            }

            int employeeId = await response.Content.ReadFromJsonAsync<int>();

            HttpResponseMessage leaveResponse = await this.apiClient.PostAsJsonAsync<int>($"Project/Leave/{id}", employeeId);

            if (!leaveResponse.IsSuccessStatusCode)
            {
                return View("BadRequest");
            }

            return RedirectToAction("AllAssigned", "Project");
        }

        public async Task<IActionResult> AllAssigned()
        {
            string? currentUsername = this.HttpContext.Session.GetString("Username");

            if (String.IsNullOrEmpty(currentUsername))
            {
                return View("Unauthorized");
            }

            string token = this.HttpContext.Session.GetString("JWT")!;
            this.apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            HttpResponseMessage response = await this.apiClient.PostAsJsonAsync<string>("Employee/GetId", currentUsername);

            if (!response.IsSuccessStatusCode)
            {
                return View("BadRequest");
            }

            int employeeId = await response.Content.ReadFromJsonAsync<int>();

            HttpResponseMessage projectsResponse = await this.apiClient.PostAsJsonAsync<int>("Project/AllAssigned", employeeId);

            if (!projectsResponse.IsSuccessStatusCode)
            {
                return View("BadRequest");
            }

            IEnumerable<ProjectViewModel>? allAssignedProjects = await projectsResponse
                .Content
                .ReadFromJsonAsync<IEnumerable<ProjectViewModel>>();

            return View(allAssignedProjects);
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

            HttpResponseMessage response = await this.apiClient.DeleteAsync($"Project/{id}");

            if (response.IsSuccessStatusCode)
            {
                isDeleted = true;
            }

            if (!isDeleted)
            {
                return View("BadRequest");
            }

            return RedirectToAction("All", "Project");
        }
    }
}
