using Microsoft.AspNetCore.Identity;

namespace WeavyChat
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string CustomTag { get; set; }
    }
}
