using Blog_System.Models.Data;
using Blog_System.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Blog_System.Servicies
{
    public class FollowService : IFollowService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public FollowService(AppDbContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public async Task AddAsync(string followerId, string followingId)
        {

            Follow follow = new Follow();
            follow.FollowerId = followerId;
            follow.FollowingId = followingId;
            follow.FollowDate = DateTime.Now;

            var exists = await _context.Follows.AnyAsync(x => x.FollowerId == followerId && x.FollowingId == followingId);

            if (exists)
                return;

            _context.Follows.Add(follow);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(string followerId, string followingId)
        {
            var exists = await _context.Follows.FirstOrDefaultAsync(x => x.FollowingId == followingId & x.FollowerId == followerId);

            if (exists != null)
            {
                _context.Follows.Remove(exists);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetFollowerCountAsync(string userId) //  اللي هما متابعيني
        {
            var count = _context.Follows.Where(x => x.FollowingId == userId).Count();

            return count;
        }

        public async Task<int> GetFollowingCountAsync(string userId) // اللي انا متابعهم 
        {
            var count = _context.Follows.Where(x => x.FollowerId == userId).Count();

            return count;
        } 

        public async Task<List<UserApplication>> GetFollwersAsync(string userId)
        {
            // علشان ميجبش الشخص اللي عامل تسجيل دخول من ضمن المتابعين او العكس 
            var currentId = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var followers = await _context.Follows
                .Where(x => x.FollowingId == userId && x.FollowerId != currentId)
                .Include(x => x.Follower)
                .Select(x => x.Follower)
                .ToListAsync();

            return followers;
        }

        public async Task<List<UserApplication>> GetFollwingsAsync(string userId)
        {
            var currentId = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var followings = await _context.Follows
            .Where(f => f.FollowerId == userId && f.FollowingId != currentId)
            .Include(f => f.Following)
            .Select(f => f.Following)
            .ToListAsync();

            return followings;
        }

        public async Task<bool> IsFollowingAsync(string followerId, string followingId)
        {
            var exists = await _context.Follows.AnyAsync(x => x.FollowingId == followingId && x.FollowerId == followerId);

            if (exists)
            {
                return true;
            }
            return false;
        }
    }
}
