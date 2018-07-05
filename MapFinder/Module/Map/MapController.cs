using Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MapFinder.Module.Map
{
    public class MapController : Controller
    {
        [HttpPost]
        public JsonResult GetUsers()
        {
            using (var db = new ModelDataContext())
            {
                return Json(getUsersCoordinteByRange(0));
            }
        }
        private string getUsersCoordinteByRange(float range)
        {
            string userJsonCoord;

            using (var db = new ModelDataContext())
            {
                var user = db.Users.Select(l => new { l.Lat, l.Lon, l.UserId }).ToList();
                userJsonCoord = JsonConvert.SerializeObject(user);
            }

            return userJsonCoord;
        }
    }
}