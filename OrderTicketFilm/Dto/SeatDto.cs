namespace OrderTicketFilm.Dto
{
    public class SeatDto
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int RoomId { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

    }

    public class SeatView : SeatDto
    {
        public int Id { get; set; }
    }

    public class SeatShow : SeatView 
    {
        public string Status { get; set; }
        public string Code { get; set; }
    }


    public class LayoutSeat
    {
        public List<ColumnSeat> columnSeats { get; set; }
    }

    public class ColumnSeat
    {
        public int Column { get; set; }
        public List<SeatShow> seatOfShowTimes { get; set; }
    }

}
