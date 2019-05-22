using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace MvsApp.Models
{
    public class Document
    {
        public Document()
        {
            Path = null;
            Fields = new Dictionary<string, string>();
        }

        public Document(string path)
        {
            Path = path;
            Fields = new Dictionary<string, string>();
        }

        public Document(string path, Dictionary<string, string> fields)
        {
            Path = path;
            Fields = fields;
        }

        public string Path { get; set; }

        public Dictionary<string, string> Fields { get; set; }

        public void GetFields()
        {
            if (Path != null)
            {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(Path, true))
                {
                    string docText = null;
                    using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                    {
                        docText = sr.ReadToEnd();
                    }

                    Regex regex = new Regex(@"\$(?!<[a-zA-Z-0-9"":/-\\s = _]+>)[A-Za-z0-9]+");
                    MatchCollection matches = regex.Matches(docText);
                    if (matches.Count > 0)
                    {
                        foreach (Match match in matches)
                        {
                            Fields.Add(match.Value, null);
                        }
                    }
                }
            }
        }

        public void SetFields()
        {
            string docText = null;
            //StringBuilder docText = new StringBuilder();

            string newPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.Path), 
                System.IO.Path.GetFileNameWithoutExtension(this.Path) + "1000" + System.IO.Path.GetExtension(this.Path));

            if (File.Exists(newPath))
            {
                File.Delete(newPath);
            }

            File.Copy(this.Path, newPath);
            this.Path = newPath;

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(this.Path, true))
            {
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                foreach (var field in this.Fields)
                {
                    docText = Regex.Replace(docText, @"\" + field.Key, field.Value);
                }

                //Regex regexText = new Regex("Hello world!");
                //docText = regexText.Replace(docText, "Hi Everyone!");

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }
            }
        }

        public override string ToString()
        {
            string s = this.Path;
            foreach (var item in this.Fields)
            {
                s += " " + item.Key + " " + item.Value;
            }
            return s;
        }
    }
}