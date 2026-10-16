using Hafi_Abi_Doğum_Günü.Models;

namespace Hafi_Abi_Doğum_Günü.Models.ViewModel
{
    public class HomeIndexViewModel
    {
        public List<Rsvp> Rsvps { get; set; } = new List<Rsvp>();
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}