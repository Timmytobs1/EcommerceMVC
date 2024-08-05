using ValeShop.Models.Entities;

namespace ValeShop.Models
{
    public class UserProductsCategoriesCartsViewModel
    {
        public User User { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public Product Product { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Carts> Carts { get; set; }
        public Carts Cart { get; set; }
    }
}
