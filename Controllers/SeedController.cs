using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
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
                Email = "peter@nielsen.com",
                Id = ObjectId.GenerateNewId().ToString()
            };
            var ole = new User
            {
                Name = "Ole Pedersen",
                Age = 14,
                Email = "xxxSwagMasterxxx@live.dk",
                Id = ObjectId.GenerateNewId().ToString()
            };
            var matilde = new User
            {
                Name = "Matilde Jørgensen",
                Age = 21,
                Email = "matilde@joergensen.dk",
                Id = ObjectId.GenerateNewId().ToString()
            };

            //Opret Circle
            string tempid = ObjectId.GenerateNewId().ToString();
            var studiegruppen = new Circle
            {
                Name = "StudieGruppen",
                Id = tempid,
                MemberIDs = new List<string>
                {
                    matilde.Id,
                    peter.Id
                },
                Posts = new List<Post>
                {
                    new Post{Id = ObjectId.GenerateNewId().ToString(), OwnerId = tempid, Created = new DateTime(2020,4,20),content = new Content{Text = "I dag skal vi undersøge de medicinske egenskaber af hash"}},
                    new Post{Id = ObjectId.GenerateNewId().ToString(), OwnerId = tempid, Created = new DateTime(2019,10,25),content = new Content{Text = "Her er noterne fra idag"}}
                }
            };

            //Tilføj fulgte og blokerede users.
            matilde.BlockedUserIDs.Add(ole.Id);
            ole.FollowedUserIDs.Add(matilde.Id);
            peter.FollowedUserIDs.Add(matilde.Id);


            //Tilføj posts til bruger.
            #region AddPosts
            matilde.Posts.Add(new Post 
            {
                OwnerId=matilde.Id,
                Created=new DateTime(2005,12,24),
                Id= ObjectId.GenerateNewId().ToString(),
                content = new Content
                {
                    Text = "Glædelig jul"
                }
            });

            matilde.Posts.Add(new Post
            {
                OwnerId = matilde.Id,
                Created = new DateTime(2006, 12, 24),
                Id = ObjectId.GenerateNewId().ToString(),
                content = new Content
                {
                    Text = "Glædelig jul 2: ELectric Boogaloo"
                }
            });

            matilde.Posts.Add(new Post
            {
                OwnerId = matilde.Id,
                Created = new DateTime(2007, 12, 24),
                Id = ObjectId.GenerateNewId().ToString(),
                content = new Content
                {
                    Text = "Glædelig jul 3: Tokyo Drift"
                }
            });

            matilde.Posts.Add(new Post
            {
                OwnerId = matilde.Id,
                Created = new DateTime(2012, 12, 31),
                Id = ObjectId.GenerateNewId().ToString(),
                content = new Content
                {
                    Text = "Øv, jorden gik ikke under."
                }
            });

            matilde.Posts.Add(new Post
            {
                OwnerId = matilde.Id,
                Created = new DateTime(2015, 7, 5),
                Id = ObjectId.GenerateNewId().ToString(),
                content = new Content
                {
                    Title = "Video af sommerferie",
                    URL = "www.youtube.com/seminferie",
                    Length = 56
                }
            });

            matilde.Posts.Add(new Post
            {
                OwnerId = matilde.Id,
                Created = new DateTime(2015, 8, 31),
                Id = ObjectId.GenerateNewId().ToString(),
                content = new Content
                {
                    Title = "Video af sommerferie2",
                    URL = "www.youtube.com/seminferie2",
                    Length = 42
                }
            });

            matilde.Posts.Add(new Post
            {
                OwnerId = matilde.Id,
                Created = new DateTime(2015, 10, 31),
                Id = ObjectId.GenerateNewId().ToString(),
                content = new Content
                {
                    Title = "Top lækker efterårsferie",
                    URL = "www.youtube.com/seminferie3",
                    Length = 1
                }
            });

            matilde.Posts.Add(new Post
            {
                OwnerId = matilde.Id,
                Created = new DateTime(2016, 1, 3),
                Id = ObjectId.GenerateNewId().ToString(),
                content = new Content
                {
                    Title = "Video af juleferie",
                    URL = "www.youtube.com/seminferie4",
                    Length = 12
                }
            });

            matilde.Posts.Add(new Post
            {
                OwnerId = matilde.Id,
                Created = new DateTime(2016, 3, 3),
                Id = ObjectId.GenerateNewId().ToString(),
                content = new Content
                {
                    Title = "Video af vinterferie",
                    URL = "www.youtube.com/seminferie5",
                    Length = 31
                }
            });

            matilde.Posts.Add(new Post
            {
                OwnerId = matilde.Id,
                Created = new DateTime(2016, 4, 21),
                Id = ObjectId.GenerateNewId().ToString(),
                content = new Content
                {
                    Title = "Video af påskeferie",
                    URL = "www.youtube.com/seminferie6",
                    Length = 18
                }
            });

            string temppostid = ObjectId.GenerateNewId().ToString();
            matilde.Posts.Add(new Post
            {
                OwnerId = matilde.Id,
                Created = new DateTime(2016, 5, 30),
                Id = temppostid,
                content = new Content
                {
                    Title = "Video af læseferie",
                    URL = "www.youtube.com/seminferie7",
                    Length = 120
                },
                Comments = new List<Comment>
                {
                    new Comment{Id = ObjectId.GenerateNewId().ToString(), Text="Det ser super spændenden ud", PostId=temppostid}
                }
            });

            temppostid = ObjectId.GenerateNewId().ToString();
            matilde.Posts.Add(new Post
            {
                OwnerId = matilde.Id,
                Created = new DateTime(2020, 4, 21),
                Id = temppostid,
                content = new Content
                {
                    Text = "Jeg har valgt at slette min konto på GenericSocialMedia"
                },
                Comments = new List<Comment>
                {
                    new Comment{Id = ObjectId.GenerateNewId().ToString(), Text ="Åh nej", PostId = temppostid},
                    new Comment{Id = ObjectId.GenerateNewId().ToString(), Text ="One of us, one of us!", PostId = temppostid}
                }
            });

            matilde.Posts.Add(new Post
            {
                OwnerId = matilde.Id,
                Created = new DateTime(1998, 4, 21),
                Id = ObjectId.GenerateNewId().ToString(),
                content = new Content
                {
                    Text = "Dette er en post med en gammel dato, som er tilføjet efter nyere posts"
                }
            });

            #endregion

            //Tilføj circle til brugere
            matilde.CirclesIDs.Add(studiegruppen.Id);
            peter.CirclesIDs.Add(studiegruppen.Id);

            //indsæt brugere og circles i collections
            _users.InsertOne(peter);
            _users.InsertOne(ole);
            _users.InsertOne(matilde);
            _circles.InsertOne(studiegruppen);
            
            return NoContent();
        }

    }
}