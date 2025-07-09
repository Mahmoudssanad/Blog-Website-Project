using Blog_System.Models.Entities;
using Blog_System.ViewModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Blog_System.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<UserApplication> _userManager;
        private readonly IHttpContextAccessor _httpContext;
        public AccountRepository(UserManager<UserApplication> userManager, IHttpContextAccessor httpContext)
        {
            _userManager = userManager;
            _httpContext = httpContext;
        }

        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordViewModel model)
        {
            // Cookie من ال UserId بروح اجيب ال 
            var userId = _httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            // بشوفه موجود في الداتا بيز ولا لا 
            var user = await _userManager.FindByIdAsync(userId);

            return await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

        }
    }
}
