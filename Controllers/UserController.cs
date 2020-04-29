using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using SocialNetwork.Models;
using SocialNetwork.Services;

namespace SocialNetwork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly GenericService<User> _userService;

        public UserController(GenericService<User> userService)
        {
            _userService = userService;
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
    }
}
