using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MvsApp.Models
{
    [DataContract]
    public class TableDocumentData : DocumentData
    {
        public TableDocumentData(string key) : base(key)
        {
            Fields = new List<FieldsDocumentData>();
        }

        public TableDocumentData(string key, List<FieldsDocumentData> fields) : base(key)
        {
            Fields = fields;
        }

        [DataMember]
        public List<FieldsDocumentData> Fields { get; set; }

        //public override void Show()
        //{
        //    Console.WriteLine(this.Key);

        //    foreach (var item in Fields)
        //    {
        //        Console.Write(item.Key + " ");
        //    }
        //    Console.WriteLine();
        //    //foreach (var item in Fields)
        //    //{
        //    //    for (int i = 0; i < item.Values.Count; i++)
        //    //    {
        //    //        Console.Write(item.Values[i] + " ");
        //    //    }
        //    //    Console.WriteLine();
        //    //}

        //    for (int i = 0; i < this.Fields[0].Values.Count; i++)
        //    {
        //        foreach (var item in this.Fields)
        //        {
        //            Console.Write(item.Values[i] + " ");
        //        }
        //        Console.WriteLine();
        //    }
        //}
    }
}