using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MvsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TemplateEngine.Docx;

namespace MvsApp.Logics
{
    public class TemplateEngine
    {
        public static List<DocumentContentControl> GetContentControls(string fileName)
        {
            #region WordprocessingDocument

            List<DocumentContentControl> documentContentControls = new List<DocumentContentControl>();

            using (WordprocessingDocument myDocument = WordprocessingDocument.Open(fileName, true))
            {
                #region List<DocumentContentControl>
                // Create a Body object.  
                DocumentFormat.OpenXml.Wordprocessing.Body body = myDocument.MainDocumentPart.Document.Body;

                foreach (var item in body.ChildElements)
                {
                    if (item is Paragraph)//если поле
                    {
                        foreach (var item1 in item.Where(n => n is SdtRun))//n => n.GetType() == typeof(SdtRun)
                        {
                            documentContentControls.Add(new FieldContentControl(item1.Elements<SdtProperties>().FirstOrDefault().Elements<Tag>().FirstOrDefault().Val));
                        }
                    }

                    if (item is SdtBlock)//если список или таблица
                    {
                        ListContentControl documentList = new ListContentControl(item.ChildElements.First<SdtProperties>().Elements<Tag>().FirstOrDefault().Val); ;

                        foreach (var item1 in item.ChildElements.First<SdtContentBlock>().ChildElements)
                        {
                            if (item1 is SdtBlock)//если простой список или список с таблицей
                            {

                                if (item1.ChildElements.First<SdtContentBlock>().Where(n => n is Table).FirstOrDefault() is Table)//если список с таблицей
                                {
                                    foreach (var item2 in item1.ChildElements.First<SdtContentBlock>().ChildElements.FirstOrDefault().Where(n => n is TableRow))
                                    {
                                        TableContentControl documentTable = new TableContentControl(item1.ChildElements.First<SdtProperties>().Elements<Tag>().FirstOrDefault().Val);
                                        foreach (var item3 in item2.Where(n => n is TableCell))
                                        {
                                            foreach (var item4 in item3.ChildElements)
                                            {
                                                if (item4 is Paragraph)//если поле, вложенное в параграф
                                                {
                                                    foreach (var item5 in item4.Where(n => n is SdtRun))
                                                    {
                                                        documentTable.Fields.Add(new FieldContentControl(item5.Elements<SdtProperties>().FirstOrDefault().Elements<Tag>().FirstOrDefault().Val));
                                                    }
                                                }

                                                if (item4 is SdtBlock)//если поле
                                                {
                                                    documentTable.Fields.Add(new FieldContentControl(item4.Elements<SdtProperties>().FirstOrDefault().Elements<Tag>().FirstOrDefault().Val));
                                                }
                                            }
                                        }

                                        if (documentTable.Fields.Count != 0)
                                        {
                                            documentList.Tables.Add(documentTable);
                                        }
                                    }
                                }
                                else//если простой список
                                {
                                    documentList.Fields.Add(new FieldContentControl(item1.Elements<SdtProperties>().FirstOrDefault().Elements<Tag>().FirstOrDefault().Val));
                                }
                            }

                            if (item1 is Paragraph)//если список с полями
                            {
                                foreach (var item2 in item1.Where(n => n is SdtRun))
                                {
                                    documentList.Fields.Add(new FieldContentControl(item2.Elements<SdtProperties>().FirstOrDefault().Elements<Tag>().FirstOrDefault().Val));
                                }
                            }

                            if (item1 is Table)//если таблица
                            {
                                foreach (var item2 in item1.Where(n => n is TableRow))
                                {
                                    TableContentControl documentTable = new TableContentControl(item.ChildElements.First<SdtProperties>().Elements<Tag>().FirstOrDefault().Val);

                                    foreach (var item3 in item2.Where(n => n is TableCell))
                                    {
                                        foreach (var item4 in item3.ChildElements)
                                        {
                                            if (item4 is Paragraph)//если поле, вложенное в параграф
                                            {
                                                foreach (var item5 in item4.Where(n => n is SdtRun))
                                                {
                                                    documentTable.Fields.Add(new FieldContentControl(item5.Elements<SdtProperties>().FirstOrDefault().Elements<Tag>().FirstOrDefault().Val));
                                                }
                                            }

                                            if (item4 is SdtBlock)//если поле
                                            {
                                                documentTable.Fields.Add(new FieldContentControl(item4.Elements<SdtProperties>().FirstOrDefault().Elements<Tag>().FirstOrDefault().Val));
                                            }
                                        }
                                    }

                                    if (documentTable.Fields.Count != 0)
                                    {
                                        documentContentControls.Add(documentTable);
                                    }
                                }
                            }
                        }

                        if (documentList.Fields.Count != 0)
                        {
                            documentContentControls.Add(documentList);
                        }
                    }
                }
                #endregion
            }

            return documentContentControls;
            #endregion
        }

