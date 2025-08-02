using Blog_System.Models.Entities;

namespace Blog_System.Servicies
{
    public interface ILikeService
    {
        Task<bool> IsPostLikedByUser(string userId, int postId);
        Task<bool> IsCommentLikedByUser(string userId, int commentId);

        Task AddLikeToPost(string userId, int postId);
        Task AddLikeToComment(string userId, int commentId);


        Task DeleteLikeFromPost(string userId, int postId);
        Task DeleteLikeFromComment(string userId, int postId);


        Task<List<UserApplication>> GetUsersWhoLikedPost(int postId);
        Task<List<UserApplication>> GetUsersWhoLikedComment(int commentId);


        Task<int> GetPostLikesCountAsync(int postId);
        Task<int> GetCommentLikesCountAsync(int commentId);


        Task SaveAsync();
    }
}
