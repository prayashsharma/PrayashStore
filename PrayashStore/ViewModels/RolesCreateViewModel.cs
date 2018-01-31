using System.ComponentModel.DataAnnotations;

namespace PrayashStore.ViewModels
{
    public class RolesCreateViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}