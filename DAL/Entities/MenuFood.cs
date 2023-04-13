namespace Restabook.DAL.Entities
{
    public class MenuFood : Entity
    {
        public int MenuId { get; set; }
        public Menu Menu  { get; set; }
        public int FoodId { get; set; }
        public Food Food { get; set; }
    }
}
