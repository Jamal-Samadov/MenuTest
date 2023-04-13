using Microsoft.AspNetCore.Mvc.Rendering;

namespace Restabook.Areas.admin.Models
{
    public class MenuCreateModel
    {
        public string Name { get; set; }
        public List<SelectListItem>? Foods { get; set; }
        public List<int> FoodsId { get; set; }

    }
}
