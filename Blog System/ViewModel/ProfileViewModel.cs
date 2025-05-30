namespace Blog_System.ViewModel
{
    public class ProfileViewModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Image { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Email { get; set; }
    }
}
