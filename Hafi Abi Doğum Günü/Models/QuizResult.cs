namespace Hafi_Abi_Doğum_Günü.Models
{
    public class QuizResult
    {
        public int Id { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public string PlayerTitle { get; set; } = string.Empty; // Örn: "🍰 Pasta Avcısı"
        public int Score { get; set; }
        public DateTime PlayedAt { get; set; } = DateTime.UtcNow;
    }
}
