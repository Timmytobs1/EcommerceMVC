using System.ComponentModel.DataAnnotations.Schema;

namespace ValeShop.Models.Entities
{
    public class ProductPromos
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
        public int PromoId { get; set; }
        [ForeignKey("PromoId")]
        public Promos? Promos { get; set; }

    }
}
