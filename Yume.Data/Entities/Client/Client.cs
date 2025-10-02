using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Yume.Data.Entities.Client
{
    public class Client
    {
        public Guid Id { get; } = Guid.NewGuid();

        [MaxLength(32)]
        public required string Name { get; set; } = string.Empty;

        [MaxLength(512)]
        public string? Description { get; set; }

        [ProtectedPersonalData]
        public required string Secret { get; set; }

        public DateTime CreatedDate { get; private set; } = DateTime.UtcNow;

        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        public required Guid OwnerId { get; set; }
    }
}
