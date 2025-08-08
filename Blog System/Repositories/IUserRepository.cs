using Blog_System.Models.Entities;

namespace Blog_System.Repositories
{
    public interface IUserRepository
    {
        // CRUD Operations

        public Task<List<UserApplication>> GetAll();
        public Task<List<UserApplication>> GetBySearch(string query);
        public UserApplication GetById(string Id);
        public void Add(UserApplication userApplication);
        public void update(UserApplication userApplication, string Id);
        public void delete(string Id);
        public void Save();
    }
}
