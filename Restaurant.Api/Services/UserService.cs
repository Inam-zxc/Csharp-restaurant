using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Restaurant.Api.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Restaurant.Api.Settings;
using Restaurant.Api.Services.Interface;

namespace Restaurant.Api.Services
{
    public class UserService : IUserService
    {

        private const string collectionName = "users";
        private readonly IMongoCollection<User> usersCollection;
        private readonly FilterDefinitionBuilder<User> filterBuilder = Builders<User>.Filter;
        public UserService(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(MongoDbSettings.databaseName);
            usersCollection = database.GetCollection<User>(collectionName);
        }

        public async Task CreateUserAsync(User user)
        {
            await usersCollection.InsertOneAsync(user);
        }

        // public async Task DeleteItemAsync(Guid id)
        // {
        //     var filter = filterBuilder.Eq(item => item.Id, id);
        //     await itemsCollection.DeleteOneAsync(filter);

        // }

        // public async Task<Item> GetItemAsync(Guid id)
        // {
        //     var filter = filterBuilder.Eq(item => item.Id, id);
        //     return await itemsCollection.Find(filter).SingleOrDefaultAsync();
        // }

        // public async Task<IEnumerable<Item>> GetItemsAsync()
        // {
        //     return await itemsCollection.Find(new BsonDocument()).ToListAsync();

        // }

        // public async Task UpdatedItemAsync(Item item)
        // {
        //     var filter = filterBuilder.Eq(existingItem => existingItem.Id, item.Id);
        //     await itemsCollection.ReplaceOneAsync(filter, item);
        // }
    }
}