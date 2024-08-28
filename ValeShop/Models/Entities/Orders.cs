using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ValeShop.Models.Enum;

namespace ValeShop.Models.Entities
{
    public class Orders
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public DateTime DeliveryDate { get; set; } = DateTime.Now;
        public List<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();


    }
}
