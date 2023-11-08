using HumanCapitalManagement.Data;
using HumanCapitalManagement.Data.Models;
using HumanCapitalManagement.Web.ViewModels.Town;
using HumanCapitalManagement.Web.ViewModels.Employee;
using HumanCapitalManagement.Services.Data.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace HumanCapitalManagement.Services.Data
{
    public class TownService : ITownService
    {
        private readonly HumanCapitalManagementContext context;

        public TownService(HumanCapitalManagementContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<TownViewModel>> GetAllAsync()
        {
            IEnumerable<TownViewModel> towns = await this.context
                .Towns
                .Where(t => t.IsDeleted == false)
                .Select(t => new TownViewModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    ImgUrl = t.ImgUrl
                })
                .ToListAsync();

            return towns;
        }

        public async Task<TownDetailsViewModel> GetByIdAsync(int id)
        {
            TownDetailsViewModel? town = await this.context
                .Towns
                .Where(t => t.IsDeleted == false && t.Id == id)
                .Select(t => new TownDetailsViewModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    ImgUrl = t.ImgUrl,
                    Employees = t.Employees
                    .Where(e => e.IsEmployed == true)
                    .Select(e => new EmployeeViewModel()
                    {
                        Id = e.Id,
                        FirstName = e.FirstName,
                        MiddleName = e.MiddleName,
                        LastName = e.LastName,
                        Username = e.Username
                    })
                    .ToList()
                })
                .FirstOrDefaultAsync();

            return town!;
        }

        public async Task AddAsync(TownViewModel townModel)
        {
            Town newTown = new Town()
            {
                Name = townModel.Name,
                ImgUrl = townModel.ImgUrl,
                IsDeleted = false
            };

            await this.context.Towns.AddAsync(newTown);
            await this.context.SaveChangesAsync();
        }

        public async Task EditAsync(int id, TownViewModel townModel)
        {
            Town? town = await this.context
                .Towns
                .FirstOrDefaultAsync(t => t.Id == id && t.IsDeleted == false);

            if (town != null)
            {
                town.Name = townModel.Name;
                town.ImgUrl = townModel.ImgUrl;
            }

            await this.context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Town? townToDelete = await this.context
                .Towns
                .Where(t => t.Id == id && t.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (townToDelete == null)
            {
                return false;
            }

            townToDelete.IsDeleted = true;
            await this.context.SaveChangesAsync();

            return true;
        }
    }
}