using System.ComponentModel.DataAnnotations;

namespace OrderTicketFilm.Models
{
    public class ShowTime
    {
        [Key]
        public int Id { get; set; }
        public DateTime Time { get; set; }

        public Film Film { get; set; }
        public Room Room { get; set; }

        public virtual ICollection<Ticket>? Tickets { get; set; }
    }

}


