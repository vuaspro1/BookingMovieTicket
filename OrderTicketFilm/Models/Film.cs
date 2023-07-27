﻿using System.ComponentModel.DataAnnotations;

namespace OrderTicketFilm.Models
{
    public class Film
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime OpeningDay { get; set; }
        public string Director { get; set; }
        public string Time { get; set; }
        public byte[] Image { get; set; }
        public TypeOfFilm? TypeOfFilm { get; set; }
        public virtual ICollection<ShowTime>? ShowTimes { get; set; }
    }
}