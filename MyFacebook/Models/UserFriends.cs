using MyFacebook.Models.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFacebook.Models
{
    public class UserFriends : BaseModel
    {
        public User User { get; set; }
        public int UserId { get; set; }
        public User Friend { get; set; }
        public int FriendId { get; set; }

    }

}