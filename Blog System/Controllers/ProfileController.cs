using Blog_System.Models.Entities;
using Blog_System.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog_System.Controllers
{
    // هنشوف اليوزر دا موجود عندي ولا لا 
    public class ProfileController : Controller
    {
        private readonly UserManager<UserApplication> _userManager;
        public ProfileController(UserManager<UserApplication> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Profile(ProfileViewModel userProfile)
        {
            var user = await _userManager.GetUserAsync(User);

            if(user == null)
            {
                return NotFound();
            }

            var model = new ProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                BirthDate = user.BirthDate,
                Email = user.Email,
                Image = user.Image,
            };
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var model = await _userManager.GetUserAsync(User);

            if (model == null)
                return NotFound();

            var result = new ProfileViewModel
            {
                Email = model.Email,
                BirthDate = model.BirthDate,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Image = model.Image
            };

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

            user.Email = userProfile.Email;
            user.BirthDate = userProfile.BirthDate;
            user.FirstName = userProfile.FirstName;
            user.LastName = userProfile.LastName;
            user.UserName = userProfile.UserName;
            user.Image = userProfile.Image;

            var saveResultInDatabase = await _userManager.UpdateAsync(user);

            if(saveResultInDatabase.Succeeded)
            {
                return RedirectToAction("Profile");
            }

            return View(userProfile);
        }
    }
}
