namespace OrderTicketFilm.Dto
{
    public class SeatStatusDto : SeatStatusUpdate
    {
        public string Code { get; set; }
    }

    public class SeatStatusView : SeatStatusDto
    {
        public int Id { get; set; }
    }

    public class SeatStatusUpdate
    {
        public string Status { get; set; }
    }
}
