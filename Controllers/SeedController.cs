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
        public SeedController(ISocialNetworkDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UserCollectionName);
            
        }

        [HttpPost("Users")]
        public ActionResult SeedUsers()
        {
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
            
            _users.InsertOne(peter);
            _users.InsertOne(ole);
            _users.InsertOne(matilde);

            return NoContent();
        }

    }
}