using System.ComponentModel.DataAnnotations.Schema;

namespace ValeShop.Models.Entities
{
    public class Promos
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsRunning { get; set; } = false;
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
