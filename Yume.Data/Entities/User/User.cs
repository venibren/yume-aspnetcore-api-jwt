using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Yume.Attributes;

namespace Yume.Data.Entities.User
{
    public class User
    {
        public User() { }

        public Guid Id { get; } = Guid.NewGuid();

        [ProtectedPersonalData, EmailAddress, MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        public bool EmailVerified { get; set; } = false;

        [Username, MaxLength(32)]
        public string Username { get; set; } = string.Empty;

        [StringLength(32)]
        public string? Nickname { get; set; }

        [StringLength(4)]
        public string Discriminator { get; set; } = "0001";

        public DateTimeOffset CreatedDate { get; private set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset UpdatedDate { get; set; } = DateTimeOffset.UtcNow;

        public bool IsActive { get; set; } = true;

        public virtual UserCustomization Customization { get; set; }

        public virtual Auth.AuthUser Auth { get; set; }

        // Display the combined username & discrimator 
        public override string ToString() => $"{Username}#{Discriminator}";
    }
}
