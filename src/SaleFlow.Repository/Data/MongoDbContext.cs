using MongoDB.Driver;
using SaleFlow.Repository.Configurations;

namespace SaleFlow.Repository.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(MongoDbOptions options)
        {
            var client = new MongoClient(options.ConnectionString);
            _database = client.GetDatabase(options.DatabaseName);
        }

        // Expose collections as needed. For example, if you want to store Sales documents:
        public IMongoCollection<T> GetCollection<T>(string name) =>
            _database.GetCollection<T>(name);
    }
}
