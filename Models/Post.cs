using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace SocialNetwork.Models
{
    public class Post : IModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string OwnerId { get; set; }
        public string CircleId { get; set; }
        public bool Public { get; set; }
        public DateTime Created { get; set; }
        public Content content { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<string> BlockedUserIds { get; set; } = new List<string>();


    }

    public class Content : TextContent, VideoContent
    {
        public string Text { get; set; }
        public string Title { get; set; }
        public float Length { get; set; }
        public string URL { get; set; }
    }

    public interface TextContent
    {
        public string Text { get; set; }
    }

    public interface VideoContent
    {
        public string Title { get; set; }
        public float Length { get; set; }
        public string URL { get; set; }
    }
}
