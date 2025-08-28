

using Blog_System.Models.Entities;
using Blog_System.Repositories;
using Blog_System.Servicies;
using Blog_System.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Blog_System.Controllers
{
    // Can you tell me registeration steps?
    //      1- اللي هخزن فيه class الي ال ViewModel بنقل الداتا من 
    //      2- بعمل انشاء مستخدم جديد داخل الداتا بيز 
    //      3- cookie بشوف هل هو موجود قبل كدا ولا لا لو مش موجود بعمله  


    // Can you explaination Login Steps?
    //      1- هشوف الايميل دا موجود ولا لا في الداتا بيز 
    //      2- لو موجود هشوف الباسورد صح ولا غلط 
    //      3- واخزن بياناته Cookie لو صح هعمله 
    public class AccountController : Controller
    {
        private readonly UserManager<UserApplication> _userManager;
        private readonly IAccountRepository _accountRepo; // Because ChangePassword function only
        private readonly IEmailService _emailService;
        private readonly SignInManager<UserApplication> _signInManager; // when you need save data in cookie

        public AccountController(UserManager<UserApplication> userManager,
            SignInManager<UserApplication> signInManager, IAccountRepository accountRepo, IEmailService emailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _accountRepo = accountRepo;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveRegister(RegisterViewModel userRegisteration)
        {
            // 1- UserApplication class نحطها في ال ViewModel هناخد الداتا من ال 
            //UserApplication userApplication = new UserApplication();

            if (ModelState.IsValid)
            {
                var found = await _userManager.FindByEmailAsync(userRegisteration.Email);

                if(found != null)
                {
                    // بيعمل كدا هل اقدر استغا عنه؟؟Custome validation طب انا عامل اصلا Email لل duplicate هنا عشان ميعملش 
                    ModelState.AddModelError("", "Email Already Exist");
                    return View("Register", userRegisteration);
                }

                // Save register data in session
                HttpContext.Session.SetString("TempRegister", JsonConvert.SerializeObject(userRegisteration));

                // Generate otp
                var otp = new Random().Next(100000, 999999).ToString();

                // Send otp to user email
                await _emailService.SendEmailAsync(userRegisteration.Email, "Confirm Your Email on Vivena website",
                    $"Welcome {userRegisteration.UserName}! \n Your OTP is: {otp}");

                var emailotp = new EmailOtp
                {
                    Email = userRegisteration.Email,
                    ExpiryTime = DateTime.UtcNow.AddMinutes(5),
                    OtpCode = otp,
                    IsUsed = false,
                };

                await _emailService.Add(emailotp);

                return RedirectToAction("VerifyOtp", new { Email = userRegisteration.Email });
            }
                return View("register", userRegisteration);
        }

        [HttpGet]
        public IActionResult VerifyOtp(string email)
        {
            return View(new OtpViewModel { Email = email});        
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> VerifyOtp(OtpViewModel model)
        {
            var found = await _emailService.FindByEmailAsync(model.Email);

            if (found != null && !found.IsUsed)
            {
                if(model.Otp == found.OtpCode && found.ExpiryTime > DateTime.UtcNow)
                {
                    var registerModel = JsonConvert.DeserializeObject<RegisterViewModel>(
                        HttpContext.Session.GetString("TempRegister"));

                    var user = new UserApplication
                    {
                        Email = registerModel.Email,
                        UserName = registerModel.UserName,
                        FirstName = registerModel.FirstName,
                        LastName = registerModel.LastName,
                        BirthDate = registerModel.BirthDate,
                        PasswordHash = registerModel.Password
                    };

                    // 2- Save new register in database by CreateAsync function
                    var result = await _userManager.CreateAsync(user, registerModel.Password);

                    if (result.Succeeded)
                    {
                        found.IsUsed = true;
                        await _emailService.UpdateAsync(found);

                        // Create Cookie and save this user data into this cookie by SignInAsync
                        await _signInManager.SignInAsync(user, true);

                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError("", "Not correct or expired OTP");
            return View(model);
        }

        public async Task<IActionResult> ResendOtp(string email)
        {
            var found = await _emailService.FindByEmailAsync(email);

            if(found != null && !found.IsUsed)
            {
                var otp = new Random().Next(100000, 999999).ToString();
                found.ExpiryTime = DateTime.UtcNow.AddMinutes(2);
                found.OtpCode = otp;

                await _emailService.UpdateAsync(found);

                //// save register data in session
                var registerModel = JsonConvert.DeserializeObject<RegisterViewModel>(
                        HttpContext.Session.GetString("TempRegister"));

                await _emailService.SendEmailAsync(registerModel.Email, "Confirm Your Email on Vivena website",
                    $"Welcome {registerModel.UserName}! \n Your OTP is: {otp}");

                TempData["Message"] = "A new OTP has been sent to your email.";

                return RedirectToAction("VerifyOtp", new { email = registerModel.Email });
            }
            else
            {
                TempData["Message"] = "Error occurred. Please try again.";
                return RedirectToAction("VerifyOtp", new {email = email});
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveLogin(LoginViewModel userLogin)
        {

            if (ModelState.IsValid)
            {
                // Check by email found in database or not
                  var result = await _userManager.FindByEmailAsync(userLogin.Email);
                    if (result != null)
                    {
                        // if email found => check password correct or not 
                        var found = await _userManager.CheckPasswordAsync(result, userLogin.Password);

                        if (found)
                        {
                            // put User Data in cookie
                            await _signInManager.SignInAsync(result, true);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    ModelState.AddModelError("", "Password or Email error.");
                    return View("Login", userLogin);
            }
            return View("Login", userLogin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepo.ChangePasswordAsync(model);

                if (result.Succeeded)
                {
                    ViewBag.IsSuccess = true;

                    // عشان لو راح يغيرها تاني ميبقاش جواه النتيجه بتاع المره اللي قبلها 
                    ModelState.Clear();

                    return View();
                }
                else
                {
                    ViewBag.IsSuccess = false;
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var found = await _userManager.FindByEmailAsync(model.Email);

                if(found != null)
                {
                    return RedirectToAction("VerifyOtp", new {email = found.Email});
                }
                ModelState.AddModelError("", "Email not found.");
            }
            return View(model);
        }

        
    }
}

