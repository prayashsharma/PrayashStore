using PrayashStore.Models;
using System.Collections.Generic;

namespace PrayashStore.ViewModels
{
    public class MenuViewModel
    {
        public MenuViewModel(string gender, ICollection<Category> categories)
        {
            Gender = gender;
            Categories = categories;
        }
        public string Gender { get; set; }
        public ICollection<Category> Categories { get; set; }
    }

}