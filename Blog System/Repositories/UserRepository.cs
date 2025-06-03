using Blog_System.Models.Data;
using Blog_System.Models.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Blog_System.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
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
           return _context.Users.ToList();
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
