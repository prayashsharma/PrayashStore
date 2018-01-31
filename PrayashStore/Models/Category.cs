using System.ComponentModel.DataAnnotations;

namespace PrayashStore.Models
{
    public class Category
    {
        public int Id { get; set; }

        [StringLength(255)]
        [Required]
        public string Name { get; set; }
    }
}