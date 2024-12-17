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

                    var passwordResetLink = Url.Action("ResetPassword", "Accounts", new { email = model.Email, token = token }, Request.Scheme);

                    //_logger.Log(LogLevel.Warning, passwordResetLink);

                    return View("ForgotPasswordConfirmation");
                }

                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            var userHasPassword = await _userManager.HasPasswordAsync(user);

            if (userHasPassword)
            {
                return View("ChangePassword");
            }
            else
            {
                RedirectToAction("AddPassword");
                return View("AddPassword");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }
                await _signInManager.RefreshSignInAsync(user);
                return View("ChangePasswordConfirmation");
            }
            return View(model);
        }

    }
}
