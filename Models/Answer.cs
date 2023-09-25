namespace FlightTickets.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public string AppUser { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
