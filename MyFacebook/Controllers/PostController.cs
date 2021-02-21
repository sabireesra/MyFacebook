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

    public class PostController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        BaseRepository<Post> postRepo = new BaseRepository<Post>();
        // GET: Post
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddPost()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPost(Post newPost)
        {
            newPost.UserId = SessionSet<User>.Get("login").Id;
            newPost.ParentId = 0;

            postRepo.Add(newPost);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Onerilen()
        {
            var _user = SessionSet<User>.Get("login");

            int FriendId = Convert.ToInt32(RouteData.Values["id"]);
            int myId = _user.Id;
            var notfriends = db.Database.SqlQuery<int>(
                                  "SELECT Id FROM Users where Id not IN(SELECT FriendId FROM UserFriends where UserId =   " + myId + ") and ID!=" + myId).ToList();
            List<User> list = new List<User>();
            foreach (int item in notfriends)
            {

                var model = db.Users.FirstOrDefault(x => x.Id == item);

                if (model.IsDeleted == false)
                {
                    list.Add(model);
                }


            }
            return View(list);


        }

        [HttpGet]
        public ActionResult AddFriend()
        {
            var _user = SessionSet<User>.Get("login");
            int FriendId = Convert.ToInt32(RouteData.Values["id"]);
            int myId = _user.Id;

            UserFriends uf1 = new UserFriends();

            uf1.UserId = myId;
            uf1.FriendId = FriendId;
            uf1.CreateDate = DateTime.Now;

            var result = postRepo.Query<UserFriends>().Where(k => (k.UserId == uf1.UserId) && (k.FriendId == uf1.FriendId)).FirstOrDefault();
            if (result == null)
            {
                db.UserFriends.Add(uf1);
                db.SaveChanges();
            }

            UserFriends uf2 = new UserFriends();

            uf2.UserId = FriendId;
            uf2.FriendId = myId;
            uf2.CreateDate = DateTime.Now;

            var result2 = postRepo.Query<UserFriends>().Where(k => (k.UserId == uf2.UserId) && (k.FriendId == uf2.FriendId)).FirstOrDefault();
            if (result == null)
            {
                db.UserFriends.Add(uf2);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        public ActionResult RemoveFriend(int Id)
        {
            var _user = SessionSet<User>.Get("login");
            int myId = _user.Id;

            var result1 = db.UserFriends.FirstOrDefault(k => (k.FriendId == Id) && (k.UserId == myId));
            if (result1 != null)
                postRepo.RemoveFriend(result1.Id);

            var result2 = db.UserFriends.FirstOrDefault(k => (k.UserId == Id) && (k.FriendId == myId));
            if (result2 != null)
                postRepo.RemoveFriend(result2.Id);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Delete()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            int myId = SessionSet<User>.Get("login").Id;
            var post = postRepo.Query<Post>().FirstOrDefault(k => k.Id == id);
            if (post != null && post.UserId == myId)
            {
                postRepo.Delete(post);

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult CommentList(int id)
        {
            var _user = SessionSet<User>.Get("login");
            var anytweet = postRepo.Query<Post>().FirstOrDefault(k => k.Id == id);
            // Tweet tweet = new Tweet();
            List<PostDto> list = new List<PostDto>();
            list.Clear();
            if (_user != null)
            {
                var mymodel = (from t in postRepo.Query<Comment>()

                               where (t.ParentId == anytweet.Id) && t.IsDeleted == false
                               select new PostDto()
                               {
                                   Id = t.Id,
                                   Body = t.Body,
                                   UserName = t.User.UserName

                               }).ToList();
                mymodel.Reverse();
                foreach (PostDto model in mymodel)
                {

                    list.Add(model);

                }

                //list = mymodel.OrderByDescending(x => x.CreateDate).ToList();


                return View(list);
            }
            return View();
        }



    }
}