using HumanCapitalManagement.Web.ViewModels.Role;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Web.ViewModels.Employee
{
    public class LoginViewModel
    {
        public LoginViewModel()
        {
            this.Roles = new HashSet<RoleViewModel>();
        }

        public string Username { get; set; } = null!;

        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public int RoleId { get; set; }

        public virtual IEnumerable<RoleViewModel> Roles { get; set; }
    }
}
