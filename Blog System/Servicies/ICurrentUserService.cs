using Blog_System.Models.Entities;

namespace Blog_System.Servicies
{
    public interface ICurrentUserService
    {
        Task<UserApplication> GetCurrentUserAsync();
    }
}
