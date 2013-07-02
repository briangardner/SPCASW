using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPCASW.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class HeaderTextAttribute : Attribute
    {
        public HeaderTextAttribute(string text)
        {
            this.Text = text;
        }

        public string Text { get; set; }
    }
}