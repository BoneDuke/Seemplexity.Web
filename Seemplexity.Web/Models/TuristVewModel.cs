using System.ComponentModel.DataAnnotations;

namespace Seemplexity.Web.Models
{
    public class TuristViewModel
    {
        [Required]
        [Display(Name = "PassportSeria", ResourceType = typeof(Resources.Resources))]
        public string PassportSeria { get; set; }

        [Required]
        [Display(Name = "PassportNumber", ResourceType = typeof(Resources.Resources))]
        public string PassportNumber { get; set; }

        [Required]
        [Display(Name = "Surname", ResourceType = typeof(Resources.Resources))]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(Resources.Resources))]
        public string Name { get; set; }

        public string PlaceNumber { get; set; }
    }
}