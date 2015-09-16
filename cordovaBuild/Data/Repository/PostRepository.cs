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
    public class PostRepository : EntityService<Space>, IPostRepository
    {
        public async Task<List<Space>> GetSpacesByUser(string username)
        {
            var builder = Builders<Space>.Filter;
            var filter = builder.Eq("User", username);
            var spaces = await this.ConnectionHandler.MongoCollection.Find(filter).ToListAsync();
            return spaces;
        }

        public async Task<bool> UpdatePhoto(string id, string path)
        {
            var builder = Builders<Space>.Filter;
            var filter = builder.Eq("_id", ObjectId.Parse(id));
            var space = await this.ConnectionHandler.MongoCollection.Find(filter).FirstOrDefaultAsync();
            var update = Builders<Space>.Update
            .Set("Photopath", path)
            .CurrentDate("lastModified");
            await this.ConnectionHandler.MongoCollection.UpdateOneAsync(filter, update);
            return true;
        }
    }
}
