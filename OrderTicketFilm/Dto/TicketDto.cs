namespace OrderTicketFilm.Dto
{
    public class TicketDto
    {
        public int Id { get; set; }
        public int SeatId { get; set; }
        public int ShowTimeId { get; set; }
        public int BillId { get; set; }
    }

    public class TicketView : TicketDto
    {
        public DateTime ShowTime { get; set; }
        public string SeatName { get; set; }
        public string FilmName { get; set; }
        public string RoomName { get; set; }
        public int Price { get; set; }
    }
}
