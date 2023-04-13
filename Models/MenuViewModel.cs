using Restabook.DAL.Entities;

namespace Restabook.Models
{
    public class MenuViewModel
    {
        public List<Food> Foods { get; set; } = new List<Food>();
        public List<Menu> Menus { get; set; } = new List<Menu>();
        public Menu Menu { get; set; }
        public List<MenuFood> MenuFoods { get; set; } = new List<MenuFood>();
    }
}
