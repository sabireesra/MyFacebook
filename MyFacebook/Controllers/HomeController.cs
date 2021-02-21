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
    public class HomeController : Controller
    {
        BaseRepository<Post> postRepo = new BaseRepository<Post>();

        public ActionResult Index()
        {
            var _user = SessionSet<User>.Get("login");

            List<PostDto> list = new List<PostDto>();
            list.Clear();
            if (_user != null)
            {
                var mymodel = (from uf in postRepo.Query<UserFriends>()
                               join
                               p in postRepo.Query<Post>() on uf.FriendId equals p.UserId
                               where (uf.UserId == _user.Id &&
                               p.UserId == uf.FriendId) && p.IsDeleted == false
                               //orderby t.CreateDate descending
                               select new PostDto()
                               {
                                   Id = p.Id,
                                   Body = p.Body,
                                   UserName = uf.Friend.UserName

                               }).Distinct().ToList();
                mymodel.Reverse();
                foreach (PostDto model in mymodel)
                {
                    list.Add(model);
                }

                list = mymodel.OrderByDescending(x => x.CreateDate).ToList();
                return View(list);
            }

            else { return RedirectToAction("Login", "Login"); }

        }

        [HttpPost]
        public ActionResult ListPost(Post newPost)
        {
            var postLists = postRepo.Query<Post>().Reverse().ToList();

            return View(postLists);

        }

        public ActionResult RePost(int postId)
        {
            using (BaseRepository<Post> _repo = new BaseRepository<Post>())
            {
                var _user = SessionSet<User>.Get("login");
                var psId = _repo.Query<Post>().FirstOrDefault(x => x.Id == postId);
                Post post = new Post
                {
                    Body = psId.Body,
                    UserId = _user.Id,

                    ParentId = postId
                };
                _repo.Add(post);
                return Json(new { result = true, JsonRequestBehavior.AllowGet });
            }
        }

        public JsonResult GetTweet(int id)
        {
            using (BaseRepository<Post> _repo = new BaseRepository<Post>())
            {

                var json = (from p in _repo.Query<Post>()
                            join u in _repo.Query<User>() on p.UserId equals u.Id
                            where p.Id == id
                            select new
                            {
                                p.Body,
                                p.CreateDate,
                                p.Id,
                                p.UserId,
                                u.UserName,
                                p.ParentId

                            }
                    ).ToList();

                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddLike(int postId)
        {
            using (BaseRepository<Like> _repo = new BaseRepository<Like>())
            {
                var _user = SessionSet<User>.Get("login");
                var psId = _repo.Query<Post>().FirstOrDefault(x => x.Id == postId);
                Like like = new Like
                {
                    Body = psId.Body,
                    UserId = _user.Id,

                    ParentId = postId
                };
                _repo.Add(like);
                return Json(new { result = true, JsonRequestBehavior.AllowGet });
            }
        }

        public JsonResult GetLike(int id)
        {
            using (BaseRepository<Like> _repo = new BaseRepository<Like>())
            {

                var json = (from lk in _repo.Query<Like>()
                            join u in _repo.Query<User>() on lk.UserId equals u.Id
                            where lk.Id == id
                            select new
                            {
                                lk.Body,
                                lk.CreateDate,
                                lk.Id,
                                lk.UserId,
                                u.UserName,
                                lk.ParentId

                            }
                    ).ToList();

                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddComment(string text, int postId)
        {
            using (BaseRepository<Comment> _repo = new BaseRepository<Comment>())
            {
                var _user = SessionSet<User>.Get("login");
                Comment cmt = new Comment
                {
                    Body = text,
                    UserId = _user.Id,

                    ParentId = postId
                };
                _repo.Add(cmt);
                return Json(new { result = true, JsonRequestBehavior.AllowGet });
            }
        }

        public JsonResult GetComment(int id)
        {
            using (BaseRepository<Comment> _repo = new BaseRepository<Comment>())
            {

                var json = (from co in _repo.Query<Comment>()
                            join u in _repo.Query<User>() on co.UserId equals u.Id
                            where co.ParentId == id
                            select new
                            {
                                co.Body,
                                co.CreateDate,
                                co.Id,
                                co.UserId,
                                u.UserName,
                                co.ParentId

                            }
                    ).ToList();

                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }



    }
}