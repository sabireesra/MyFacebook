using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFacebook.Models
{
    public class PostDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ParentId { get; set; }
        public string UserName { get; set; }
        public string Body { get; set; }
        public DateTime CreateDate { get; set; }
        public User User { get; set; }
        public int? ParentPostId { get; set; }
        public Post ParentPost { get; set; }
        public bool IsDeleted { get; set; }
    }
}