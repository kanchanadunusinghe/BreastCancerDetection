namespace BCD.Domain.Entities
{
    public class MammographyScan
    {
        public int Id { get; set; }

        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public string PredictionResult { get; set; } = string.Empty;

        public decimal ConfidenceScore { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? Comment { get; set; } = string.Empty;

        public int? CreatedUserId { get; set; }
        public virtual User? CreatedUser { get; set; }

    }
}
