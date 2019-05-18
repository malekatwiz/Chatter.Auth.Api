using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chatter.Auth.MongoIdentity.Repository
{
    public interface IRoleRepository
    {
        Task Create(IdentityRole role);
        Task Delete(IdentityRole role);
        Task Update(IdentityRole role);
        Task<IdentityRole> FindByName(string roleName);
        Task<IdentityRole> FindById(string id);
        Task<IEnumerable<IdentityRole>> GetAll();
    }
}
