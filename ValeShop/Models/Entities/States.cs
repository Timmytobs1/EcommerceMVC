using System.ComponentModel.DataAnnotations.Schema;

namespace ValeShop.Models.Entities
{
    public class States
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;    
        public string Zip { get; set; }
        public Guid CountryId { get; set; }
        [ForeignKey("CountryId")]
        public Country? Country { get; set; }
    }
}
