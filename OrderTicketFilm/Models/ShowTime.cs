using Microsoft.EntityFrameworkCore;
using OrderTicketFilm.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OrderTicketFilm.Models
{
    public class ShowTime
    {
        [Key]
        public int Id { get; set; }
        public DateTime Time { get; set; }

        public virtual Film? Film { get; set; }
        public virtual Room? Room { get; set; }

        public virtual ICollection<Ticket>? Tickets { get; set; }
    }

}


