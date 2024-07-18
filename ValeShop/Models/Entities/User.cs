using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ValeShop.Models.Entities
{
    [Index("PhoneNumber", IsUnique = true)]
    [Index("Email", IsUnique = true)]
    public class User
    {

        public int Id { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        [StringLength(50)]
        public string UserName { get; set; } = string.Empty;
        [StringLength(13)]
        public string PhoneNumber { get; set; } = string.Empty;
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        [StringLength(25)]
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
