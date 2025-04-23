using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelingApp.Domain.Entities
{
    [Table("Users", Schema = "Travel")]
    public class User : IdentityUser
    {
        [Column("IsActive")]
        public bool IsActive { get; set; } = false;

        [Column("UpdateAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("RowVersion")]
        public byte[] RowVersion { get; set; } = [];

    }
}
