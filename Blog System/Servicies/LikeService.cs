using Blog_System.Models.Data;
using Blog_System.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Blog_System.Servicies
{
    public class LikeService : ILikeService
    {
        private readonly AppDbContext _context;

        public LikeService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<bool> IsPostLikedByUser(string userId, int postId)
        {
            var found = await _context.Likes.AnyAsync(x => x.UserId == userId && x.PostId == postId);

            return found;
        }

        public async Task<bool> IsCommentLikedByUser(string userId, int commentId)
        {
            var found = await _context.Likes.AnyAsync(x => x.UserId == userId && x.CommentId == commentId);

            return found;
        }


        public async Task AddLikeToComment(string userId, int commentId)
        {
            if(!await IsCommentLikedByUser(userId, commentId))
            {
                var like = new Like
                {
                    UserId = userId,
                    CommentId = commentId,
                    CreatedAt = DateTime.Now
                };
                await _context.Likes.AddAsync(like);
                await SaveAsync();
            }
        }

        public async Task AddLikeToPost(string userId, int postId)
        {
            if (!await IsPostLikedByUser(userId, postId))
            {
                var like = new Like
                {
                    UserId = userId,
                    PostId = postId,
                    CreatedAt = DateTime.Now
                };
                await _context.Likes.AddAsync(like);
                await SaveAsync();
            }
        }


        public async Task DeleteLikeFromComment(string userId, int commentId)
        {
            if(userId != null && commentId != 0)
            {
                var likeFound = await _context.Likes.FirstOrDefaultAsync(x => x.CommentId == commentId && x.UserId == userId);

                if(likeFound != null)
                {
                    _context.Likes.Remove(likeFound);
                    await SaveAsync();
                }
            }
        }

        public async Task DeleteLikeFromPost(string userId, int postId)
        {
            var like = await _context.Likes.FirstOrDefaultAsync(x => x.UserId == userId && x.PostId == postId);

            if(like != null)
            {
                _context.Likes.Remove(like);
                await SaveAsync();
            };
        }


        public async Task<int> GetCommentLikesCountAsync(int commentId)
        {
            var count = await _context.Likes.Where(x => x.CommentId == commentId).CountAsync();

            return count;
        }

        public async Task<int> GetPostLikesCountAsync(int postId)
        {
            var count = await _context.Likes.CountAsync(x => x.PostId == postId);

            return count;
        }


        public async Task<List<UserApplication>> GetUsersWhoLikedComment(int commentId)
        {
            var users = await _context.Likes.Include(x => x.UserApplication)
                .Where(x => x.CommentId == commentId)
                .Select(x => x.UserApplication).ToListAsync();

            return users;
        }

        public async Task<List<UserApplication>> GetUsersWhoLikedPost(int postId)
        {
            return await _context.Likes
                .Include(l => l.UserApplication)
                .Where(l => l.PostId == postId)
                .Select(l => l.UserApplication)
                .ToListAsync();
        }


        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
