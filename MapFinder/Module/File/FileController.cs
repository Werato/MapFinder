﻿using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MapFinder.Module.File
{
    public class FileController : Controller
    {

        [HttpPost]
        public string Edit(HttpPostedFileBase image)
        {
            int PhotoId = 0;

            if (Request.Files.Count > 0)
                PhotoId = StoreFile(Request.Files[0]);

            if(Session["PhotoId"] == null)
            {
                Session["PhotoId"] = new List<int>(PhotoId);
            } else
            {
                var PhotoIdList = Session["PhotoId"] as List<int>;

                if (PhotoIdList != null)
                {
                    PhotoIdList.Add(PhotoId);
                    Session["PhotoId"] = PhotoIdList;
                }
            }

            return String.Format("File/ShowImg?strPhotoId={0}", PhotoId);
        }

        private int StoreFile(HttpPostedFileBase file)
        {
            byte[] data = new byte[file.ContentLength];
            file.InputStream.Read(data, 0, file.ContentLength);

            using (var db = new ModelDataContext())
            {
                if(!file.ContentType.Contains("image"))
                {
                    return 0;
                }

                var photo = new Photo();

                photo.FileData = data;
                photo.MnimeType = file.ContentType;

                db.Photos.InsertOnSubmit(photo);

                db.SubmitChanges();

                return photo.PhotoId;
            }
        }

        [HttpGet]
        public ActionResult ShowImg(string strPhotoId)
        {
            using (var db = new ModelDataContext())
            {
                var photo = db.Photos
                    .Where(l => l.PhotoId == Convert.ToInt32(strPhotoId)).ToList().FirstOrDefault();

                Response.Clear();
                Response.ContentType = photo.MnimeType;
                Response.BinaryWrite(photo.FileData.ToArray());
                Response.End();

                return null;
               
            }
        }
    }
}