using Restabook.DAL.Entities;

namespace Restabook.Areas.admin.Models
{
    public class FoodCreateModel
    {
        public string Name { get; set; }
        public string About { get; set; }
        public decimal Price { get; set; }
        public IFormFile Image { get; set; }

    }
}
