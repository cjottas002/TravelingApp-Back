using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelingApp.Domain.Entities
{
    [Table("Users", Schema = "Travel")]
    public class User : IdentityUser
    {
        [Column("IsActive")]
        public bool IsActive { get; set; } = false;
    }
}
