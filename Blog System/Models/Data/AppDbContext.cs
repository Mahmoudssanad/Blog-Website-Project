using Blog_System.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Blog_System.Models.Data
{
    public class AppDbContext : IdentityDbContext<UserApplication>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Friend> Friends { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<EmailOtp> EmailOtps { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Create Composite Primary key in Follows table
            builder.Entity<Follow>()
                .HasKey(x => new { x.FollowingId, x.FollowerId });

            builder.Entity<Follow>()
                .HasOne(x => x.Following).WithMany() // الشخص اللي تم متابعته يقدر يتتابع من ناس كتير  
                .HasForeignKey(x => x.FollowingId)   // ForeignKey => UserApplication بال Follow اللي بيربط ال 
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Follow>()
                .HasOne(x => x.Follower).WithMany() // الشخص اللي بيتابع يقدر يتابع ناس كتير 
                .HasForeignKey(x => x.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);


             // الواحد Post واحد بس علي ال lIKE الواحد يقدر يعمل User عشان ال 
            builder.Entity<Like>()
                .HasIndex(l => new { l.UserId, l.PostId })
                .IsUnique()
                .HasFilter("[PostId] IS NOT NULL");

            // الواحد Comment واحد بس علي ال lIKE الواحد يقدر يعمل User عشان ال 
            builder.Entity<Like>()
                .HasIndex(l => new { l.UserId, l.CommentId })
                .IsUnique()
                .HasFilter("[CommentId] IS NOT NULL");


            //builder.Entity<Like>()
            //    .HasKey(x => new { x.UserId, x.PostId });

        }
    }
}
