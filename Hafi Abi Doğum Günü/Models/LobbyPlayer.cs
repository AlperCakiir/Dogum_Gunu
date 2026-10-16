namespace Hafi_Abi_Doğum_Günü.Models
{
    public class LobbyPlayer
    {
        public string ConnectionId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int Score { get; set; } = 0;
        public bool IsHost { get; set; } = false; // Lobi Lideri mi?
        public bool IsTarget { get; set; } = false; // Bu turda "Hakkında soru sorulan Kurban/Hedef" mi?
        public string CurrentAnswer { get; set; } = string.Empty; // Soruya verdiği cevap / tahmin
    }
}