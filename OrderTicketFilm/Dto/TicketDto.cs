﻿namespace OrderTicketFilm.Dto
{
    public class TicketDto
    {
        public int? SeatId { get; set; }
        public int? ShowTimeId { get; set; }
        public int BillId { get; set; }
    }

    public class TicketView : TicketDto
    {
        public int Id { get; set; }
        public DateTime ShowTime { get; set; }
        public string SeatName { get; set; }
        public string FilmName { get; set; }
        public string RoomName { get; set; }
        public int Price { get; set; }
        public int RoomId { get; set; }
    }


}
