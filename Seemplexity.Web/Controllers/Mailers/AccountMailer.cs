using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Mvc.Mailer;
using Seemplexity.Web.Models;

namespace Seemplexity.Web.Controllers.Mailers
{
    public sealed class AccountMailer : MailerBase, IIdentityMessageService
    {

        public AccountMailer()
        {
            MasterName = "_Layout";
        }

        private MvcMailMessage GetMessage(EmailModel model, string viewModel, IEnumerable<KeyValuePair<string, string>> addLinkedResources = null)
        {
            var message = new MvcMailMessage();

            foreach (var sendTo in model.To)
                message.To.Add(sendTo);

            message.From = new MailAddress(ConfigurationManager.AppSettings["EMailAddressFrom"], Resources.Resources.BalkanExpress);
            message.Subject = model.Subject;
            ViewData.Model = model;

            var resources = new Dictionary<string, string>();
            //resources["email_footer"] = HttpContext.Current.Server.MapPath("~/Content/images/email_footer_800.jpg");

            if (addLinkedResources != null)
            {
                foreach (KeyValuePair<string, string> resource in addLinkedResources)
                {
                    resources.Add(resource.Key, resource.Value);
                }
            }

            PopulateBody(message, viewModel, resources);

            return message;
        }

        public MvcMailMessage ConfirmEmail(EmailModel model)
        {
            return GetMessage(model, "ConfirmEmail");
        }

        public MvcMailMessage BookingEmail(EmailModel model)
        {
            return GetMessage(model, "BookingEmail");
        }

        public async Task SendAsync(IdentityMessage message)
        {
            var model = new EmailModel()
            {
                Subject = Resources.Resources.ConfirmAccount
            };
            model.To.Add(message.Destination);
            model.Data.Add(new KeyValuePair<string, string>("url", message.Body));

            await ConfirmEmail(model).SendAsync();
        }


    }
}