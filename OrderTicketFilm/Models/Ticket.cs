using Microsoft.EntityFrameworkCore;
using OrderTicketFilm.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OrderTicketFilm.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        public int Price { get; set; }

        public ShowTime ShowTime { get; set; }
        public Seat Seat { get; set; }
        public Bill Bill { get; set; }

        //public int? ShowTimeId { get; set; }
        //public int? SeatId { get; set; }
        //public int? CustomerId { get; set; }
        //public int? UserId { get; set; }

        //[ForeignKey("ShowTimeId")]
        //public virtual ShowTime? ShowTime { get; set; }

        //[ForeignKey("SeatId")]
        //public virtual Seat? Seat { get; set; }
        //[ForeignKey("CustomerId")]
        //public virtual Customer? Customer { get; set; }
        //[ForeignKey("UserId")]
        //public virtual User? User { get; set; }
    }
}
