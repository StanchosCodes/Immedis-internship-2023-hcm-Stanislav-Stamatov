using System.ComponentModel.DataAnnotations;
using static HumanCapitalManagement.Common.ModelValidationConstants.Role;

namespace HumanCapitalManagement.Web.ViewModels.Role
{
    public class AddRoleViewModel
    {
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
        public string Name { get; set; } = null!;
    }
}
