using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MvsApp.Models
{
    [DataContract]
    [KnownType("GetKnownTypes")]
    public abstract class DocumentData
    {
        public DocumentData(string key)
        {
            Key = key;
        }

        [DataMember]
        public string Key { get; set; }

        //public virtual void Show()
        //{
        //    Console.WriteLine(this.Key);
        //}

        static Type[] GetKnownTypes()
        {
            return new Type[] { typeof(FieldDocumentData), typeof(ListDocumentData), typeof(TableDocumentData), typeof(FieldsDocumentData) };
        }
    }
}