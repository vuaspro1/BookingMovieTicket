using System.ComponentModel.DataAnnotations;

namespace OrderTicketFilm.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public RoomStatus RoomStatus { get; set; }
        public virtual ICollection<Seat>? Seats { get; set; }
        public virtual ICollection<ShowTime>? ShowTimes { get; set; }
    }
}
