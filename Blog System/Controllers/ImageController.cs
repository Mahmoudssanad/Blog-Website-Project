using Blog_System.Models.Entities;
using Blog_System.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;

namespace Blog_System.Controllers
{
    public class ImageController : Controller
    {
        public IWebHostEnvironment _webHost { get; }

        public ImageController(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }


        public async Task<IActionResult> Index(ImageUploadViewModel imageUpload)
        {
            
            return View();
        }

        public async Task<IActionResult> UploadAsync(ImageUploadViewModel imageUpload)
        {
            if (imageUpload.ImageFile != null && imageUpload.ImageFile.Length > 0)
            {
                string uploadfolder = Path.Combine(_webHost.WebRootPath, "Images");
                Directory.CreateDirectory(uploadfolder);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageUpload.ImageFile.FileName);

                string fullPath = Path.Combine(uploadfolder, fileName);

                using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await imageUpload.ImageFile.CopyToAsync(fileStream);
                }

                UserApplication userApplication = new UserApplication();
                userApplication.Image = fileName;
                return View("profile", "profile");
            }
            else
            {
                return View("Index", "Home");
            }
                
        }
    }
   
}
