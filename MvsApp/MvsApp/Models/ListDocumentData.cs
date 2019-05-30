using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MvsApp.Models
{
    [DataContract]
    public class ListDocumentData : DocumentData
    {
        public ListDocumentData(string key) : base(key)
        {
            Fields = new List<FieldsDocumentData>();
            Tables = new List<TableDocumentData>();
        }

        public ListDocumentData(string key, List<FieldsDocumentData> fields, List<TableDocumentData> tables) : base(key)
        {
            Fields = fields;
            Tables = tables;
        }

        [DataMember]
        public List<FieldsDocumentData> Fields { get; set; }

        [DataMember]
        public List<TableDocumentData> Tables { get; set; }

        //public override void Show()
        //{
        //    Console.WriteLine(this.Key);

        //    //foreach (var item in Fields)
        //    //{
        //    //    item.Show();
        //    //}

        //    foreach (var item in Fields)
        //    {
        //        Console.Write(item.Key + " ");
        //    }
        //    Console.WriteLine();

        //    for (int i = 0; i < this.Fields[0].Values.Count; i++)
        //    {
        //        foreach (var item in this.Fields)
        //        {
        //            Console.Write(item.Values[i] + " ");
        //        }
        //        Console.WriteLine();
        //    }

        //    foreach (var item in Tables)
        //    {
        //        item.Show();
        //    }
        //}
    }
}