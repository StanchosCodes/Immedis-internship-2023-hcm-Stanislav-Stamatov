using HumanCapitalManagement.Web.ViewModels.Town;
using HumanCapitalManagement.Services.Data.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HumanCapitalManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TownController : ControllerBase
    {
        private readonly ITownService townService;

        public TownController(ITownService townService)
        {
            this.townService = townService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<TownViewModel> towns = await this.townService.GetAllAsync();

            return this.Ok(towns);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            TownDetailsViewModel town = await this.townService.GetByIdAsync(id);

            return this.Ok(town);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody]TownViewModel townModel)
        {
            await this.townService.AddAsync(townModel);

            return this.Ok(townModel);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [FromBody]TownViewModel townModel)
        {
            await this.townService.EditAsync(id, townModel);

            return this.Ok(townModel);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            bool isDeleted = await this.townService.DeleteAsync(id);

            return this.Ok(isDeleted);
        }
    }
}
