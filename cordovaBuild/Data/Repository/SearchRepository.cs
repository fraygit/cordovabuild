using MongoDB.Bson;
using MongoDB.Driver;
using cordovaBuild.Data.Entities.Service;
using cordovaBuild.Data.Interface;
using cordovaBuild.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cordovaBuild.Data.Repository
{
    public class SearchRepository : EntityService<Search>, ISearchRepository
    {
        public async void AssociateAddress(string id, string user)
        {
            var filter = Builders<Search>.Filter.Eq("_id", ObjectId.Parse(id));
            var update = Builders<Search>.Update
                .Set("Username", user)
                .CurrentDate("lastModified");
            var result = this.ConnectionHandler.MongoCollection.UpdateOneAsync(filter, update);
        }

        public async Task<List<Search>> GetAllSearches()
        {
            var builder = Builders<Search>.Filter;
            var filter = builder.Gt("Radius", 0);
            var searches = await this.ConnectionHandler.MongoCollection.Find(new BsonDocument()).ToListAsync();
            return searches;
        }
    }
}
