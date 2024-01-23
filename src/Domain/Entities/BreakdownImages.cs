namespace Domain.Entities
{
    public class BreakdownImages
    {
        public long Id { get; set; }

        public int BreakdownId { get; set; }

        public string Image { get; set; } = string.Empty;

        public Breakdown? Breakdown { get; set; }
    }
}
