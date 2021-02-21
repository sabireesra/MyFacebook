using MyFacebook.Models.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFacebook.Models
{
    public class User : BaseModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string ProfilePhoto { get; set; }
        public int Following_Count { get; set; }
        public int Follower_Count { get; set; }

        public ICollection<User> Friends { get; set; }

        public ICollection<Post> Posts { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }

    }
}