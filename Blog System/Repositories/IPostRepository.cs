using Blog_System.Models.Entities;
using Blog_System.ViewModel;

namespace Blog_System.Repositories
{
    // CRUD => Create, Read(Specific by id, All), Update(Specific by id), Delete(Specific by id)
    public interface IPostRepository
    {
        Task Add(Post post);

        Task<bool> Update(PostViewModel postModel, int id);

        Task Delete(int id);

        Task<Post> GetById(int id);

        List<Post> GetAll();

        Task<List<Post>> GetByUserIdAndShowNotVisible(string userId);

        Task<List<Post>> GetByUserIdAndHideNotVisible(string userId);
    }
}
