using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MapFinder;
using MapFinder.App_Code;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace MapFinder.Controllers
{
    [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
    public class HomeController : Controller
    {
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Index()
        {
            using (var db = new UsersDataContext())
            {
                var users = db.Users.ToList();
                return View(users);
            }
        }

        public ActionResult Liflet()
        {
            ViewBag.Message = "Liflet";
            using (var db = new UsersDataContext())
            {
                var users = db.Users.ToList();
                return View(users);
            }
        }

        [HttpGet]
        public PartialViewResult InitUser()
        {
            return PartialView();
        }
        
        [HttpPost]
        public JsonResult GetUsers()
        {
            using (var db = new UsersDataContext())
            {
                return Json(getUsersCoordinteByRange(0));
            }
        }

        [HttpPost]
        public JsonResult save(string userData)
        {
            try
            {
                using (var db = new UsersDataContext())
                {
                    User user = new User();
                    user = JsonConvert.DeserializeObject<User>(userData);

                    user.Password = CalculateMD5Hash(user.Password);

                    db.Users.InsertOnSubmit(user);

                    db.SubmitChanges();

                    return Json(getUsersCoordinteByRange(0));
                }
            } catch (Exception exe)
            {
                throw new Exception(exe.Message);
            }
        }

        public string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            string sb = "";
            for (int i = 0; i < hash.Length; i++)
            {
                sb += (hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public string getUsersCoordinteByRange(float range)
        {
            string userJsonCoord;

            using (var db = new UsersDataContext())
            {
                var user = db.Users.Select(x => new { x.Lat, x.Lon }).ToList();
                userJsonCoord = JsonConvert.SerializeObject(user);
            }

            return userJsonCoord;
        }

    }
}