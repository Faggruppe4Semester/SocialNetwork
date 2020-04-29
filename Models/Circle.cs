using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Models
{
    public class Circle
    {
        public string CircleId { get; set; }
        public string Name { get; set; }
        public List<User> Users { get; set; }
    }
}
