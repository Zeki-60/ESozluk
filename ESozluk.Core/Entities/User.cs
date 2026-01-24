namespace ESozluk.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime RegistrationDate { get; set; }
        public List<Like>? Likes { get; set; }
        public List<Topic>? Topics { get; set; }
        public List<Entry>? Entries { get; set; }
        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public string? PasswordResetToken { get; set; } 
        public DateTime? PasswordResetTokenExpires { get; set; } 

    }
}
