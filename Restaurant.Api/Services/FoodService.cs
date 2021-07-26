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
    public class FoodService : IFoodService
    {

        private const string collectionName = "foods";
        private readonly IMongoCollection<Food> foodsCollection;
        private readonly FilterDefinitionBuilder<Food> filterBuilder = Builders<Food>.Filter;
        public FoodService(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(MongoDbSettings.databaseName);
            foodsCollection = database.GetCollection<Food>(collectionName);
        }

        public async Task CreateFoodAsync(Food food)
        {
            await foodsCollection.InsertOneAsync(food);
        }

        public async Task<Food> GetAllFoodsAsync(string name = null)
        {
            var filterConditions = new List<FilterDefinition<Food>>();
            if (!string.IsNullOrWhiteSpace(name))
            {
                var namefilter = filterBuilder.Where(food => food.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
                filterConditions.Add(namefilter);
            }

            var filter = filterBuilder.Or(filterConditions);
            return await foodsCollection.Find(filter).SingleOrDefaultAsync();
        }


        public async Task DeleteFoodAsync(Guid id)
        {
            var filter = filterBuilder.Eq(food => food.Id, id);
            await foodsCollection.DeleteOneAsync(filter);

        }

        public async Task<Food> GetItemByIdAsync(Guid id)
        {
            var filter = filterBuilder.Eq(food => food.Id, id);
            return await foodsCollection.Find(filter).SingleOrDefaultAsync();
        }


        public async Task UpdatedFoodAsync(Food food)
        {
            var filter = filterBuilder.Eq(existingFood => existingFood.Id, food.Id);
            await foodsCollection.ReplaceOneAsync(filter, food);
        }
    }
}