using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using SocialNetwork.Areas.Database;
using SocialNetwork.Models;

namespace SocialNetwork.Services
{
    public class PostService : IService<Post, string>
    {
        private readonly IMongoCollection<Post> _posts;

        public PostService(ISocialNetworkDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _posts = database.GetCollection<Post>(settings.PostCollectionName);
        }

        public Post Create(Post obj)
        {
            _posts.InsertOne(obj);
            return obj;
        }

        public List<Post> Read() => _posts.Find(post => true).ToList();

        public Post Read(string id) => _posts.Find(post => post.PostId == id).FirstOrDefault();

        public Post Update(Post obj, string id)
        {
            _posts.ReplaceOne(post => post.UserId == id, obj);
            return obj;
        }

        public void Delete(Post obj) => _posts.DeleteOne(post => post.PostId == obj.PostId);

        public void Delete(string id) => _posts.DeleteOne(post => post.PostId == id);
    }
}
