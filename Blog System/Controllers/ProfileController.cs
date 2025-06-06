using Blog_System.Models.Entities;
using Blog_System.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Blog_System.Controllers
{
    // هنشوف اليوزر دا موجود عندي ولا لا 

    //[Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<UserApplication> _userManager;
        private readonly IWebHostEnvironment _hosting;

        public ProfileController(UserManager<UserApplication> userManager, IWebHostEnvironment hosting)
        {
            _userManager = userManager;
            _hosting = hosting;
        }


        [HttpGet]
        public async Task<IActionResult> Profile(string? id)
        {
            UserApplication user;

            if (string.IsNullOrEmpty(id)) // مسجل دخول اصلا logged-in يعني المستخدم 
            {
                // User == HttpContext.User
                // => cookie اللي متخزنه في ال Claim عن طريق ال User بيحتوي علي كل المعلومات الخاصه بال ClaimsPrincipal المستخدم اللي مسجل دخول نوعه 
                // GetUserAsync(User) => دا من الداتابيز User من ال object هيرجع
                user = await _userManager.GetUserAsync(User);
            }
            else
            {
                user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
            }

            var currentUserId = _userManager.GetUserId(User);

            ProfileViewModel result = new ProfileViewModel();
            result.Email = user.Email;
            result.BirthDate = user.BirthDate;
            result.FirstName = user.FirstName;
            result.LastName = user.LastName;
            result.UserName = user.UserName;
            result.Image = user.Image;
            result.UserId = user.Id;
            result.IsOwner = currentUserId == user.Id;

            ViewBag.ProfileImage = result.Image;

            return View(result);
        }


        [HttpGet]
        public async Task<IActionResult> EditProfile(string id)
        {
            var model = await _userManager.FindByIdAsync(id);

            if (model == null)
                return NotFound();

            ProfileViewModel result = new ProfileViewModel();

            result.Email = model.Email;
            result.BirthDate = model.BirthDate;
            result.FirstName = model.FirstName;
            result.LastName = model.LastName;
            result.UserName = model.UserName;
            result.Image = model.Image;
            result.UserId = model.Id;


            return View(result);
        }

            
        [HttpPost]
        [ValidateAntiForgeryToken] // => RdirectToAction وامتا اعمل return View امتا اعمل 
        public async Task<IActionResult> EditProfile(ProfileViewModel userProfile)
        {   
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return NotFound();
            try
            {
                if (ModelState.IsValid)
                {
                    string fileName = string.Empty;

                    if (userProfile.ImageFile != null)
                    {
                        // Upload اللي اليوزر هيعمل ليها Images بجيب الملف اللي هضع فيه ال 
                        string uploads = Path.Combine(_hosting.WebRootPath, "Images");

                        // للحصول علي اسم الملف
                        fileName = userProfile.ImageFile.FileName;

                        string fullPath = Path.Combine(uploads, fileName);

                        // نقوم بإنشاء الملف fullPath بعد ما نديله ال 
                        userProfile.ImageFile.CopyTo(new FileStream(fullPath, FileMode.Create));

                        // delete old image if found
                        if (!string.IsNullOrEmpty(userProfile.Image))
                        {
                            var oldPath = Path.Combine(uploads, userProfile.Image);
                            if (System.IO.File.Exists(oldPath))
                                System.IO.File.Delete(oldPath);
                        }

                        user.Image = fileName;
                    }


                    user.Email = userProfile.Email;
                    user.BirthDate = userProfile.BirthDate;
                    user.FirstName = userProfile.FirstName;
                    user.LastName = userProfile.LastName;
                    user.UserName = userProfile.UserName;


                    var saveResultInDatabase = await _userManager.UpdateAsync(user);

                    if (saveResultInDatabase.Succeeded)
                    {
                        // اصلا فلازم ابعته id parameter عشان يعرف هو هيعرض بروفايل مين والبروفايل اكشن بتاخد id لازم اباصي ليه ال 
                        return RedirectToAction("Profile", new { id = user.Id });
                    }

                    foreach (var error in saveResultInDatabase.Errors)
                    {
                        ModelState.AddModelError("", $"{error.Description}");
                    }
                }
            }
            catch(Exception ex)
            {
                return Content($"{ex.Message}");
                
            }
            
            return View(userProfile);
        }
    }
}
