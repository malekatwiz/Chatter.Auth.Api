using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Chatter.Auth.MongoIdentity.Repository
{
    public class MongoRepository : IRepository
    {
        private readonly MongoClient _mongoClient;
        private readonly IMongoDatabase _db;

        public MongoRepository(string connectionString, string dbName)
        {
            _mongoClient = new MongoClient(connectionString);
            _db = _mongoClient.GetDatabase(dbName);
        }

        public Task Insert<T>(T item, CancellationToken cancellationToken)
        {
            var collection = GetCollection<T>();
            return collection.InsertOneAsync(item, cancellationToken: cancellationToken);
        }

        public Task Update<T>(T item, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public IEnumerable<T> All<T>()
        {
            var collection = GetCollection<T>();
            return collection.AsQueryable();
        }

        public T Find<T>(Expression<Func<T, bool>> predicate)
        {
            var collection = GetCollection<T>();
            return collection.AsQueryable().Where(predicate).FirstOrDefault();
        }

        private IMongoCollection<T> GetCollection<T>()
        {
            return _db.GetCollection<T>(typeof(T).Name.ToLower());
        }
    }
}
