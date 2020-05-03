using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SocialNetwork.Areas.Database;
using SocialNetwork.Models;

namespace SocialNetwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Circle> _circles;

        public SeedController(ISocialNetworkDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UserCollectionName);
            _circles = database.GetCollection<Circle>(settings.CircleCollectionName);
            
        }

        [HttpPost("Users")]
        public ActionResult SeedUsers()
        {
            //Tøm databasen
            _users.DeleteMany(u => u.Id != null);
            _circles.DeleteMany(c => c.Id != null);

            //Opret brugere
            var peter = new User
            {
                Name = "Peter Nielsen",
                Age = 52,
                Email = "peter@nielsen.com"
            };
            var ole = new User
            {
                Name = "Ole Pedersen",
                Age = 14,
                Email = "xxxSwagMasterxxx@live.dk"
            };
            var matilde = new User
            {
                Name = "Matilde Jørgensen",
                Age = 21,
                Email = "matilde@joergensen.dk"
            };

            //Opret Circle
            var studiegruppen = new Circle
            {
                Name = "StudieGruppen"
            };

            //Tilføj brugere til circle
            studiegruppen.MemberIDs.Add(matilde.Id);
            studiegruppen.MemberIDs.Add(peter.Id);

            
            //Opret post
            var post1 = new Post
            {
                Created = DateTime.Now,
                Text = "Hej",
                OwnerId = matilde.Id
            };

            var post2 = new Post
            {
                Text = "Afleveringen er blevet afleveret",
                OwnerId = peter.Id,
                Created = new DateTime(1912, 04, 4)
            };

            //Tilføj post til bruger/circle
            matilde.Posts.Add(post1);
            studiegruppen.Posts.Add(post2);

            //indsæt brugere og circles i collections
            _users.InsertOne(peter);
            _users.InsertOne(ole);
            _users.InsertOne(matilde);
            _circles.InsertOne(studiegruppen);


            //var filter = Builders<User>.Filter.Eq(u => u.Id, matilde.Id);
            //var update = Builders<User>.Update.Push(u => u.Posts, post1);

            //_users.UpdateOne(filter, update);


            return NoContent();
        }

    }
}