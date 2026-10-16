namespace Hafi_Abi_Doğum_Günü.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string OptionA { get; set; } = string.Empty;
        public string OptionB { get; set; } = string.Empty;
        public string OptionC { get; set; } = string.Empty;
        public string OptionD { get; set; } = string.Empty;
        public string CorrectOption { get; set; } = string.Empty; // "A", "B", "C" veya "D"
        public int OrderIndex { get; set; } // Soruların kaçıncı sırada çıkacağı
    }
}
