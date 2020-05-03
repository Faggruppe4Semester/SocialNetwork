using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SocialNetwork.Models
{
    public class Post : IModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string OwnerId { get; set; }
        public DateTime Created { get; set; }
        public string Text { get; set; }
        //public Circle Circle { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
