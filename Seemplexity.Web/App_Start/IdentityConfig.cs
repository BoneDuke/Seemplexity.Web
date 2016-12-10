using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services.Description;
using System.Web.WebPages;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Mvc.Mailer;
using Seemplexity.Web.Controllers;
using Seemplexity.Web.Controllers.Mailers;
using Seemplexity.Web.Models;

namespace Seemplexity.Web
{
    public class EmailService : MailerBase, IIdentityMessageService
    {
        private static readonly string Host;
        private static readonly int Port;
        private static readonly bool IsSsl;
        private static readonly string UserName;
        private static readonly string UserPass;

        static EmailService()
        {
            Host = ConfigurationManager.AppSettings["mailHost"];
            Port = int.Parse(ConfigurationManager.AppSettings["mailPort"]);
            IsSsl = bool.Parse(ConfigurationManager.AppSettings["mailEnableSsl"]);
            UserName = ConfigurationManager.AppSettings["mailUserName"];
            UserPass = ConfigurationManager.AppSettings["mailPassword"];
        }

        private MvcMailMessage GetMessage(EmailModel model, string viewModel, IEnumerable<KeyValuePair<string, string>> addLinkedResources = null)
        {
            var message = new MvcMailMessage();

            foreach (string sendTo in model.To)
                message.To.Add(sendTo);

            message.From = new MailAddress("admin@chudopridet.ru", "Анонимный дед мороз");
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

        public async Task SendAsync(IdentityMessage message)
        {
            //await ConfigSendGridasync(message);

            // Configure the client:
            var client = new SmtpClient(Host)
            {
                Port = Port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = IsSsl
            };

            // Creatte the credentials:
            var credentials = new NetworkCredential(UserName, UserPass);
            client.Credentials = credentials;

            // Create the message:
            var mail = new MailMessage(UserName, message.Destination)
            {
                Subject = message.Subject,
                Body = message.Body
            };

            await client.SendMailAsync(mail);
            
        }
        
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6
                //RequiredLength = 6,
                //RequireNonLetterOrDigit = true,
                //RequireDigit = true,
                //RequireLowercase = true,
                //RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new AccountMailer(); //new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
