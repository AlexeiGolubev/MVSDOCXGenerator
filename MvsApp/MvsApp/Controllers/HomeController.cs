using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvsApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IndexAction(HttpPostedFileBase upload)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Server.MapPath("~/App_Data/Files/"));

            foreach (FileInfo file in dirInfo.GetFiles())
            {
                file.Delete();
            }

            string path = null;

            if (upload != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileNameWithoutExtension(upload.FileName) + DateTime.Now.ToBinary() + System.IO.Path.GetExtension(upload.FileName);
                // сохраняем файл в папку Files в проекте
                path = Server.MapPath("~/App_Data/Files/" + fileName);
                upload.SaveAs(path);
            }

            if (path != null)
            {
                return View((object)path);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

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
    }
}