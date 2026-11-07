using CinemaTicketBooking.View_Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CinemaTicketBooking.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        readonly private UserManager<ApplicationUser> _userManager;
        readonly private SignInManager<ApplicationUser> _signInManager;
        readonly private IReposetory<ApplicationUserOTP> _applicationUserOTP;
        readonly private IEmailSender _emailSender;



        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, IReposetory<ApplicationUserOTP> applicationUserOTP)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _applicationUserOTP = applicationUserOTP;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", registerVM);
            }
            ApplicationUser user = new ApplicationUser()
            {
                Name = registerVM.Name,
                Address = registerVM.Address,
                UserName = registerVM.UserName,
                Email = registerVM.Email
            };
            var result = await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("Register", registerVM);
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var Link = Url.Action("ConfirmEmail", "Account", new { area = "Identity", id = user.Id, token }, Request.Scheme);

            await _emailSender.SendEmailAsync(
                registerVM.Email,
                "Confirm your email CINEMA",
                $"<h1>Welcome to CINEMA!</h1><p>Please confirm your email address by clicking <a href='{Link}'>here</a>.</p>");
            TempData["Successful_Notification"] = "تم إنشاء الحساب بنجاح، يرجى تسجيل الدخول!";
            return RedirectToAction("Login");
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string id, string token)
        {
            var user = await _userManager.FindByIdAsync(id);
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                TempData["Error_Notification"] = "حدث خطأ أثناء تأكيد البريد الإلكتروني. يرجى المحاولة مرة أخرى.";
                return RedirectToAction("Login");
            }
            TempData["Successful_Notification"] = "تم تأكيد البريد الإلكتروني بنجاح. يمكنك الآن تسجيل الدخول.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", loginVM);
            }
            var user = await _userManager.FindByNameAsync(loginVM.UserNameorEmail) ?? await _userManager.FindByEmailAsync(loginVM.UserNameorEmail);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View("Login", loginVM);
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Your account is locked. Please try again later.");
                    return View("Login", loginVM);
                }
                else if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "pls confirm email first.");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");

                }
                return View("Login", loginVM);
            }
            TempData["Successful_Notification"] = " تم تسجيل الدخول بنجاح ";

            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        [HttpGet]
        public IActionResult ResendEmailConfirmation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationVM resendEmailConfirmation)
        {
            if (!ModelState.IsValid)
            {
                return View(resendEmailConfirmation);
            }
            var user = await _userManager.FindByNameAsync(resendEmailConfirmation.UserNameOrEmail) ?? await _userManager.FindByEmailAsync(resendEmailConfirmation.UserNameOrEmail);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid UserNameOrEmail");
                return View(resendEmailConfirmation);
            }
            if (user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Email is already confirmed.");
                return View(resendEmailConfirmation);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var Link = Url.Action("ConfirmEmail", "Account", new { area = "Identity", id = user.Id, token }, Request.Scheme);

            await _emailSender.SendEmailAsync(
                user.Email,
                "Confirm your email CINEMA",
                $"<h1>Welcome to CINEMA!</h1><p>Please confirm your email address by clicking <a href='{Link}'>here</a>.</p>");
            TempData["Successful_Notification"] = "تم ارسال رابط التأكيد إلى بريدك الإلكتروني!";


            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM forgetPasswordVM)
        {
            if (!ModelState.IsValid)
            {
                return View("ForgetPassword", forgetPasswordVM);
            }
            var user = await _userManager.FindByNameAsync(forgetPasswordVM.UserNameOrEmail) ?? await _userManager.FindByEmailAsync(forgetPasswordVM.UserNameOrEmail);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid UserNameOrEmail");
                return View("ForgetPassword", forgetPasswordVM);
            }

            var OTP = new Random().Next(1000, 9999).ToString();

            var applicationUserOTP = new ApplicationUserOTP(user.Id, OTP);

            await _applicationUserOTP.AddAsync(applicationUserOTP);
            await _applicationUserOTP.SaveAsync();

            await _emailSender.SendEmailAsync(
                user.Email,
                "Reset your password CINEMA",
                $"<h1>use this {OTP} to Reset Password</h1>");
            TempData["Successful_Notification"] = "تم ارسال رابط إعادة تعيين كلمة المرور إلى بريدك الإلكتروني!";
            return RedirectToAction("VerifyOTP", new { userId = user.Id });
        }

        [HttpGet]
        public IActionResult VerifyOTP(string userId)
        {
            var verifyOTPVM = new VerifyOTPVM
            {
                UserId = userId
            };
            return View(verifyOTPVM);
        }


        [HttpPost]
        public async Task<IActionResult> VerifyOTP(VerifyOTPVM verifyOTPVM)
        {
            if (!ModelState.IsValid)
            {
                return View(verifyOTPVM);
            }

            var user = await _userManager.FindByIdAsync(verifyOTPVM.UserId);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid UserNameOrEmail");
                return View(verifyOTPVM);
            }

            var applicationUserOTP = await _applicationUserOTP.GetAllAsync(a =>
            a.ApplicationUserId == verifyOTPVM.UserId
            && a.IsValid
            && a.ValidTo >= DateTime.UtcNow

            );
            var otp = applicationUserOTP.OrderBy(a => a.CreatedAt).LastOrDefault();
            if (otp == null || otp.OTP != verifyOTPVM.OTPCode)
            {
                ModelState.AddModelError("", "Invalid OTP");
                return View(verifyOTPVM);
            }
            otp.IsValid = false;
            await _applicationUserOTP.SaveAsync();
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return RedirectToAction("ResetPassword", new { userId = verifyOTPVM.UserId, token });
        }

        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
        {

            return View(new ResetPasswordVM { UserId = userId, Token = token });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPasswordVM);
            }
            var user = await _userManager.FindByIdAsync(resetPasswordVM.UserId);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid UserNameOrEmail");
                return View(resetPasswordVM);
            }
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordVM.Token, resetPasswordVM.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(resetPasswordVM);
            }
            TempData["Successful_Notification"] = "تم إعادة تعيين كلمة المرور بنجاح. يمكنك الآن تسجيل الدخول باستخدام كلمة المرور الجديدة.";
            return RedirectToAction("Login");
        }



    }
}
