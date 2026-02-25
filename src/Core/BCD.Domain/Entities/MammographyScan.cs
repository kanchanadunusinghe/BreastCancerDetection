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
    }
}
