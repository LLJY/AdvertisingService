using System.Collections.Generic;
using System.Threading.Tasks;
using AdvertisingService.Models;
using MongoDB.Driver;

namespace AdvertisingService.Services
{
    public class AdvertisementsService
    {
        private readonly IMongoCollection<Advertisement> _advertisements;

        public AdvertisementsService(IAdvertisementDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _advertisements = database.GetCollection<Advertisement>(settings.CollectionName);
        }
        /**
         * Adds a new token
         */
        public async Task<Advertisement> CreateAsync(Advertisement token)
        {
            await _advertisements.InsertOneAsync(token);
            return token;
        }
        
        /**
         * Get all the user tokens regardless
         */
        public async Task<List<Advertisement>> GetAllAdvertsAsync()
        {
            return await (await _advertisements.FindAsync(token=>true)).ToListAsync();
        }
        /**
         * Upserts item in database that matches the id with the object
         * *Upsert = insert if not exist, update if exists
         * <param name="token">The token key value pair to upsert</param>
         */
        public async Task UpsertAsync(Advertisement token)
        {
            await _advertisements.ReplaceOneAsync(userToken => userToken.Id == token.Id, token, new ReplaceOptions
            {
                IsUpsert = true
            });
        }

        public async Task DeleteByIdAsync(string id)
        {
            await _advertisements.DeleteOneAsync(ad => ad.Id == id);
        }
    }
}