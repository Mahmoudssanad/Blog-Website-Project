using Blog_System.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace Blog_System.Servicies
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly UserManager<UserApplication> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public CurrentUserService(UserManager<UserApplication> userManager, IHttpContextAccessor httpContext)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContext;
        }

        public IHttpContextAccessor HttpContext { get; }

        public Task<UserApplication> GetCurrentUserAsync()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var model = _userManager.GetUserAsync(user);
            return model;
        }
    }
}
