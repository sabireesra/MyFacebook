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
    public class LoginController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        BaseRepository<Post> postRepo = new BaseRepository<Post>();
        public ActionResult ConnectWithYourself()
        {
            var _user = SessionSet<User>.Get("login");
            int FriendId = SessionSet<User>.Get("login").Id;
            int myId = _user.Id;

            UserFriends uf0 = new UserFriends();

            uf0.UserId = myId;
            uf0.FriendId = FriendId;
            uf0.CreateDate = DateTime.Now;

            var result = postRepo.Query<UserFriends>().Where(k => (k.UserId == uf0.UserId) && (k.FriendId == uf0.FriendId)).FirstOrDefault();
            if (result == null)
            {
                db.UserFriends.Add(uf0);
                db.SaveChanges();
            }

          
            return RedirectToAction("Index", "Home");
        }
        public ActionResult LogOff()
        {
            Session.Clear();
            return RedirectToAction("Login", "Login");
        }

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User model)
        {
            BaseRepository<User> _repo = new BaseRepository<User>();

            var repo = _repo.Query<User>().Where(x => x.Email == model.Email && x.Password == model.Password).ToList();

            if (repo.Count > 0)
            {
                User _temp = new User();
                foreach (var item in repo)
                {
                    _temp.UserName = item.UserName;
                    _temp.Email = item.Email;
                    _temp.Id = item.Id;
                    //_temp.LastName = item.LastName;
                    //_temp.Dis = item.Dis;

                }

                SessionSet<User>.Set(_temp, "login");

                ConnectWithYourself();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Hatalı Giriş Yaptınız");
                return View();
            }

        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User model)
        {
            using (BaseRepository<User> loginRepo = new BaseRepository<User>())
            {
                loginRepo.Add(model);
                return RedirectToAction("Login");
            }
        }

        public ActionResult UserDashBoard()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


    }
}