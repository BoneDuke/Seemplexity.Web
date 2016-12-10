using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Seemplexity.Web.Models.Admin
{
    public class UserViewModel
    {
        public string Id { get; set; }
        [DisplayName(@"Имя пользователя")]
        public string UserName { get; set; }
        [DisplayName(@"EMail")]
        public string EMail { get; set; }
        [Display(Name = "EMailConfirmed", ResourceType = typeof(Resources.Resources))]
        public bool EMailConfirmed { get; set; }
        [DisplayName(@"Номер телефона")]
        public string PhoneNumber { get; set; }
        [DisplayName(@"Роли")]
        public List<string> RoleNames { get; set; }
    }
}