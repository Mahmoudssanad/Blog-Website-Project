using Blog_System.Models.Entities;
using Blog_System.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using static System.Net.Mime.MediaTypeNames;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Blog_System.Controllers
{
    // هنشوف اليوزر دا موجود عندي ولا لا 
    public class ProfileController : Controller
    {
        private readonly UserManager<UserApplication> _userManager;
        private readonly IHostingEnvironment hosting;

        public ProfileController(UserManager<UserApplication> userManager, IHostingEnvironment hosting)
        {
            _userManager = userManager;
            this.hosting = hosting;
        }

        [HttpGet]
        public async Task<IActionResult> Profile(ProfileViewModel userProfile)
        {
            var user = await _userManager.GetUserAsync(User);

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
            result.ImageFile = user.ImageFile;
            result.Image = user.Image;

            if (user.ImageFile != null)
            {
                result.Image = user.ImageFile.FileName;
            }

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
            //result.ImageFile = model.ImageFile;

            

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
                // Read File
                using (var ms = new MemoryStream())
                {
                    userProfile.ImageFile.CopyTo(ms);
                    string base64 = Convert.ToBase64String(ms.ToArray());

                    base64 = "data:" + userProfile.ImageFile.ContentType + ";base64," + base64;

                    userProfile.Image = base64;
                }


                user.Email = userProfile.Email;
                user.BirthDate = userProfile.BirthDate;
                user.FirstName = userProfile.FirstName;
                user.LastName = userProfile.LastName;
                user.UserName = userProfile.UserName;
                user.Image = userProfile.Image;

                //if (userProfile.ImageFile.FileName != null)
                //    user.Image = userProfile.ImageFile.FileName;

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
