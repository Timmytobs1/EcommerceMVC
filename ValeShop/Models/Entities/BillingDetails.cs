using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ValeShop.Models.Entities
{
    public class BillingDetails
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
        public string CompanyName { get; set; }=string.Empty;
        [StringLength(13)]
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public Guid StateId { get; set; }
        [ForeignKey("StateId")]
        public States? States { get; set; }
        public bool IsActive { get; set; } = false;
    }
}
