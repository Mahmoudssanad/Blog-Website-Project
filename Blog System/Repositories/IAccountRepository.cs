using Blog_System.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace Blog_System.Repositories
{
    public interface IAccountRepository
    {
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordViewModel model);
    }
}
