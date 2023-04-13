namespace Restabook.DAL.Entities
{
    public class Food : Entity
    {
        public string Name { get; set; }
        public string About { get; set; }
        public decimal Price { get; set; }
        public string FoodImage { get; set; }
        public List<MenuFood> MenuFoods { get; set; }
    }
}
