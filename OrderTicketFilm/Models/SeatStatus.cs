namespace OrderTicketFilm.Models
{
    public class SeatStatus
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }

        public virtual ICollection<Seat>? Seats { get; set; }
    }
}
