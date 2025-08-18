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
            var users = await _userRepo.GetFollowandUnfollowUsers();

            if (string.IsNullOrEmpty(query))
            {
                return Content(""); // يرجع فاضي لو البحث فاضي
            }

            users = users.Where(x => x.UserName.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();

            return PartialView("_UsersListPartial", users);
        }

    }
}
