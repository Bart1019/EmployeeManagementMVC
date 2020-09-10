using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EmployeeManagement.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Hosting.Internal;

namespace EmployeeManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddPassword()
        {
            var user = await _userManager.GetUserAsync(User);

            var userHasPassword = await _userManager.HasPasswordAsync(user);

            if (userHasPassword) return RedirectToAction("ChangePassword");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPassword(AddPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                var result = await _userManager.AddPasswordAsync(user, model.NewPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
                    return View();
                }

                await _signInManager.RefreshSignInAsync(user);

                return View("AddPasswordConfirmation");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);

            var userHasPassword = await _userManager.HasPasswordAsync(user);

            if (!userHasPassword) return RedirectToAction("AddPassword");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null) return RedirectToAction("Login");

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);

                    return View();
                }

                await _signInManager.RefreshSignInAsync(user);

                return View("ChangePasswordConfirmation");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null) return RedirectToAction("index", "home");

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"The User ID {userId} is invalid";
                return View("Error");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded) return View();

            ViewBag.ErrorTitle = "Email cannot be confirmed";
            return View("Error");
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new {ReturnUrl = returnUrl});

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            var loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins =
                    (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty,
                    $"Error from external provider: {remoteError}");

                return View("Login", loginViewModel);
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError(string.Empty, "Error loading external login information.");

                return View("Login", loginViewModel);
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            IdentityUser user = null;

            if (email != null)
            {
                user = await _userManager.FindByEmailAsync(email);

                if (user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet");

                    return View("Login", loginViewModel);
                }
            }

            var signInResult =
                await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, true);

            if (signInResult.Succeeded) return LocalRedirect(returnUrl);

            if (email != null)
            {
                if (user == null)
                {
                    user = new IdentityUser
                    {
                        UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };

                    await _userManager.CreateAsync(user);

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new {userId = user.Id, token},
                        Request.Scheme);

                    try
                    {
                        BuildEmailTemplate("Email Confirmation link", $"Hello, thank you for your registration, in order to confirm it click in the link: {confirmationLink}", user.Email);
                    }
                    catch (Exception e)
                    {
                        await _userManager.DeleteAsync(user);
                        Console.WriteLine(e);
                        throw;
                    }

                    ViewBag.ErrorTitle = "Registration successful";
                    ViewBag.ErrorMessage = "Before you can Login, please confirm your " +
                                           "email, by clicking on the confirmation link we have emailed you";
                    return View("RegisterSuccessful");
                }

                await _userManager.AddLoginAsync(user, info);
                await _signInManager.SignInAsync(user, false);

                return LocalRedirect(returnUrl);
            }

            ViewBag.ErrorTitle = $"Email claim not received from: {info.LoginProvider}";
            ViewBag.ErrorMessage = "Please contact support on bart1019@gmail.com";

            return View("Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null && await _userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var passwordResetLink = Url.Action("ResetPassword", "Account", new {email = model.Email, token},
                        Request.Scheme);

                    try
                    {
                        BuildEmailTemplate("Password reset link", $"Hello, in order to reset your current password click the link: {passwordResetLink}", user.Email);
                    }
                    catch (Exception e)
                    {
                        await _userManager.DeleteAsync(user);
                        Console.WriteLine(e);
                        throw;
                    }

                    return View("ForgotPasswordConfirmation");
                }

                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string emailAddress)
        {
            var user = await _userManager.FindByEmailAsync(emailAddress);

            if (user == null) return Json(true);

            return Json($"Email {emailAddress} is already used!");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.EmailAddress);

                if (user != null && !user.EmailConfirmed && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                    return View(model);
                }

                var result =
                    await _signInManager.PasswordSignInAsync(model.EmailAddress, model.Password, model.RememberMe,
                        true);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                        return LocalRedirect(returnUrl);
                    return RedirectToAction("Index", "Home");
                }

                if (result.IsLockedOut) return View("AccountLocked");

                ModelState.AddModelError(string.Empty, "Invalid login attempt");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser {UserName = model.EmailAddress, Email = model.EmailAddress};

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new {userId = user.Id, token},
                        Request.Scheme);

                    try
                    {
                        BuildEmailTemplate("Email Confirmation link", $"Hello, thank you for your registration, in order to confirm it click in the link:{confirmationLink}", user.Email);
                    }
                    catch (Exception e)
                    {
                        await _userManager.DeleteAsync(user);
                        Console.WriteLine(e);
                        throw;
                    }
                    
                    if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                        return RedirectToAction("ListUsers", "Administration");

                    ViewBag.ErrorTitle = "Registration successful !";
                    ViewBag.ErrorMessage = "Before you can Login, please confirm your " +
                                           "email, by clicking on the confirmation link we have emailed you.";
                    return View("RegisterSuccessful");
                }

                foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null) ModelState.AddModelError("", "Invalid password reset token");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

                    if (result.Succeeded)
                    {
                        if (await _userManager.IsLockedOutAsync(user))
                            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);

                        return View("ResetPasswordConfirmation");
                    }

                    foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);

                    return View(model);
                }

                return View("ResetPasswordConfirmation");
            }

            return View(model);
        }

        public static void BuildEmailTemplate(string subjectText, string bodyText, string sendTo)
        {
            string from, to, bcc, cc, subject, body;
            from = "YourEmail@gmail.com";
            to = sendTo.Trim();
            bcc = "";
            cc = "";
            subject = subjectText;
            StringBuilder sb = new StringBuilder();
            
            sb.Append(bodyText);
            body = sb.ToString();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));
            if (!string.IsNullOrEmpty(bcc))
            {
                mail.Bcc.Add(new MailAddress(bcc));
            }
            if (!string.IsNullOrEmpty(cc))
            {
                mail.CC.Add(new MailAddress(cc));
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SendEmail(mail);
        }

        public static void SendEmail(MailMessage mail)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential("your email - email@domain.com", "your email password");
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}