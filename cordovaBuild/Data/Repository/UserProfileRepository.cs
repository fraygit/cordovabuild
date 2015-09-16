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
    public class UserProfileRepository : EntityService<UserProfile>, IUserProfileRepository
    {
        public async Task<UserProfile> GetUserProfile(string user)
        {
            var builder = Builders<UserProfile>.Filter;
            var filter = builder.Eq("Username", user);
            var userProfile = await this.ConnectionHandler.MongoCollection.Find(filter).FirstOrDefaultAsync();
            return userProfile;
        }
    }
}
