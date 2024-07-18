using System.ComponentModel.DataAnnotations.Schema;

namespace ValeShop.Models.Entities
{
    public class States
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;    
        public int Zip { get; set; }
        public int CountryId { get; set; }
        [ForeignKey("CountryId")]
        public Country? Country { get; set; }
    }
}
