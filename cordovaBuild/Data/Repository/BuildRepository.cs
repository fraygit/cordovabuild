using cordovaBuild.Data.Entities.Service;
using cordovaBuild.Data.Interface;
using cordovaBuild.Data.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cordovaBuild.Data.Repository
{
    public class BuildRepository : EntityService<Build>, IBuildRepository
    {
        public async Task<bool> UpdateStatus(string status, string buildId, int buildTimeSec)
        {
            var builder = Builders<Build>.Filter;
            var filter = builder.Eq("_id", ObjectId.Parse(buildId));
            var build = await this.ConnectionHandler.MongoCollection.Find(filter).FirstOrDefaultAsync();
            var update = Builders<Build>.Update
            .Set("Status", status).Set("BuildTimeSec", buildTimeSec)
            .CurrentDate("lastModified");
            await this.ConnectionHandler.MongoCollection.UpdateOneAsync(filter, update);
            return true;
        }

        public async Task<Build> GetById(string buildId)
        {
            var builder = Builders<Build>.Filter;
            var filter = builder.Eq("_id", ObjectId.Parse(buildId));
            var builds = await this.ConnectionHandler.MongoCollection.Find(filter).ToListAsync();
            if (builds != null)
            {
                return builds.FirstOrDefault();
            }
            return new Build();
        }

        public async Task<List<Build>> GetByProject(string projectId)
        {
            var builder = Builders<Build>.Filter;
            var filter = builder.Eq("ProjectId", ObjectId.Parse(projectId));
            var builds = await this.ConnectionHandler.MongoCollection.Find(filter).ToListAsync();
            return builds;
        }
    }
}
