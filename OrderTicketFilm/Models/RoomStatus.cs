using System.ComponentModel.DataAnnotations;

namespace OrderTicketFilm.Models
{
    public class RoomStatus
    {
        [Key]
        public int Id { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
