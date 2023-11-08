using HumanCapitalManagement.Web.ViewModels.Town;
using static HumanCapitalManagement.Common.GeneralConstants;

using Microsoft.AspNetCore.Mvc;

namespace HumanCapitalManagement.Web.Controllers
{
    public class TownController : Controller
    {
        private readonly HttpClient apiClient;

        public TownController(IHttpClientFactory httpClientFactory)
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

            IEnumerable<TownViewModel>? townsList = await this.apiClient.GetFromJsonAsync<List<TownViewModel>>("town");

            return View(townsList);
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

            TownDetailsViewModel? townDetails = new TownDetailsViewModel();

            try
            {
                townDetails = await this.apiClient
                    .GetFromJsonAsync<TownDetailsViewModel>($"town/{id}");
            }
            catch (Exception)
            {
                return View("NotFound");
            }

            return View(townDetails);
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

            TownViewModel townModel = new TownViewModel();

            return View(townModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(TownViewModel townModel)
        {
            if (!ModelState.IsValid)
            {
                return View(townModel);
            }

            try
            {
                string token = this.HttpContext.Session.GetString("JWT")!;
                this.apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                HttpResponseMessage response = await this.apiClient.PostAsJsonAsync("town", townModel);

                if (!response.IsSuccessStatusCode)
                {
                    return View("BadRequest");
                }

                return RedirectToAction("All", "Town");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Failed to add town!");

                return View(townModel);
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

            TownDetailsViewModel? townToEdit = new TownDetailsViewModel();

            try
            {
                townToEdit = await this.apiClient
                    .GetFromJsonAsync<TownDetailsViewModel>($"town/{id}");
            }
            catch (Exception)
            {
                return View("NotFound");
            }

            TownViewModel townModel = new TownViewModel()
            {
                Name = townToEdit!.Name,
                ImgUrl = townToEdit.ImgUrl
            };

            return View(townModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TownViewModel townModel)
        {
            if (!ModelState.IsValid)
            {
                return View(townModel);
            }

            string token = this.HttpContext.Session.GetString("JWT")!;
            this.apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            HttpResponseMessage editResponse = await this.apiClient.PutAsJsonAsync<TownViewModel>($"town/{id}", townModel);

            if (!editResponse.IsSuccessStatusCode)
            {
                return View("BadRequest");
            }

            return RedirectToAction("Details", "Town", new {id = townModel.Id});
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

            HttpResponseMessage response = await this.apiClient.DeleteAsync($"town/{id}");

            if (response.IsSuccessStatusCode)
            {
                isDeleted = true;
            }

            if (!isDeleted)
            {
                return View("BadRequest");
            }

            return RedirectToAction("All", "Town");
        }
    }
}
