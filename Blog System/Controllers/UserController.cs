using Blog_System.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Blog_System.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<IActionResult> Index()
        {
            var allUsers = _userRepository.GetAll();
            return View(allUsers);
        }

        public IActionResult GetById(string id)
        {
            var found = _userRepository.GetById(id);
            return RedirectToAction("Profile", "Profile", found);
        }
    }
}
