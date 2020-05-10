using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SocialNetwork.Models
{
    public class User : IModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public List<string> CirclesIDs { get; set; } = new List<string>();
        public List<string> BlockedUserIDs { get; set; } = new List<string>();
        public List<string> FollowedUserIDs { get; set; } = new List<string>();
        //public List<string> PostIDs { get; set; } = new List<string>();

    }
}
