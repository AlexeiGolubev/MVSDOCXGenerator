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

        [HttpGet]
        public ActionResult LoadJson(string path)
        {
            return View((object)path);
        }

        [HttpPost]
        public ActionResult LoadJson(HttpPostedFileBase upload, string pathFile)
        {
            string pathJson = null;

            if (upload != null)
            {
                // получаем имя файла
                string jsonName = System.IO.Path.GetFileNameWithoutExtension(upload.FileName) + DateTime.Now.ToBinary() + System.IO.Path.GetExtension(upload.FileName);
                // сохраняем файл в папку Files в проекте
                pathJson = Server.MapPath("~/App_Data/Files/" + jsonName);
                upload.SaveAs(pathJson);
            }

            if (pathFile != null && pathJson != null)
            {
                do
                {
                    var documentContentControls = MvsApp.Logics.TemplateEngine.GetContentControls(pathFile);

                    var documentDatasFromJson = MvsApp.Logics.JsonEngine.ReadJson(pathJson);

                    var newDocumentDatas = MvsApp.Logics.TemplateEngine.CompareContentControls(pathFile, documentContentControls, documentDatasFromJson);

                    MvsApp.Logics.TemplateEngine.FillContentControls(pathFile, newDocumentDatas);
                }
                while (MvsApp.Logics.TemplateEngine.GetContentControls(pathFile).Count != 0);

                if (System.IO.File.Exists(pathFile))
                {
                    return File(pathFile, "application/docx", System.IO.Path.GetFileName(pathFile));
                }
            }
            ViewBag.Message = "Возможно вы не загрузили json или загрузили пустой json, попробуйте ещё раз";
            return View("~/Views/Home/Index.cshtml");
        }
    }
}