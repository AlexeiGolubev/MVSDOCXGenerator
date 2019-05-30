using MvsApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web;

namespace MvsApp.Logics
{
    public class JsonEngine
    {
        public static void WriteJson(string fileNameJson, List<DocumentContentControl> documentContentControls)
        {
            #region Data to Json

            List<DocumentData> documentDatasToJson = new List<DocumentData>();
            //записываю ContentControls в Data с пустыми данными для отдачи Data в json
            foreach (var item in documentContentControls)
            {
                if (item is FieldContentControl)
                {
                    documentDatasToJson.Add(new FieldDocumentData(item.Field, ""));
                }

                if (item is ListContentControl)
                {
                    ListDocumentData listDocumentData = new ListDocumentData(item.Field);

                    foreach (var item1 in (item as ListContentControl).Fields)
                    {
                        FieldsDocumentData field = new FieldsDocumentData(item1.Field);
                        field.Values.Add("");
                        listDocumentData.Fields.Add(field);
                    }

                    foreach (var item1 in (item as ListContentControl).Tables)
                    {
                        TableDocumentData tableDocumentData = new TableDocumentData(item1.Field);

                        foreach (var item2 in (item1 as TableContentControl).Fields)
                        {
                            FieldsDocumentData field = new FieldsDocumentData(item2.Field);
                            field.Values.Add("");
                            tableDocumentData.Fields.Add(field);
                        }

                        listDocumentData.Tables.Add(tableDocumentData);
                    }

                    documentDatasToJson.Add(listDocumentData);
                }

                if (item is TableContentControl)
                {
                    TableDocumentData tableDocumentData = new TableDocumentData(item.Field);

                    foreach (var item1 in (item as TableContentControl).Fields)
                    {
                        FieldsDocumentData field = new FieldsDocumentData(item1.Field);
                        field.Values.Add("");
                        tableDocumentData.Fields.Add(field);
                    }

                    documentDatasToJson.Add(tableDocumentData);
                }
            }

            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<DocumentData>));

            //запись в json
            using (FileStream fs = new FileStream(fileNameJson, FileMode.CreateNew))
            {
                jsonFormatter.WriteObject(fs, documentDatasToJson);
            }
            #endregion
        }

        public static List<DocumentData> ReadJson(string fileNameJson)
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<DocumentData>));

            List<DocumentData> documentDatasFromJson = new List<DocumentData>();
            //получаю Data из json
            using (FileStream fs = new FileStream(fileNameJson, FileMode.OpenOrCreate))
            {
                documentDatasFromJson = (List<DocumentData>)jsonFormatter.ReadObject(fs);
            }

            return documentDatasFromJson;
        }
    }
}