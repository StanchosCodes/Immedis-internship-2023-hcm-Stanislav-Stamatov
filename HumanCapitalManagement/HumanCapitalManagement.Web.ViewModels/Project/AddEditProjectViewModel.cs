using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HumanCapitalManagement.Common.ModelValidationConstants.Project;

namespace HumanCapitalManagement.Web.ViewModels.Project
{
    public class AddEditProjectViewModel
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Required]
        [Display(Name = "Image Url")]
        public string ImgUrl { get; set; } = null!;

        [Required]
        public decimal Salary { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
    }
}
