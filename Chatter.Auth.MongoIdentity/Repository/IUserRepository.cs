using Chatter.Auth.MongoIdentity.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chatter.Auth.MongoIdentity.Repository
{
    public interface IUserRepository
    {
        Task Create(ApplicationUser user);
        Task Update(ApplicationUser user);
        Task Delete(ApplicationUser user);
        Task<ApplicationUser> FindById(string id);
        Task<ApplicationUser> FindByUsername(string username);
        Task<IEnumerable<ApplicationUser>> FindUsersInRole(string roleName);
        Task<ApplicationUser> FindByLogin(string loginProvider, string providerKey);
    }
}
