using HumanCapitalManagement.Data.Models;
using HumanCapitalManagement.Web.ViewModels.Town;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Services.Data.Interfaces
{
    public interface ITownService
    {
        Task<IEnumerable<TownViewModel>> GetAllAsync();

        Task<TownDetailsViewModel> GetByIdAsync(int id);

        Task AddAsync(TownViewModel townModel);

        Task EditAsync(int id, TownViewModel townModel);

        Task<bool> DeleteAsync(int id);
    }
}
