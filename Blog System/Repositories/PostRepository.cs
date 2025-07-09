using Blog_System.Models.Data;
using Blog_System.Models.Entities;
using Blog_System.ViewModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Blog_System.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IWebHostEnvironment _webHost;

        // هتظهرله ازي في الصفحه بتاعته not visible البوستات اللي المستخدم عاملها 
        public PostRepository(AppDbContext context, IHttpContextAccessor httpContext, IWebHostEnvironment webHost)
        {
            _context = context;
            _httpContext = httpContext;
            _webHost = webHost;
        }


        public async Task Add(Post post)
        {
            if(post != null)
            {
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var user =  await _context.Posts.FirstOrDefaultAsync(x => x.Id == id);
            if (user != null)
            {
                _context.Posts.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public List<Post> GetAll()
        {
            // Include(x => x.UserId) => navigation property بتاعه, عشان كدا لازم يكون فيه post مع ال User عشان اجيب بيانات ال 

            var posts =  _context.Posts.Where(x => x.Visible).Include(x => x.UserApplication)
                .ToList();

            return posts;
        }

        public async Task<Post> GetById(int id)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == id);

            return post;
        }

        public async Task<List<Post>> GetByUserIdAndShowNotVisible(string userId)
        {
            var postsByUserId = await _context.Posts
                .Where(x => x.UserId == userId)
                .Include(x => x.UserApplication)
                .ToListAsync();

            return  postsByUserId;
        }

        // ده userId المشكله اللي انا مش فاهمها اني هباصي ازي ال 
        public async Task<List<Post>> GetByUserIdAndHideNotVisible(string userId)
        {
            var postsByUserId = await _context.Posts
                .Where(x => x.Visible)
                .Where(x => x.UserId == userId)
                .Include(x => x.UserApplication)
                .ToListAsync();

            return postsByUserId;
        }

        // بستقبل فيها التغيرات اللي المستخدم عملها في البوست ورقم البوست القديم 
        // عشان اجيب البوست القديم واحط فيه القيم الجديده اللي انا مستقبلها منه 
        // ازي Update Upload Image خش شوف بقا بتعمل 
        public async Task<bool> Update(PostViewModel newPostModel, int id)
        {
            // Old Reference
            var oldPost = await _context.Posts.FirstOrDefaultAsync(x => x.Id == id);

            if (oldPost == null)
                return false;

            var userId = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var publishDate = oldPost.PublichDate;

            // Upload or update image
            if(newPostModel.ImageFile != null)
            {
                var fileUploads = Path.Combine(_webHost.WebRootPath, "Uploads");

                var imageFile = newPostModel.ImageFile.FileName;

                var fullPath = Path.Combine(fileUploads, imageFile);

                await newPostModel.ImageFile.CopyToAsync(new FileStream(fullPath,FileMode.Create));

                // عشان يحذف القديمه ميسبهاش واخده مساحه 
                if (!string.IsNullOrEmpty(oldPost.ImageURL))
                {
                    var oldPath = Path.Combine(oldPost.ImageURL, "Uploads");
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }
                oldPost.ImageURL = imageFile;
            }

            oldPost.Title = newPostModel.Title;
            oldPost.Content = newPostModel.Content;
            oldPost.Visible = newPostModel.Visible;

            oldPost.UserId = userId;
            oldPost.PublichDate = publishDate;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
