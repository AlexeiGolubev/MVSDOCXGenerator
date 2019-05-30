using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvsApp.Models
{
    public class TableContentControl : DocumentContentControl
    {
        public TableContentControl(string field) : base(field)
        {
            Fields = new List<FieldContentControl>();
        }

        public TableContentControl(string field, List<FieldContentControl> fields) : base(field)
        {
            Fields = fields;
        }

        public List<FieldContentControl> Fields { get; set; }

        //public override void Show()
        //{
        //    Console.WriteLine(this.Field);

        //    foreach (var item in Fields)
        //    {
        //        Console.WriteLine(item);
        //    }
        //}
    }
}