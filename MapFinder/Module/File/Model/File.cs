using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MapFinder.Module.File.Model
{
    public class File
    {
        public int FileId;
        //Лучше на js
        public string NewFileHtmlBlock = "<div>" +
            "<input type='file' name='fileToUpload' class='fileToUpload'><div>";
    }
}