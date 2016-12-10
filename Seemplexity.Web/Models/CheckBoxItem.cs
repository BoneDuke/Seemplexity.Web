using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seemplexity.Web.Models
{
    public class CheckBoxItem
    {
        public string Id { get; set; }        // value of a checkbox
        public string Name { get; set; }      // name of a checkbox
        public object Tags { get; set; }      // Object of html tags to be applied to checkbox, e.g.: 'new { tagName = "tagValue" }'
    }
}