using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvsApp.Controllers
{
    public class JsonProcessorController : Controller
    {
        // GET: JsonProcessor
        public ActionResult GetJson(string path)
        {
            string fileNameJson = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path),
                System.IO.Path.GetFileNameWithoutExtension(path) + "(empty)" +".json");

            var documentContentControls = MvsApp.Logics.TemplateEngine.GetContentControls(path);

            MvsApp.Logics.JsonEngine.WriteJson(fileNameJson, documentContentControls);

            if (System.IO.File.Exists(fileNameJson))
            {
                return File(fileNameJson, "application/json", System.IO.Path.GetFileName(fileNameJson));
            }

            return View();
        }

        public ActionResult LoadJson(string path)
        {
            string fileNameJson = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path),
                System.IO.Path.GetFileNameWithoutExtension(path) + ".json");

            string fileName = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path),
                System.IO.Path.GetFileNameWithoutExtension(path) + "(template)" + System.IO.Path.GetExtension(path));

            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }

            System.IO.File.Copy(path, fileName);

            do
            {
                var documentContentControls = MvsApp.Logics.TemplateEngine.GetContentControls(fileName);
                
                var documentDatasFromJson = MvsApp.Logics.JsonEngine.ReadJson(fileNameJson);

                var newDocumentDatas = MvsApp.Logics.TemplateEngine.CompareContentControls(fileName, documentContentControls, documentDatasFromJson);

                MvsApp.Logics.TemplateEngine.FillContentControls(fileName, newDocumentDatas);
            }
            while (MvsApp.Logics.TemplateEngine.GetContentControls(fileName).Count != 0);

            if (System.IO.File.Exists(fileName))
            {
                return File(fileName, "application/docx", System.IO.Path.GetFileName(fileName));
            }

            return View();
        }
    }
}