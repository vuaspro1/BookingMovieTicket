﻿using System.ComponentModel.DataAnnotations;

namespace OrderTicketFilm.Models
{
    public class Bill
    {
        [Key]
        public int Id { get; set; }
        public int PriceTotal { get; set; }
        public DateTime CreateDate { get; set; }
        public int Quantity { get; set; }
        
        public virtual ICollection<Ticket>? Tickets { get; set; }
        public Customer? Customer { get; set; }
        public User User { get; set; }
    }
}
