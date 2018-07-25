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
      
        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Index()
        {
            using (var db = new ModelDataContext())
            {
                var users = db.Users.ToList(); 
                return View(users);
            }
        }

        //Not active
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
            using (var db = new DbWorker())
            {
                return Json(db.getUsersCoordinteByRange(0));
            }
        }

        [HttpPost]
        public JsonResult SaveUser(string userData)
        {
            try
            {
                using (var db = new DbWorker())
                {
                    int userId = db.SaveUser(userData);

                    Session["UserId"] = userId;

                    LinkFile2User(userId);

                    return Json(db.getUsersCoordinteByRange(0));
                }
            } catch (Exception exe)
            {
                throw new Exception(exe.Message);
            }
        }


        //refresh panel from cs
        //moved in mapController
        [HttpPost]
        public JsonResult GetUser(string userId)
        {
            using (var db = new DbWorker())
            {
                string model = db.getModel(userId);

                return Json(model);
            }
        }

    

        private void LinkFile2User(int UserId)
        {
            if (Session["PhotoId"] == null)
                return;

            try
            {
                using (var db = new DbWorker())
                {
                    var PhotoIds = (List<int>)Session["PhotoId"];

                    db.linqEntityAndPhotos(PhotoIds, UserId);
                }
            } 
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}