using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public UserController(GenericService<User> userService, GenericService<Circle> circleService)
        {
            _userService = userService;
            _circleService = circleService;
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



        // POST: api/User/Post/OnUser
        [HttpPost("Post/OnUser")]
        public ActionResult<User> CreatePost(Post post)
        {
            
            var user = _userService.Read(u => Equals(u.Id, post.OwnerId));


            if (post.Id == null)
            {
                post.Id = ObjectId.GenerateNewId().ToString();
            }


            if (post.Created.CompareTo(new DateTime(1, 1, 1)) <= 0)
            {
                post.Created = DateTime.Now;
            }


            user.Posts.Add(post);
            _userService.Update(user, user.Id);

            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }



        //POST: api/User/Comment
        [HttpPost("Comment")]
        public ActionResult<Post> CreateComment(Comment comment)
        {

            try
            {
                var userlist = _userService.Read();
                var user = userlist.Find(u => Equals(u.Id, u.Posts.Find(p => Equals(p.Id, comment.PostId)).OwnerId));


                if (comment.Id == null)
                {
                    comment.Id = ObjectId.GenerateNewId().ToString();
                }

                user.Posts.Find(p => p.Id == comment.PostId).Comments.Add(comment);

                _userService.Update(user, user.Id);
                return CreatedAtRoute("GetUser", new { id = user.Id }, user);
            }
            catch (Exception e)
            {
                
            }

            try
            {
                var circleList = _circleService.Read();
                var circle = circleList.Find(c => Equals(c.Id, c.Posts.Find(p => Equals(p.Id, comment.PostId)).OwnerId));

                if (comment.Id == null)
                {
                    comment.Id = ObjectId.GenerateNewId().ToString();
                }

                circle.Posts.Find(p=>p.Id==comment.PostId).Comments.Add(comment);
                _circleService.Update(circle, circle.Id);
                return CreatedAtRoute("GetCircle", new { id = circle.Id }, circle);
            }
            catch (Exception e)
            {
                
            }
            return NotFound();

        }

    }
}
