using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MvsApp.Models
{
    [DataContract]
    public class FieldsDocumentData : DocumentData
    {
        public FieldsDocumentData(string key) : base(key)
        {
            Values = new List<string>();
        }

        public FieldsDocumentData(string key, List<string> values) : base(key)
        {
            Values = values;
        }

        [DataMember]
        public List<string> Values { get; set; }

        //public override void Show()
        //{
        //    Console.WriteLine(this.Key);
        //    foreach (var item in Values)
        //    {
        //        Console.Write(item + " ");
        //    }
        //    Console.WriteLine();
        //}
    }
}