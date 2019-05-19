using MongoDB.Driver;
using System.Threading.Tasks;

namespace Chatter.Auth.MongoIdentity.Repository
{
    public abstract class RepositoryBase
    {
        protected readonly MongoClient MongoClient;
        protected readonly IMongoDatabase Db;

        public RepositoryBase(string connectionString, string dbName)
        {
            //var credentials = MongoCredential.CreateCredential(dbName, "root", "example");
            //var settings = new MongoClientSettings
            //{
            //    Credential = credentials,
            //    Server = new MongoServerAddress("localhost", 27018)
            //};

            MongoClient = new MongoClient(connectionString);
            //MongoClient = new MongoClient(settings);
            Db = MongoClient.GetDatabase(dbName);
        }

        protected virtual Task InsertAsync<T>(T item, string collectionName = "")
        {
            var collection = string.IsNullOrEmpty(collectionName) ? GetCollection<T>() :
                GetCollection<T>(collectionName);

            return collection.InsertOneAsync(item);
        }

        protected IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return string.IsNullOrEmpty(collectionName) ? GetCollection<T>() :
                GetCollectionByName<T>(collectionName);
        }

        private IMongoCollection<T> GetCollection<T>()
        {
            return Db.GetCollection<T>(typeof(T).Name.ToLower());
        }

        private IMongoCollection<T> GetCollectionByName<T>(string collectionName)
        {
            return Db.GetCollection<T>(collectionName);
        }
    }
}
