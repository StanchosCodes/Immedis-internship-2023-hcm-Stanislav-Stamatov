using HumanCapitalManagement.Web.ViewModels.Town;
using HumanCapitalManagement.Web.ViewModels.Department;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static HumanCapitalManagement.Common.ModelValidationConstants.Employee;

namespace HumanCapitalManagement.Web.ViewModels.Employee
{
    public class RegisterViewModel
    {
        public RegisterViewModel()
        {
            this.Departments = new HashSet<DepartmentViewModel>();
            this.Managers = new HashSet<EmployeeViewModel>();
            this.Towns = new HashSet<TownViewModel>();
        }

        [Required]
        [StringLength(UserNameMaxLength, MinimumLength = UserNameMinLength, ErrorMessage = "The {0} must be at least {2} and maximum {1} characters long.")]
        public string Username { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(EmailMaxLength, MinimumLength = EmailMinLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
        public string Email { get; set; } = null!;

        [Required]
        [Display(Name = "First Name")]
        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
        public string FirstName { get; set; } = null!;

        [Required]
        [Display(Name = "Middle Name")]
        [StringLength(MiddleNameMaxLength, MinimumLength = MiddleNameMinLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
        public string MiddleName { get; set; } = null!;

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
        public string LastName { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = null!;

        [Required]
        public int Age { get; set; }

        [Required]
        [Display(Name = "Job title")]
        public string JobTitle { get; set; } = null!;

        [Required]
        [Display(Name = "Image Url")]
        public string ImgUrl { get; set; } = null!;

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public int ManagerId { get; set; }

        [Required]
        public int TownId { get; set; }

        public virtual IEnumerable<TownViewModel> Towns { get; set; }

        public virtual IEnumerable<DepartmentViewModel> Departments { get; set; }

        public virtual IEnumerable<EmployeeViewModel> Managers { get; set; }
    }
}
