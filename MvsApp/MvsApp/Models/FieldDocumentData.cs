using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MvsApp.Models
{
    [DataContract]
    public class FieldDocumentData : DocumentData
    {
        public FieldDocumentData(string key) : base(key)
        {
        }

        public FieldDocumentData(string key, string value) : base(key)
        {
            Value = value;
        }

        [DataMember]
        public string Value { get; set; }

        //public override void Show()
        //{
        //    Console.WriteLine(this.Key + " - " + this.Value);
        //}
    }
}