using Blog_System.Models.Entities;
using Blog_System.Repositories;
using Blog_System.Servicies;
using Blog_System.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Client;
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
        private readonly IWebHostEnvironment _hosting; // Image عشان ال 
        private readonly IPostRepository _postRepository;
        private readonly IFollowService _followService;

        public ProfileController(UserManager<UserApplication> userManager,
            IWebHostEnvironment hosting, IPostRepository postRepository, IFollowService followService)
        {
            _userManager = userManager;
            _hosting = hosting;
            _postRepository = postRepository;
            _followService = followService;
        }


        [HttpGet]
        public async Task<IActionResult> Profile(string? id)
        {
            UserApplication user;

            if (string.IsNullOrEmpty(id)) // مسجل دخول اصلا logged-in يعني المستخدم 
            {
                // User(In controller) == HttpContext.User(any where but not in controller)
                // => cookie اللي متخزنه في ال Claim عن طريق ال User بيحتوي علي كل المعلومات الخاصه بال ClaimsPrincipal المستخدم اللي مسجل دخول نوعه 
                // GetUserAsync(User) => دا من الداتابيز User من ال object هيرجع

                // Cookie من ال User الطريقه الاولي عشان اجيب بيها ال 
                user = await _userManager.GetUserAsync(User);


                // Cookie من ال User الطريقه التانيه عشان اجيب بيها ال 
                //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //user = await _userManager.FindByIdAsync(userId);
            }
            else
            {
                // دا بتاعه من الداتا بيز id اللي ال user دا موجود هات ال id يعني لو ال 
                user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
            }

            var currentUserId = _userManager.GetUserId(User);

            var isFollowing = await _followService.IsFollowingAsync(currentUserId, id);

            ProfileViewModel result = new ProfileViewModel();
            result.Email = user.Email;
            result.BirthDate = user.BirthDate;
            result.FirstName = user.FirstName;
            result.LastName = user.LastName;
            result.UserName = user.UserName;
            result.Image = user.Image;
            result.UserId = user.Id;

            result.IsOwner = currentUserId == user.Id;

            result.UserApplication = user;
            result.IsFollow = isFollowing;

            result.CountFollowers = await _followService.GetFollowerCountAsync(id);
            result.CountFollowings = await _followService.GetFollowingCountAsync(id);

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

        // اللي هيشاور عليها Method بتاع ال Signature بياخد نفس ال 
        public delegate Task AddPostDelegate(PostViewModel post);

        // Instance of delegate => Method عشان اشاور بيه علي ال 
        public AddPostDelegate addPostDelegate;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(ProfileViewModel userProfile)
        {

            var user = await _userManager.GetUserAsync(User);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            string oldImage = user.Image;

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
                        string uploads = Path.Combine(_hosting.WebRootPath, "Uploads");

                        // للحصول علي اسم الملف
                        fileName = userProfile.ImageFile.FileName;

                        string fullPath = Path.Combine(uploads, fileName);

                        // نقوم بإنشاء الملف fullPath بعد ما نديله ال 
                        await userProfile.ImageFile.CopyToAsync(new FileStream(fullPath, FileMode.Create));

                        user.Image = fileName;

                        // craete post by old image if found
                        if (oldImage != null)
                        {
                            var oldPath = Path.Combine(uploads, oldImage);
                            if (System.IO.File.Exists(oldPath))
                            {
                                var newPost = new Post
                                {
                                    ImageURL = oldImage,
                                    Title = null,
                                    Content = "Old Profile Photo",
                                    PublichDate = DateTime.Now,
                                    Visible = true,
                                    UserId = userId
                                };
                                await _postRepository.Add(newPost);
                                await _postRepository.Save();
                            }
                        }
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
