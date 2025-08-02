using System.ComponentModel.DataAnnotations.Schema;

namespace Blog_System.Models.Entities
{
    public class Follow
    {
        public DateTime FollowDate { get; set; }

        [ForeignKey("Follower")]
        public string FollowerId { get; set; }
        public UserApplication Follower { get; set; }


        [ForeignKey("Following")]
        public string FollowingId { get; set; }
        public UserApplication Following { get; set; }
    }
}
