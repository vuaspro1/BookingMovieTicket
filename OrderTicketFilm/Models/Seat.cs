using System.ComponentModel.DataAnnotations;

namespace OrderTicketFilm.Models
{
    public class Seat
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Status { get; set; }
        public virtual Room? Room { get; set; }
        public virtual ICollection<Ticket>? Tickets { get; set; }
    }
}
