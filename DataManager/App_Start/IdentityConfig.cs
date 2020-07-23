using DataManager.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DataManager
{
    public class EmailService : IIdentityMessageService
    {
        #region Methods
        public async Task SendAsync(IdentityMessage message)
        {
            MailMessage mail = new MailMessage(new MailAddress("postmaster@cagrownnurserymarketplace.com"), new MailAddress(message.Destination))
            {
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = true
            };
            using (var client = new SmtpClient())
            {
                await client.SendMailAsync(mail);
            }
        }
        #endregion
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<AspNetUser>
    {
        #region Methods
        public ApplicationUserManager(IUserStore<AspNetUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            ApplicationUserManager manager = new ApplicationUserManager(new UserStore<AspNetUser>(context.Get<DataContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<AspNetUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                //RequireNonLetterOrDigit = true,
                RequireDigit = true,
                //RequireLowercase = true,
                //RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            manager.EmailService = new EmailService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<AspNetUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
        #endregion
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<AspNetUser, string>
    {
        #region Methods
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(AspNetUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
        #endregion
    }
}
