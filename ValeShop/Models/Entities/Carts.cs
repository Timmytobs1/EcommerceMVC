using System.ComponentModel.DataAnnotations.Schema;

namespace ValeShop.Models.Entities
{
    public class Carts
    {
        public Guid Id { get; set; }
        public string SessionId { get; set; }
        public int Quantity { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
