using Blog_System.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Blog_System.Controllers
{
    public class SearchController : Controller
    {
        private readonly IUserRepository _userRepo;

        public SearchController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }
        public async Task<IActionResult> SearchUsers(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return NotFound();
            }

            var result = _userRepo.GetBySearch(query);

            return View(result);
        }
    }
}
