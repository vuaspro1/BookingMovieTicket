namespace OrderTicketFilm.Dto
{
    public class BillDto
    {
        public DateTime CreateDate { get; set; }
        public int CustomerId { get; set; }
        public int UserId { get; set; }
    }

    public class BillView : BillDto
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public string UserName { get; set; }
        public int PriceTotal { get; set; }
        public int Quantity { get; set; }
    }

    public class BillBuilderDTO : BillDto
    {
        public List<TicketBuilder> TicketBuilders { get; set; }
    }

    public class TicketBuilder
    {
        public int? SeatId { get; set; }
        public int? ShowTimeId { get; set; }
    }
}
