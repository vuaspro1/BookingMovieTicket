using System.ComponentModel.DataAnnotations;

namespace OrderTicketFilm.Models
{
    public class Seat
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public Room Room { get; set; }
        public virtual ICollection<Ticket>? Tickets { get; set; }
    }
}
