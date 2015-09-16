using MongoDB.Bson;
using MongoDB.Driver;
using cordovaBuild.Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cordovaBuild.Data.Entities.Service
{
    public class EntityService<T> : IEntityService<T> where T : IMongoEntity
    {
        protected readonly ConnectionHandler<T> ConnectionHandler;

        protected EntityService()
        {
            ConnectionHandler = new ConnectionHandler<T>();
        }

        public virtual void Create(T entity)
        {
            var result = this.ConnectionHandler.MongoCollection.InsertOneAsync(entity);
        }

        public virtual async Task<bool> CreateSync(T entity)
        {
            await this.ConnectionHandler.MongoCollection.InsertOneAsync(entity);
            return true;
        }
        
    }
}
