namespace PontoCalculator.Models
{
    public class Ponto
    {
        public int Id { get; set; }
        public int User_id { get; set; }
        public DateTime DateTime { get; set; }
        public required bool In_out { get; set; }
        public string? Details { get; set; }
    }
}
