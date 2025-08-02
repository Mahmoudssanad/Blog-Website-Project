using Blog_System.Models.Entities;

namespace Blog_System.Servicies
{
    public interface ICommentService
    {
        Task AddAsync(Comment comment);

        Task UpdateAsync(Comment comment, int id);

        Task DeleteAsync(int id);

        Task<List<Comment>> GetCommentsAsync(int postId);

        Task<int> GetCommentLikeCountAsync(int commentId);

        Task<bool> IsCommentOwnedByUserAsync(int commentId, string userId);

        Task<Post> PostDetail(int postId);
    }
}
