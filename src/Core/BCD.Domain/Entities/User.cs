namespace BCD.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public string GMCNumber { get; set; } = string.Empty;
        // For UK doctors (optional for some roles)

        public bool IsActive { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();


    }
}
