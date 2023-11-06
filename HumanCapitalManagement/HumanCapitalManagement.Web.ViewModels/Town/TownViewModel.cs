using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HumanCapitalManagement.Common.ModelValidationConstants.Town;

namespace HumanCapitalManagement.Web.ViewModels.Town
{
    public class TownViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [Display(Name = "Image Url")]
        public string ImgUrl { get; set; } = null!;
    }
}
