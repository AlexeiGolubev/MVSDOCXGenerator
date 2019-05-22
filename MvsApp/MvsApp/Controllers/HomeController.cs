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
        public ActionResult Index()
        {
            string[] files = Directory.GetFiles(@"C:\Users\Alex\Desktop\Test");
            List<string> listDocx = new List<string>();

            foreach (var fileName in files)
            {
                if (fileName.EndsWith(".docx"))
                {
                    listDocx.Add(fileName);
                }
            }

            return View(listDocx);
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