using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SocialNetwork.Models
{
    public interface IModel
    {
        public string Id { get; set; }
    }
}