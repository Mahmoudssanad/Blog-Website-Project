using Blog_System.Models.Entities;

namespace Blog_System.Servicies
{
    public interface IFollowService
    {
        Task AddAsync(string followerId, string followingId);

        Task Delete(string followerId, string followingId);

        Task<int> GetFollowerCountAsync(string userId);

        Task<int> GetFollowingCountAsync(string userId);

        Task<List<UserApplication>> GetFollwersAsync(string userId);

        Task<List<UserApplication>> GetFollwingsAsync(string userId);

        Task<bool> IsFollowingAsync(string followerId, string followingId);

    }
}
