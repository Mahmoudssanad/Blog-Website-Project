using Blog_System.Models.Entities;
using Blog_System.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IHostingEnvironment _hosting;

        public ProfileController(UserManager<UserApplication> userManager, IHostingEnvironment hosting)
        {
            _userManager = userManager;
            _hosting = hosting;
        }


        [HttpGet]
        public async Task<IActionResult> Profile(ProfileViewModel userProfile, string id)
        {
            var user = new UserApplication();
            if (id == null)
            {
                 user = await _userManager.GetUserAsync(User);

            }
            else
            {
                user = await _userManager.FindByIdAsync(id);
            }

            if(user == null)
            {
                return NotFound();
            }

            ProfileViewModel result = new ProfileViewModel();
            result.Email = user.Email;
            result.BirthDate = user.BirthDate;
            result.FirstName = user.FirstName;
            result.LastName = user.LastName;
            result.UserName = user.UserName;
            result.Image = user.Image;
            result.UserId = id;

            return View(result);
        }


        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var model = await _userManager.GetUserAsync(User);

            if (model == null)
                return NotFound();

            ProfileViewModel result = new ProfileViewModel();
            result.Email = model.Email;
            result.BirthDate = model.BirthDate;
            result.FirstName = model.FirstName;
            result.LastName = model.LastName;
            result.UserName = model.UserName;
            result.Image = model.Image;


            return View(result);
        }

            
        [HttpPost]
        [ValidateAntiForgeryToken] // => RdirectToAction وامتا اعمل return View امتا اعمل 
        public async Task<IActionResult> EditProfile(ProfileViewModel userProfile)
        {
            if(userProfile == null)
                 return View("EditProfile", userProfile);
            
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                string fileName = string.Empty;
                if(userProfile.ImageFile != null)
                {
                    // Upload اللي اليوزر هيعمل ليها Images بجيب الملف اللي هضع فيه ال 
                    string uploads = Path.Combine(_hosting.WebRootPath, "Images");

                    // للحصول علي اسم الملف
                    fileName = userProfile.ImageFile.FileName;

                    string fullPath = Path.Combine(uploads, fileName);

                    // نقوم بإنشاء الملف fullPath بعد ما نديله ال 
                    userProfile.ImageFile.CopyTo(new FileStream(fullPath, FileMode.Create));
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
                    return RedirectToAction("Profile");
                }
            }
            return View(userProfile);
        }
    }
}
