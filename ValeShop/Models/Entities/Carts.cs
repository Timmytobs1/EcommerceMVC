using System.ComponentModel.DataAnnotations.Schema;

namespace ValeShop.Models.Entities
{
    public class Carts
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
