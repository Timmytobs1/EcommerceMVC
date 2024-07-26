using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ValeShop.Models.Entities;

namespace ValeShop.Models
{
    public class CategoryViewModel
    {

        [StringLength(30)]
        public string Name { get; set; } = string.Empty;
        public Guid? ParentId { get; set; }
      
    }
}
