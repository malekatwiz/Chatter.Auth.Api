using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chatter.Auth.MongoIdentity.Entities;
using MongoDB.Driver;

namespace Chatter.Auth.MongoIdentity.Repository
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        private const string CollectionName = "Users";

        public UserRepository(string connectionString, string dbName) : base(connectionString, dbName)
        {
        }

        public Task Create(ApplicationUser user)
        {
            return InsertAsync(user, CollectionName);
        }

        public Task Delete(ApplicationUser user)
        {
            var collection = GetCollection<ApplicationUser>(CollectionName);
            return collection.DeleteOneAsync(x => x.Id.Equals(user.Id.ToLower()));
        }

        public async Task<ApplicationUser> FindById(string id)
        {
            var collection = GetCollection<ApplicationUser>(CollectionName);
            return await (await collection.FindAsync(x => x.Id.Equals(id.ToLower()))).FirstOrDefaultAsync();
        }

        public async Task<ApplicationUser> FindByUsername(string username)
        {
            var collection = GetCollection<ApplicationUser>(CollectionName);
            return await(await collection.FindAsync(x => x.UserName.Equals(username.ToLower()))).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> FindUsersInRole(string roleName)
        {
            var collection = GetCollection < ApplicationUser>(CollectionName);
            var filter = Builders<ApplicationUser>.Filter.AnyEq(x => x.Roles, roleName);
            var users = await collection.FindAsync(filter);

            return users.ToEnumerable();
        }

        public async Task<ApplicationUser> FindByLogin(string loginProvider, string providerKey)
        {
            var collection = GetCollection<ApplicationUser>(CollectionName);
            return await (await collection.FindAsync(x => x.Logins.Any(l => l.LoginProvider.Equals(loginProvider) && l.ProviderKey.Equals(providerKey))))
                .FirstOrDefaultAsync();
        }

        public Task Update(ApplicationUser user)
        {
            var collection = GetCollection<ApplicationUser>(CollectionName);
            return collection.ReplaceOneAsync(x => x.Id.Equals(user.Id), user);
        }
    }
}
