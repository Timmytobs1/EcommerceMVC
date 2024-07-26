using System.ComponentModel.DataAnnotations.Schema;

namespace ValeShop.Models.Entities
{
    public class ProductPromos
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
        public Guid PromoId { get; set; }
        [ForeignKey("PromoId")]
        public Promos? Promos { get; set; }

    }
}
