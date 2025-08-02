using Blog_System.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog_System.ViewModel
{
    public class ProfileViewModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [HiddenInput]
        public string? Image { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Email { get; set; }
        public IFormFile? ImageFile { get; set; }

        public string? UserId { get; set; }

        public bool IsOwner { get; set; } // To show/hide "Edit" button

        public bool IsFollow { get; set; }
        public UserApplication? UserApplication { get; set; }

        public int CountFollowers {  get; set; }
        public int CountFollowings {  get; set; }
    }
}