        public static List<DocumentData> CompareContentControls(string fileName, List<DocumentContentControl> documentContentControls, List<DocumentData> documentDatasFromJson)
        {
            List<DocumentData> newDocumentDatas = new List<DocumentData>();
            //записываю в new Data только те данные, которые соответствуют ContentControls в документе
            foreach (var item in documentContentControls)
            {
                if (item is FieldContentControl)
                {
                    FieldDocumentData fieldDocumentData = null;
                    foreach (var item1 in documentDatasFromJson)
                    {
                        if (item1 is FieldDocumentData)
                        {
                            if ((item1 as FieldDocumentData).Key == item.Field)
                            {
                                fieldDocumentData = (item1 as FieldDocumentData);
                                //documentDatasFromJson.Remove(item1);
                                break;
                            }
                        }
                    }
                    if (fieldDocumentData != null)
                    {
                        newDocumentDatas.Add(fieldDocumentData);
                    }

                }

                if (item is ListContentControl)
                {
                    ListDocumentData listDocumentData = null;
                    foreach (var item1 in documentDatasFromJson)
                    {
                        if (item1 is ListDocumentData)
                        {
                            if ((item1 as ListDocumentData).Key == item.Field)
                            {
                                listDocumentData = (item1 as ListDocumentData);
                                documentDatasFromJson.Remove(item1);
                                break;
                            }
                        }
                    }
                    if (listDocumentData != null)
                    {
                        newDocumentDatas.Add(listDocumentData);
                    }
                }

                if (item is TableContentControl)
                {
                    TableDocumentData tableDocumentData = null;
                    foreach (var item1 in documentDatasFromJson)
                    {
                        if (item1 is TableDocumentData)
                        {
                            if ((item1 as TableDocumentData).Key == item.Field)
                            {
                                tableDocumentData = (item1 as TableDocumentData);
                                documentDatasFromJson.Remove(item1);
                                break;
                            }
                        }
                    }
                    if (tableDocumentData != null)
                    {
                        newDocumentDatas.Add(tableDocumentData);
                    }
                }
            }

            return newDocumentDatas;
        }

        public static void FillContentControls(string fileName, List<DocumentData> newDocumentDatas)
        {
            List<IContentItem> contentItems = new List<IContentItem>();

            foreach (var item in newDocumentDatas)
            {
                if (item is FieldDocumentData)
                {
                    contentItems.Add(new FieldContent((item as FieldDocumentData).Key, (item as FieldDocumentData).Value));
                }

                if (item is ListDocumentData)
                {
                    ListContent listContent = new ListContent(item.Key);

                    if ((item as ListDocumentData).Fields.Count != 0)
                    {
                        for (int i = 0; i < (item as ListDocumentData).Fields[0].Values.Count; i++)
                        {
                            ListItemContent listcontentItems = new ListItemContent();

                            foreach (var item1 in (item as ListDocumentData).Fields)
                            {
                                listcontentItems.AddField((item1 as FieldsDocumentData).Key, (item1 as FieldsDocumentData).Values[i]);
                            }

                            foreach (var item1 in (item as ListDocumentData).Tables)
                            {
                                TableContent tableContent = new TableContent(item1.Key);

                                if ((item1 as TableDocumentData).Fields.Count != 0)
                                {
                                    for (int j = ((item1 as TableDocumentData).Fields[0].Values.Count / (item as ListDocumentData).Fields[0].Values.Count) * i; j < ((item1 as TableDocumentData).Fields[0].Values.Count / (item as ListDocumentData).Fields[0].Values.Count) + (((item1 as TableDocumentData).Fields[0].Values.Count / (item as ListDocumentData).Fields[0].Values.Count) * i); j++)
                                    {
                                        List<IContentItem> tableRowContent = new List<IContentItem>();

                                        foreach (var item2 in (item1 as TableDocumentData).Fields)
                                        {
                                            tableRowContent.Add(new FieldContent((item2 as FieldsDocumentData).Key, (item2 as FieldsDocumentData).Values[j]));
                                        }

                                        tableContent.AddRow(tableRowContent.ToArray());
                                    }
                                }

                                listcontentItems.AddTable(tableContent);
                            }

                            listContent.AddItem(listcontentItems);
                        }
                    }

                    contentItems.Add(listContent);
                }

                if (item is TableDocumentData)
                {
                    TableContent tableContent = new TableContent(item.Key);

                    if ((item as TableDocumentData).Fields.Count != 0)
                    {
                        for (int i = 0; i < (item as TableDocumentData).Fields[0].Values.Count; i++)
                        {
                            List<IContentItem> tableRowContent = new List<IContentItem>();

                            foreach (var item1 in (item as TableDocumentData).Fields)
                            {
                                tableRowContent.Add(new FieldContent((item1 as FieldsDocumentData).Key, (item1 as FieldsDocumentData).Values[i]));
                            }

                            tableContent.AddRow(tableRowContent.ToArray());
                        }
                    }

                    contentItems.Add(tableContent);
                }
            }

            var valuesToFill = new Content(contentItems.ToArray());

            using (var outputDocument = new TemplateProcessor(fileName).SetRemoveContentControls(true))//true false
            {
                outputDocument.FillContent(valuesToFill);
                outputDocument.SaveChanges();
            }
        }
    }
}