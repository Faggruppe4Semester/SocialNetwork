using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SocialNetwork.Models
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string PostId { get; set; }
        public string UserId { get; set; }
        public string Text { get; set; }
        public Circle Circle { get; set; }
        public DateTime Created { get; set; }
        private List<Comment> Comments { get; set; }
    }
}
