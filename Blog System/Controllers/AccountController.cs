

using Blog_System.Models.Entities;
using Blog_System.Repositories;
using Blog_System.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Security.Claims;
using System.Threading.Tasks;

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
        private readonly SignInManager<UserApplication> _signInManager; // when you need save data in cookie

        public AccountController(UserManager<UserApplication> userManager,
            SignInManager<UserApplication> signInManager, IAccountRepository accountRepo)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _accountRepo = accountRepo;
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
            UserApplication userApplication = new UserApplication();

            if (ModelState.IsValid)
            {
                userApplication.PasswordHash = userRegisteration.Password;
                userApplication.Email = userRegisteration.Email;
                userApplication.FirstName = userRegisteration.FirstName;
                userApplication.LastName = userRegisteration.LastName;
                userApplication.BirthDate = userRegisteration.BirthDate;
                userApplication.UserName = userRegisteration.UserName;

                var found = await _userManager.FindByEmailAsync(userApplication.Email);

                if(found != null)
                {
                    // بيعمل كدا هل اقدر استغا عنه؟؟Custome validation طب انا عامل اصلا Email لل duplicate هنا عشان ميعملش 
                    ModelState.AddModelError("", "Email Already Exist");
                }

                else
                {
                    // 2- Save new register in database by CreateAsync function
                    var result = await _userManager.CreateAsync(userApplication, userRegisteration.Password);

                    if (result.Succeeded)
                    {
                        // Create Cookie and save this user data into this cookie by SignInAsync
                        await _signInManager.SignInAsync(userApplication, true);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        throw new Exception("result not succeeded");
                    }
                }
            }
                return View("register", userRegisteration);
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
                            // but User Data in cookie
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
            return RedirectToAction("Login", "Account");
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
    }
}

