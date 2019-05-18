using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace Chatter.Auth.MongoIdentity.Repository
{
    public class RoleRepository : RepositoryBase, IRoleRepository
    {
        private const string CollectionName = "Roles";

        public RoleRepository(string connectionString, string dbName) : base(connectionString, dbName)
        {
        }

        public Task Create(IdentityRole role)
        {
            return InsertAsync(role, CollectionName);
        }

        public Task Update(IdentityRole role)
        {
            var collection = GetCollection<IdentityRole>(CollectionName);
            return collection.ReplaceOneAsync(x => x.Id.Equals(role.Id), role);
        }

        public async Task<IdentityRole> FindByName(string roleName)
        {
            var collection = GetCollection<IdentityRole>(CollectionName);
            return await (await collection.FindAsync(x => x.Name.Equals(roleName))).FirstOrDefaultAsync();
        }

        public async Task<IdentityRole> FindById(string id)
        {
            var collection = GetCollection<IdentityRole>(CollectionName);
            return await (await collection.FindAsync(x => x.Id.Equals(id))).FirstOrDefaultAsync();
        }

        public Task<IEnumerable<IdentityRole>> GetAll()
        {
            var collection = GetCollection<IdentityRole>(CollectionName);
            return Task.FromResult(collection.AsQueryable().ToEnumerable());
        }

        public Task Delete(IdentityRole role)
        {
            var collection = GetCollection< IdentityRole>(CollectionName);
            return collection.DeleteOneAsync(x => x.Id.Equals(role.Id));
        }
    }
}
