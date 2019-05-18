using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Chatter.Auth.MongoIdentity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IList<string> Roles { get; set; }
        public IList<IdentityUserLogin<string>> Logins { get; set; }

        public ApplicationUser()
        {
            Roles = new List<string>();
            Logins = new List<IdentityUserLogin<string>>();
        }
    }
}
