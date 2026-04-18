using System;
using TMS.Domain.Entities.Bases;
using TMS.Domain.Entities.People;

namespace TMS.Domain.Entities.Users
{
    public class User : BaseEntity
    { 
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Removed the Audit Class (No longer needed as per the new requirements)
        public int CreatedByUserId { get; set; }
        public virtual User CreatedByUser { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int PersonId { get; set; }
        public virtual Person Person { get; set; } = null!;
    }
}
