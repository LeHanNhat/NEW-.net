
using Microsoft.AspNetCore.Identity;

namespace bookingflightmvcUI.Data
{
        public class ApplicationUser : IdentityUser
        {
            public string? Name { get; set; }
            public string? ProfilePicture { get; set; }

            public string? Biography { get; set; }
    }
    
}
