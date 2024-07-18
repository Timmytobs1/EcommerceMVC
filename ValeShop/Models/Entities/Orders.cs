using System.ComponentModel.DataAnnotations.Schema;

namespace ValeShop.Models.Entities
{
    public class Orders
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
      //  public enum Status { get; set; };
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public DateTime DeliveryDate { get; set; } = DateTime.Now;


    }
}
