using MyFacebook.Models;
using MyFacebook.Models.Repository;
using MyFacebook.SessionModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyFacebook.Controllers
{
    public class ProfilController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        BaseRepository<User> userRepo = new BaseRepository<User>();
        //BaseRepository<Like> _repo = new BaseRepository<Like>();

        public ActionResult Index()
        {
            var _user = SessionSet<User>.Get("login");
            if (_user != null)
            {
                using (BaseRepository<Post> _repo = new BaseRepository<Post>())
                {
                    var mymodel = (from u in _repo.Query<User>()
                                   join
                                   p in _repo.Query<Post>() on u.Id equals p.UserId
                                   where (u.Id == _user.Id) && p.IsDeleted == false
                                   orderby p.CreateDate descending
                                   select new PostDto()
                                   {
                                       Id = p.Id,
                                       Body = p.Body,
                                       UserName = u.UserName,
                                   }).Distinct().ToList();
                    mymodel.Reverse();
                    return View(mymodel);
                }
            }
            else { return RedirectToAction("Login", "Login"); }
        }

        public ActionResult Friends()
        {
            List<UserFriends> newList = new List<UserFriends>();
            var _user = SessionSet<User>.Get("login");
            int myId = _user.Id;
            var mine = db.UserFriends.Where(k => (k.UserId == myId) && (k.FriendId != myId)).ToList();
            foreach (var item in mine)
            {
                var friend = db.Users.Where(k => k.Id == item.FriendId).First();
                UserFriends yeni = new UserFriends()
                {
                    UserId = item.UserId,
                    FriendId = item.FriendId,
                    User = db.Users.Where(k => k.Id == friend.Id).First()
                };
                newList.Add(yeni);
            }
            return View(newList);
        }

    }
}