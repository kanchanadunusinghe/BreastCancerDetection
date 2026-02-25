using BCD.Domain.Enums;

namespace BCD.Domain.Entities
{
    public class Patient
    {
        public int Id { get; set; }

        public string NHSNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string PostCode { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<MammographyScan> MammographyScans { get; set; } = new List<MammographyScan>();
    }
}
