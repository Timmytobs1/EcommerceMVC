using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ValeShop.Models.Entities
{
    public class Reviews
    {
        public int Id { get; set; }
        [StringLength(30)]
        public string Name { get; set; } = string.Empty;
        [StringLength(150)]
        public string Content { get; set; } = string.Empty;
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        public int Rating { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
