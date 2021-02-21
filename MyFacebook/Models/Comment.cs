﻿using MyFacebook.Models.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFacebook.Models
{
    public class Comment : BaseModel
    {
        public User User { get; set; }
        public int UserId { get; set; }
        public string Body { get; set; }

        public Post ParentPost { get; set; }
        public int ParentId { get; set; }
    }
}