using AspNetCoreGeneratedDocument;
using Blog_System.Models.Data;
using Blog_System.Models.Entities;
using Blog_System.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Blog_System.Servicies
{
    public class CommentService : ICommentService
    {
        private readonly AppDbContext _context;
        private readonly ILikeService _likeService;
        private readonly IPostRepository _postRepository;

        public CommentService(AppDbContext context, ILikeService likeService, IPostRepository postRepository)
        {
            _context = context;
            _likeService = likeService;
            _postRepository = postRepository;
        }

        public async Task AddAsync(Comment comment)
        {
            if (comment == null)
                throw new ArgumentNullException(nameof(comment));

            Comment comment1 = new Comment()
            {
                CreatedAt = DateTime.Now,
                Content = comment.Content,
                IsEdited = false,
                UserId = comment.UserId,
                PostId = comment.PostId
            };
                
            await _context.Comments.AddAsync(comment1);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var found = await _context.Comments.FindAsync(id);

            if(found == null) 
                return;

            _context.Comments.Remove(found);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCommentLikeCountAsync(int commentId)
        { 
            var count = await _likeService.GetCommentLikesCountAsync(commentId);

            return count;
        }

        public async Task<List<Comment>> GetCommentsAsync(int postId)
        {
            if(postId == 0)
                throw new ArgumentNullException(nameof(postId));

            var comments = await _context.Comments
                .Include(x => x.Likes)
                .Include(x => x.UserApplication)
                .Where(x => x.PostId == postId).ToListAsync();

            return comments;
        }

        public async Task<bool> IsCommentOwnedByUserAsync(int commentId, string userId)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == commentId);

            if(comment != null)
            {
                //var user = await _context.Comments.FirstOrDefaultAsync(x => x.UserId == userId);

                return comment.UserId == userId;
            }
            return false;
        }

        public async Task UpdateAsync(Comment comment, int id)
        {
            var exist = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);

            if (exist == null)
                return;

            exist.UserId = comment.UserId;
            exist.Content = comment.Content;
            exist.CreatedAt = DateTime.Now;
            exist.IsEdited = true;
            exist.PostId = comment.PostId;

            await _context.SaveChangesAsync();
        }

        public async Task<Post> PostDetail(int postId)
        {
            var post = await _postRepository.GetById(postId);

            return post;
        }
    }
}
