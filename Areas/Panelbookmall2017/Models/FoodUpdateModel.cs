namespace Restabook.Areas.admin.Models
{
    public class FoodUpdateModel
    {
        public string Name { get; set; }
        public string About { get; set; }
        public decimal Price { get; set; }
        public string? FoodImage { get; set; }
        public IFormFile? Image { get; set; }

    }
}
