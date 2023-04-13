using Microsoft.AspNetCore.Mvc.Rendering;

namespace Restabook.Areas.admin.Models
{
    public class MenuUpdateModel
    {
        public string Name { get; set; }
        public IFormFile? Image { get; set; }
        public List<SelectListItem>? Foods { get; set; } = new();
        public List<int> FoodsId { get; set; } = new();
    }
}
