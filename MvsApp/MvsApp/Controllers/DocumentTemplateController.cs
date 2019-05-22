using MvsApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MvsApp.Controllers
{
    public class DocumentTemplateController : Controller
    {
        // GET: DocumentTemplate
        public ActionResult Index(string path)
        {
            Document doc = new Document(path);
            doc.GetFields();//получаем переменные из док-та и записываем их в модель

            return View(doc);
        }

        [HttpPost]
        public FileResult Index(Document doc)
        {
            doc.SetFields();//полученные переменные заменяем на введённые пользователем данные

            //FilePathResult file = File(doc.Path, "application/docx", "123.docx");

            //System.IO.File.Delete(newPath);

            return File(doc.Path, "application/docx", "123.docx");
        }
    }
}