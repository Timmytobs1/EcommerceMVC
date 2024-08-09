using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ValeShop.Models.Entities;

namespace ValeShop.Models
{
    public class BillingDetailsViewModel
    {

        public string CompanyName { get; set; } = string.Empty;
        [StringLength(13)]
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;
        public string State { get; set; } =string.Empty;
        public string Country { get; set; } = string.Empty;
      
    }
}
