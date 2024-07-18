using System.ComponentModel.DataAnnotations.Schema;

namespace ValeShop.Models.Entities
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Orders? Orders { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Price { get; set; }
        public int BillingDetailsId { get; set; }
        [ForeignKey("BillingDetailsId")]
        public BillingDetails? BillingDetails { get; set; }
        public int ShippingId { get; set; }

    }
}
