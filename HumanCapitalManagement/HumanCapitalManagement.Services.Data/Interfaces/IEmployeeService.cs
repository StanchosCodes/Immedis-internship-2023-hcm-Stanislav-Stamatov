using HumanCapitalManagement.Web.ViewModels.Employee;

namespace HumanCapitalManagement.Services.Data.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeViewModel>> GetAllAsync();

        Task<EmployeeDetailsViewModel> GetByIdAsync(int id);

        Task<int> GetIdByUsername(string username);

        Task<bool> RegisterAsync(RegisterViewModel registerModel);

        Task<string> LoginAsync(LoginViewModel loginModel);

        Task<EditViewModel> GenerateEditViewModelAsync(int id);

        Task EditAsync(int id, EditViewModel editModel);

        Task<bool> DeleteAsync(int id);

        Task<RegisterHelperViewModel> FillRegisterCollectionsAsync();

        Task MakeAdminAsync(int id);
    }
}
