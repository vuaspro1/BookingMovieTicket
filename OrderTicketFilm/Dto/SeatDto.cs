namespace OrderTicketFilm.Dto
{
    public class SeatDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Status { get; set; }
        public int Price { get; set; }
        public int RoomId { get; set; }
    }
}
