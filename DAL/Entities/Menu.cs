namespace Restabook.DAL.Entities
{
    public class Menu : Entity
    {
        public string Name { get; set; }
        public List<MenuFood> MenuFoods { get; set; }
    }
}
