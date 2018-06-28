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
using System.IO;

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
            using (var db = new ModelDataContext())
            {
                var users = db.Users.ToList();
                return View(users);
            }
        }

        public ActionResult Liflet()
        {
            ViewBag.Message = "Liflet";
            using (var db = new ModelDataContext())
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
            using (var db = new ModelDataContext())
            {
                return Json(getUsersCoordinteByRange(0));
            }
        }

        [HttpPost]
        public JsonResult save(string userData)
        {
            try
            {
                using (var db = new ModelDataContext())
                {
                    User user = new User();
                    user = JsonConvert.DeserializeObject<User>(userData);

                    user.Password = CalculateMD5Hash(user.Password);

                    db.Users.InsertOnSubmit(user);

                    db.SubmitChanges();

                    Session["UserId"] = user.UserId;//?? 

                    linkFile2User();

                    return Json(getUsersCoordinteByRange(0));
                }
            } catch (Exception exe)
            {
                throw new Exception(exe.Message);
            }
        }

        [HttpPost]
        public JsonResult Edit(HttpPostedFileBase image)
        {
            int PhotoId = 0;

            if(Request.Files.Count > 0)
                PhotoId = StoreFile(Request.Files[0]);

            return Json(String.Format("Photo:{0}",PhotoId));
        }

        [HttpPost]
        public JsonResult getUser(string Data)
        {
            using (var db = new ModelDataContext())
            {
                var model = from photo in db.Photos
                            join user in db.Users on photo.ObjectId equals user.UserId into tmp
                            where photo.ObjectId == Convert.ToInt32(Data)
                            from userData in tmp.DefaultIfEmpty() 

                            select new
                            {
                                userData,
                                photoId = photo.PhotoId,
                                objName = photo.ObjectName
                                //Name = ed.EmpName,
                                //Department = dpem.DeptName
                            };

                return Json(JsonConvert.SerializeObject(model));
            }
        }

        private int StoreFile(HttpPostedFileBase file)
        {
            byte[] data = new byte[file.ContentLength];
            file.InputStream.Read(data, 0, file.ContentLength);

            using (var db = new ModelDataContext())
            {
                var photo = new Photo();// db;//.Select(x => new { x.Lat, x.Lon }).ToList();

                photo.FileData = data;
                photo.MnimeType = file.ContentType;

                db.Photos.InsertOnSubmit(photo);

                db.SubmitChanges();

                return photo.PhotoId;
                //return Json(getUsersCoordinteByRange(0));

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

            using (var db = new ModelDataContext())
            {
                var user = db.Users.Select(l => new { l.Lat, l.Lon, l.UserId }).ToList();
                userJsonCoord = JsonConvert.SerializeObject(user);
            }

            return userJsonCoord;
        }

        private void linkFile2User()
        {
            if (Session["PhotoId"] == null)
                return;

            try
            {
                using (var db = new ModelDataContext())
                {
                    var PhotoIds = (List<int>)Session["PhotoId"];

                    var photo = db.Photos.Where(l => PhotoIds.Contains(l.PhotoId)).ToList();

                    photo.ForEach(l => { l.ObjectId = (int)Session["UserId"]; l.ObjectName = "Users"; });
                    db.SubmitChanges();
                    //.Select(l => new { l.PhotoId = (int)Session["PhotoId"] })
                }
            } 
            catch(Exception ex)
            {
                throw new Exception(ex.Message);//зачем?
            }
        }

    }
}