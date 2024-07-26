using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ValeShop.Models.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        [StringLength(30)]
        public string Name { get; set; } = string.Empty;
        public int SubCategory { get; set; } = 0;
        public Guid? ParentId { get; set; }
        public bool Status { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastUpdatedAt { get; set; } = DateTime.Now;
        [ForeignKey("ParentId")]
        public Category? Parent { get; set; }

    }
}
