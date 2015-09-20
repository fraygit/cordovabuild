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
    public class ProjectRepository : EntityService<Project>, IProjectRepository
    {
        public async Task<List<Project>> GetByUser(string user)
        {
            var builder = Builders<Project>.Filter;
            var filter = builder.Eq("User", user);
            List<Project> projects = await this.ConnectionHandler.MongoCollection.Find(filter).ToListAsync();
            return projects;
        }

        public async Task<Project> Get(string projectId)
        {
            var builder = Builders<Project>.Filter;
            var filter = builder.Eq("_id", ObjectId.Parse(projectId));
            Project project = await this.ConnectionHandler.MongoCollection.Find(filter).FirstOrDefaultAsync();
            return project;
        }

    }
}
