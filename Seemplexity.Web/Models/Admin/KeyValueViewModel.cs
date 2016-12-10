using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Seemplexity.Web.Models.Admin
{
    public class KeyValueViewModel
    {
        [DisplayName(@"Ключ записи")]
        public int Id { get; set; }
        [DisplayName(@"Название")]
        public string Name { get; set; }
    }
}