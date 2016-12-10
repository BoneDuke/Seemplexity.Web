using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seemplexity.Web.Models
{
    public class EmailModel
    {
        public EmailModel()
        {
            Data = new Dictionary<string, string>();
            To = new List<string>();
        }

        public string Subject { get; set; }
        public IList<string> To { get; set; }
        public IDictionary<string, string> Data { get; set; }

        public List<TuristViewModel> Turists { get; set; }
    }
}