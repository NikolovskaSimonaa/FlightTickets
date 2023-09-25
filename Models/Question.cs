namespace FlightTickets.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public string AppUser { get; set; }
        public ICollection<Answer>? Answers { get; set; }
    }
}
