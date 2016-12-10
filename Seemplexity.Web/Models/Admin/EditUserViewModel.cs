using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Seemplexity.BusinesLogic.Model;

namespace Seemplexity.Web.Models.Admin
{
    public class EditUserViewModel
    {
        public UserViewModel User { get; set; }
        public List<string> Roles { get; set; }

        public string[] SelectedRoleNames { get; set; }
    }
}