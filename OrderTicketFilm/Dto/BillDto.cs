namespace OrderTicketFilm.Dto
{
    public class BillDto
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int CustomerId { get; set; }
        public int UserId { get; set; }
    }

    public class BillView : BillDto
    {
        public string? CustomerName { get; set; }
        public string? UserName { get; set; }
        public int PriceTotal { get; set; }
        public int Quantity { get; set; }
    }
}
