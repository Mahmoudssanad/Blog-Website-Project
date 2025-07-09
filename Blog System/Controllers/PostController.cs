using Blog_System.Models.Data;
using Blog_System.Models.Entities;
using Blog_System.Repositories;
using Blog_System.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog_System.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly IWebHostEnvironment _webHost;

        public PostController(IPostRepository postRepository, IWebHostEnvironment webHost)
        {
            _postRepository = postRepository;
            _webHost = webHost;
        }
        public IActionResult GetAll()
        {
            return View(_postRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Add(PostViewModel newPost)
        {
            if (ModelState.IsValid)
            {
                string imageFileName = null;
                if(newPost.ImageFile != null)
                {
                    var uploadFolder = Path.Combine(_webHost.WebRootPath, "Uploads");

                    imageFileName = newPost.ImageFile.FileName;

                    var filePath = Path.Combine(uploadFolder, imageFileName);

                    using var stream = new FileStream (filePath, FileMode.Create);
                    newPost.ImageFile.CopyTo(stream);
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


                var post = new Post
                {
                    ImageURL = imageFileName,
                    Title = newPost.Title,
                    Content = newPost.Content,
                    PublichDate = DateTime.Now,
                    Visible = newPost.Visible,
                    UserId = userId
                };
                if (post.Content == null & post.ImageURL == null)
                {
                    ModelState.AddModelError("", "empty post!!");
                    return View("Add", newPost);
                }
                try
                {
                    await _postRepository.Add(post);
                    return RedirectToAction("Index", "Home");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", ex.Message.ToString());
                }
            }
            return View("Add", newPost);
        }


        public async Task<IActionResult> ShowYourPosts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var posts = await _postRepository.GetByUserIdAndShowNotVisible(userId);
            return View(posts);
        }


        public async Task<IActionResult> ShowSpecificUserPosts(string id)
        {
            var posts = await _postRepository.GetByUserIdAndHideNotVisible(id);
            return View(posts);
        }


        public async Task<IActionResult> Delete(int id)
        {
            await _postRepository.Delete(id);
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        // This Action => Update عشان تحطلي القيم بتاع البوست اللي انا عاوز اعدله في الحقول بتاع صفحه ال 
        public async Task<IActionResult> Update(int id)
        {
            var found = await _postRepository.GetById(id);

            var postViewModel = new PostViewModel();

            postViewModel.Title = found.Title;
            postViewModel.Content = found.Content;
            postViewModel.Visible = found.Visible;

            // لو عاوز اعرضها في الصفحه View عشان ابعتها لل ViewBag عملتها في 
            // لان دا نوع ودا نوع ImageFile بال ImagerURL ال assign وعلشان مينفعش 
            // Update ممكن مكنتش عملتها اصلا بس الفكره انه مش هيظهرله الصوره القديمه قبل ما يعمل 
            ViewBag.ImageURL = found.ImageURL;

            return View(postViewModel);
        }


        [HttpPost]
        // دي بقا اللي بتشوف التعديلات اللي حصلت تمام ولا اي ولو تمام توديني في حته لو مش تمام بترجعني نفس الصفحه 
        public async Task<IActionResult> Update(PostViewModel newPost, int id)
        {

            if (!ModelState.IsValid)
                return View(newPost);

            var result = await _postRepository.Update(newPost, id);

            if (!result)
            {
                return NotFound();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
