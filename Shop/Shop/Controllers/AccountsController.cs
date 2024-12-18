using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.ApplicationServices.Services;
using Shop.Core.Domain;
using Shop.Core.Dto;
using Shop.Core.ServiceInterface;
using Shop.Data;
using Shop.Models;
using Shop.Models.Accaunts;
using Shop.Models.Accounts;
using System.Diagnostics;
using System.Security.Claims;

namespace Shop.Controllers
{
    public class AccountsController : Controller
    {
        // Dependency Injection: UserManager and SignInManager services to manage users and authentication
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailServices _emailsServices;

        // Constructor to initialize the services through dependency injection
        public AccountsController
            (
            
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailServices emailsServices
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailsServices = emailsServices; ;


        }

        // GET: Render the registration form
        [HttpGet]
        public IActionResult Register()
        {
            return View(); // Returns the registration view
        }

        // POST: Handle the user registration process
        [HttpPost]
        [AllowAnonymous] // Allow access without authentication
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            // Check if the form data is valid
            if (ModelState.IsValid)
            {
                // Create a new user with the provided data
                var user = new ApplicationUser
                {
                    UserName = vm.Email,
                    Name = vm.Name,
                    Email = vm.Email,
                    City = vm.City
                };

                // Attempt to create the user in the database
                var result = await _userManager.CreateAsync(user, vm.Password);

                if (result.Succeeded) // If user creation is successful
                {
                    // Generate an email confirmation token
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    // Create the email confirmation link
                    var confirmationLink = Url.Action("ConfirmEmail", "Accounts", new { userId = user.Id, token = token }, Request.Scheme);

                    EmailTokenDto newsignup = new();
                    newsignup.Token = token;
                    newsignup.Body = $"Thank you for registrating: <a href=\"{confirmationLink}\">click here</a>";
                    newsignup.Subject = "CRUD registration";
                    newsignup.To = user.Email;

                    // Check if the current user is an admin
                    if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        // Redirect admins to the list of users
                        return RedirectToAction("ListUsers", "Administrations");
                    }

                    // Inform the user about email confirmation
                    _emailsServices.SendEmailToken(newsignup, token);
                    List<string> errordatas =
                        [
                        "Area", "Accounts",
                        "Issue", "Success",
                        "StatusMessage", "Registration Success",
                        "ActedOn", $"{vm.Email}",
                        "CreatedAccountData", $"{vm.Email}\n{vm.City}\n[password hidden]\n[password hidden]"
                        ];
                    ViewBag.ErrorDatas = errordatas;
                    ViewBag.ErrorTitle = "You have successfully registered";
                    ViewBag.ErrorMessage = "Before you can log in, please confirm email from the link" +
                        "\nwe have emailed to your email address.";
                    return View("ConfirmEmail");
                }

                // Add any errors to the model state to display on the form
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            // Return the form with validation errors if any
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null) { return RedirectToAction("Index", "Home"); }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The user with id of {userId} is not valid";
                return View("NotFound");
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            List<string> errordatas = [
                "Area", "Accounts",
                "Issue", "Failure",
                "StatusMessage", "Confirmation Failure",
                "ActionId", $"{user.Email}\n{user.City}\n[password hidden]\n[password hidden]"
            ];


            if (result.Succeeded)
            {
                errordatas = [
                    "Area", "Accounts",
                    "Issue", "Success",
                    "StatusMessage", "Confirmation Success",
                    "ActedOn", $"{user.Email}",
                    "CreatedAccountData", $"{user.Email}\n{user.City}\n[password hidden]\n[password hidden]"
                ];
                ViewBag.ErrorDatas = errordatas;
                return View();
            }

            ViewBag.ErrorDatas = errordatas;
            ViewBag.ErrorTitle = "Email cannot be confirmed";
            ViewBag.ErrorMessage = $"The user's email, with userid of {userId}, cannot be confirmed.";
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        // GET: Render the login form
        [HttpGet]
        [AllowAnonymous] // Allow access without authentication
        public async Task<IActionResult> Login(string? returnUrl)
        {
            // Prepare the login view model with external login providers
            LoginViewModel vm = new()
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            return View(vm); // Return the login view
        }

        // POST: Handle the user login process
        [HttpPost]
        [AllowAnonymous] // Allow access without authentication
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            // Populate the external login providers
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            // Validate the form data
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await _userManager.FindByEmailAsync(model.Email);

                // Check if email is not confirmed but password is correct
                if (user != null && !user.EmailConfirmed && (await _userManager.CheckPasswordAsync(user, model.Password)))
                {
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                    return View(model);
                }

                // Attempt to sign in the user
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);

                // Log the result for debugging purposes
                if (!result.Succeeded)
                {
                    // Log or debug the result to identify the issue
                    Console.WriteLine($"Login failed. Result: {result}");
                }

                // If login is successful
                if (result.Succeeded)
                {
                    // Redirect to returnUrl if provided and local, otherwise to home page
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                // If the account is locked
                if (result.IsLockedOut)
                {
                    return View("AccountLocked");  // Render the account locked view
                }

                // Invalid login attempt
                ModelState.AddModelError("", "Invalid Login attempt");
            }

            // Return the form with errors if validation failed
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
        public IActionResult ResetPassword(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Invalid password reset token.");
                return View("Error");
            }

            // Pass the token and email to the view
            var model = new ResetPasswordViewModel
            {
                Token = token,
                Email = email
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Find the user by email
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            // Reset the password
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                // Unlock the user if the account was locked
                if (await _userManager.IsLockedOutAsync(user))
                {
                    await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                }

                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            // Add errors to the ModelState
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or email not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // Generate password reset token
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                // Generate reset link
                var resetLink = Url.Action(
                    "ResetPassword",
                    "Accounts",
                    new { email = user.Email, token = token },
                    Request.Scheme);

                // Send the reset link via email
                var emailDto = new EmailTokenDto
                {
                    To = user.Email,
                    Subject = "Password Reset Request",
                    Body = $"Please reset your password by clicking <a href='{resetLink}'>here</a>."
                };

                _emailsServices.SendEmailToken(emailDto, token);

                // Show confirmation page
                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string? returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Accounts", new { returnUrl = returnUrl });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            LoginViewModel loginViewModel = new()
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Problem with external provider : {remoteError}");

                return View("Login", loginViewModel);
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError("", "Error loading external login information.");

                return View("Login", loginViewModel);
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = null;

            if (email != null)
            {
                user = await _userManager.FindByEmailAsync(email);

                //if (user != null && !user.EmailConfirmed)
                //{
                //	ModelState.AddModelError("", "Email not confirmed yet");
                //	return View("Login", loginViewModel);
                //}
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey,
                                                                            isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                if (email != null)
                {
                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),

                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),

                            City = "Unset",
                        };

                        await _userManager.CreateAsync(user);

                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                        var confirmationLink = Url.Action("ConfirmEmail", "Accounts", new { userId = user.Id, token = token }, Request.Scheme);

                        //logger.Log(LogLevel.Warning, confirmationLink);

                        ViewBag.ErrorTitle = "Registration successful";
                        ViewBag.ErrorMessage = "Before you can login, please verify your email address. Verification link has been sent to your email";

                        return View("Error");
                    }

                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }

                ViewBag.ErrorTitle = $"Email claim not received from: {info.LoginProvider}";
                ViewBag.ErrorMessage = "Please contact support at asd@asd.com";

                return View("Error");
            }
        }

    }
}
