using System.ComponentModel;
using System;
using Microsoft.AspNetCore.Identity;

namespace WeavyChat.Entities
{
    public class User
    {
        public enum Status : byte
        {
            [Description("PENDING")] Pending = 0,
            [Description("ACTIVE")] Active = 1,
            [Description("INACTIVE")] Inactive = 2,
            [Description("ARCHIVE")] Archive = 3
        }

        public string UserName { get; set; } = null!;
        public Status Statum { get; set; }
        public string EmailAddress { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string Password { get; set; } = null!;
        //public DateTime? LastLoginAt { get; set; }
        //public short FailedLogins { get; set; }
        //public bool LoginLocked { get; set; }
        //public DateTime? LoginLockedAt { get; set; }
        //public KSUid OrgId { get; set; } = null!;
        public string? OrgRoleName { get; set; }
        //public Portals Portal { get; set; }
        //public SystemRoles SystemRole { get; set; }
        //public KSUid? OrgRole { get; set; }
        //public string LangId { get; set; } = null!;
        //public KSUid? LocationId { get; set; }
        //public KSUid? UnitId { get; set; }
        //public string? MobilePhone { get; set; }
        //public string? SecondaryPhone { get; set; }
        //public KSUid? ModifiedBy { get; set; }
        //public DateTime? ModifiedAt { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string? EmailToken { get; set; }
        //public string VerificationCode { get; set; }
        //public DateTime VerificationCodeCreatedAt { get; set; }
        //public DateTime? EmailVerificationAt { get; set; }
    }
}
