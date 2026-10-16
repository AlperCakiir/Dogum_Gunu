namespace Hafi_Abi_Doğum_Günü.Models
{
    public class Rsvp
    {
        public int Id { get; set; }
        public string GuestName { get; set; } = string.Empty;
        public bool IsAttending { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
