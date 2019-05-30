using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvsApp.Models
{
    public abstract class DocumentContentControl
    {
        public DocumentContentControl(string field)
        {
            Field = field;
        }

        public string Field { get; set; }

        public override string ToString()
        {
            return Field;
        }

        //public virtual void Show()
        //{
        //    Console.WriteLine(this.Field);
        //}
    }
}