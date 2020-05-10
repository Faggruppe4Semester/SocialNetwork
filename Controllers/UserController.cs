using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using SocialNetwork.Areas.Database;
using SocialNetwork.Models;
using SocialNetwork.Services;

namespace SocialNetwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly GenericService<User> _userService;
        private readonly GenericService<Circle> _circleService;
        private readonly GenericService<Post> _postService;

        public UserController(GenericService<User> userService, GenericService<Circle> circleService, GenericService<Post> postService)
        {
            _userService = userService;
            _circleService = circleService;
            _postService = postService;
        }

        // GET: api/User
        [HttpGet]
        public ActionResult<List<User>> Get() => _userService.Read();

        // GET: api/User/5
        [HttpGet("{id:length(24)}", Name = "GetUser")]
        public ActionResult<User> Get(string id) => _userService.Read(id) ?? (ActionResult<User>) NotFound();

        // POST: api/User
        [HttpPost]
        public ActionResult<User> Create(User user)
        {
            _userService.Create(user);
            return CreatedAtRoute("GetUser", new {id = user.Id}, user);
        }

        // PUT: api/User/5
        [HttpPut("{id:length(24)}")]
        public ActionResult<User> Update(User userIn, string id)
        {
            var user = _userService.Read(id);
            if (user == null)
            {
                return NotFound();
            }

            return _userService.Update(userIn, id);
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var user = _userService.Read(id);
            if (user == null)
            {
                return NotFound();
            }

            _userService.Delete(user.Id);

            return NoContent();
        }



        // POST: api/User/Post/
        [HttpPost("Post")]
        public ActionResult<User> CreatePost(Post post)
        {
            try
            {
                var user = _userService.Read(u => Equals(u.Id, post.OwnerId));

                post.BlockedUserIds = user.BlockedUserIDs;

                if (post.Id == null)
                {
                    post.Id = ObjectId.GenerateNewId().ToString();
                }


                if (post.Created.CompareTo(new DateTime(1, 1, 1)) <= 0)
                {
                    post.Created = DateTime.Now;
                }


                _postService.Create(post);

                return CreatedAtRoute("GetUser", new { id = user.Id }, user);
            }
            catch (Exception)
            {

                return Conflict();
            }

        }



        //POST: api/User/Comment
        [HttpPost("Comment")]
        public ActionResult<Post> CreateComment(Comment comment)
        {

            try
            {
                var parentPost = _postService.Read(comment.PostId);

                if (comment.Id == null) comment.Id = ObjectId.GenerateNewId().ToString();

                parentPost.Comments.Add(comment);

                _postService.Update(parentPost, parentPost.Id);
                return parentPost;
            }
            catch (Exception)
            {

                return Conflict();
            }

        }


        //GET: api/User/Wall
        [HttpGet("Wall")]
        public ActionResult<List<Post>> ShowWall(List<string> Ids)
        {

            try
            {
                string userId = Ids[0];
                string guestId = Ids[1];



                var posts = _postService.ReadCollection().Aggregate()
                    .Match(p =>
                        p.OwnerId == userId &&
                        p.Public == true &&
                        !p.BlockedUserIds.Contains(guestId))
                    .SortByDescending(p => p.Created)
                    .Limit(10).ToList();

                return posts;

            }
            catch (Exception)
            {

                return NotFound();
            }
        }

        //Get: api/User/Feed/UserId
        [HttpGet("Feed/{userId}")]
        public ActionResult<List<Post>> ShowFeed(string userId)
        {

            try
            {
                User user = _userService.Read(u => Equals(u.Id, userId));
                List<Circle> circles = _circleService.Read();
                List<Post> returnPosts = new List<Post>();

                var posts = _postService.ReadCollection().Aggregate()
                    .Match(p =>
                        user.FollowedUserIDs.Contains(p.OwnerId) &&
                        !p.BlockedUserIds.Contains(user.Id) &&
                        p.Public == true)
                    .SortByDescending(p => p.Created)
                    .Limit(10)
                    .ToList();

                returnPosts.AddRange(posts);

                foreach (Circle c in circles)
                {
                    if(c.MemberIDs.Contains(user.Id))
                    {
                        posts = _postService.ReadCollection().Aggregate()
                            .Match(p => 
                                user.CirclesIDs.Contains(p.CircleId) &&
                                c.Id == p.CircleId &&
                                c.MemberIDs.Contains(user.Id))
                            .SortByDescending(p => p.Created)
                            .Limit(10)
                            .ToList();

                        returnPosts.AddRange(posts);
                    }
                }



                return returnPosts.OrderByDescending(p => p.Created).Take(10).ToList();
            }
            catch (Exception)
            {

                return Conflict();
            }


        }

    }
}
