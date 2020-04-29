using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using SocialNetwork.Areas.Database;
using SocialNetwork.Models;

namespace SocialNetwork.Services
{
    public class UserService : IService<User, string>
    {
        private readonly IMongoCollection<User> _users;

        public UserService(ISocialNetworkDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UserCollectionName);
        }
        public User Create(User obj)
        {
            _users.InsertOne(obj);
            return obj;
        }

        public List<User> Read() => _users.Find(user => true).ToList();

        public User Read(string id) => _users.Find(user => user.UserId == id).FirstOrDefault();

        public User Update(User obj, string id)
        {
            _users.ReplaceOne(user => user.UserId == id, obj);
            return obj;
        }

        public void Delete(User obj) => _users.DeleteOne(user => user.UserId == obj.UserId);


        public void Delete(string id) => _users.DeleteOne(user => user.UserId == id);

    }
}
