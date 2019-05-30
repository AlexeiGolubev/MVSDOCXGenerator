using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvsApp.Models
{
    public class ListContentControl : DocumentContentControl
    {
        public ListContentControl(string field) : base(field)
        {
            Fields = new List<FieldContentControl>();
            Tables = new List<TableContentControl>();
        }

        public ListContentControl(string field, List<FieldContentControl> fields, List<TableContentControl> tables) : base(field)
        {
            Fields = fields;
            Tables = tables;
        }

        public List<FieldContentControl> Fields { get; set; }
        public List<TableContentControl> Tables { get; set; }

        //public override void Show()
        //{
        //    Console.WriteLine(this.Field);

        //    foreach (var item in Fields)
        //    {
        //        Console.WriteLine(item);
        //    }

        //    Console.WriteLine();

        //    foreach (var item in Tables)
        //    {
        //        item.Show();
        //    }
        //}
    }
}