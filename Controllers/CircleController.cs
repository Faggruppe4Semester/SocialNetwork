using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using SocialNetwork.Models;
using SocialNetwork.Services;

namespace SocialNetwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CircleController : ControllerBase
    {
        private readonly GenericService<Circle> _circleService;
        private readonly GenericService<User> _userService;

        public CircleController(GenericService<Circle> circleService, GenericService<User> userService)
        {
            _circleService = circleService;
            _userService = userService;
        }

        // GET: api/Circle
        [HttpGet]
        public ActionResult<List<Circle>> Get() => _circleService.Read();

        // GET: api/Circle/5
        [HttpGet("{id:length(24)}", Name = "GetCircle")]
        public ActionResult<Circle> Get(string id) => _circleService.Read(id) ?? (ActionResult<Circle>) NotFound();

        // POST: api/Circle
        [HttpPost]
        public ActionResult<Circle> Create(Circle circle)
        {
            _circleService.Create(circle);
            return CreatedAtRoute("GetCircle", new {id = circle.Id}, circle);
        }

        // PUT: api/Circle/5
        [HttpPut("{id:length(24)}")]
        public ActionResult<Circle> Update(Circle circleIn, string id)
        {
            var circle = _circleService.Read(id);
            if (circle == null)
            {
                return NotFound();
            }

            return _circleService.Update(circleIn, id);
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var circle = _circleService.Read(id);
            if (circle == null)
            {
                return NotFound();
            }

            _circleService.Delete(circle.Id);

            return NoContent();
        }

        // POST: api/Circle/AddUsers
        [HttpPost("AddUsers")]
        public ActionResult<Circle> AddUsers(List<string> Ids)
        {
            try
            {
                string circleId = Ids[0];
                List<string> userIds = Ids.Skip(1).ToList();

                Circle circle = _circleService.Read(c => Equals(c.Id, circleId));

                foreach (string userId in userIds)
                {
                    try
                    {
                        User user = _userService.Read(u => Equals(u.Id, userId));
                        user.CirclesIDs.Add(circleId);
                        _userService.Update(user, user.Id);
                        circle.MemberIDs.Add(user.Id);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

                _circleService.Update(circle, circle.Id);

                return CreatedAtRoute("GetCircle", new { id = circle.Id }, circle);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }



        // POST: api/Circle/Post/OnCircles
        [HttpPost("Post/OnCircles")]
        public ActionResult<Circle> CreatePostOnCircle(CirclePost cpost)
        {

            var post = cpost.post;
            var circleIds = cpost.circleIds;


            var circles = _circleService.Read();

            circles = circles.FindAll(c => circleIds.Contains(c.Id));

            if (post.Id == null)
            {
                post.Id = ObjectId.GenerateNewId().ToString();
            }


            if (post.Created.CompareTo(new DateTime(1, 1, 1)) <= 0)
            {
                post.Created = DateTime.Now;
            }


            foreach (Circle c in circles)
            {
                post.OwnerId = c.Id;
                c.Posts.Add(post);
                _circleService.Update(c, c.Id);
            }

            return Ok();
        }
    }

    public class CirclePost
    {
        public List<string> circleIds { get; set; }
        public Post post { get; set; }
    }
}
