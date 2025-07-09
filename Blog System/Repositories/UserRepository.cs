using Blog_System.Models.Data;
using Blog_System.Models.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Security.Principal;

namespace Blog_System.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public UserRepository(AppDbContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }


        public void Add(UserApplication userApplication)
        {
            throw new NotImplementedException();
        }

        public void delete(string Id)
        {
            throw new NotImplementedException();
        }

        public List<UserApplication> GetAll()
        {
            // ماعدا اللي هو عامل تسجيل دخول Users عشان يجيب كل ال 
            var userId = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = _context.Users.Where(x => x.Id != userId).ToList();

            return result;
        }

        public UserApplication GetById(string Id)
        {
            var found = _context.Users.FirstOrDefault(x => x.Id ==  Id);
            if(found == null)
            {
                return null;
            }
            else
                return found;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void update(UserApplication userApplication, string Id)
        {
            throw new NotImplementedException();
        }
    }
}
